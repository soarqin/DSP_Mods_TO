using System;
using System.IO;
using System.Linq;
using System.Threading;
using CompressSave.Wrapper;

internal static class Program
{
    private static int Main()
    {
        try
        {
            WritesCompleteStreamBeforeClose();
            DisposeFinalizesStream();
            WriterDisposeThenStreamDisposeFinalizesStream();
            SynchronousPathRemainsComplete();
            ImmediateClosePreservesTail();
            CloseWaitsForBlockedWorkerWrite();
            CloseDrainsQueuedFinalBuffer();
            Console.WriteLine("PASS");
            return 0;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return 1;
        }
    }

    private static void WritesCompleteStreamBeforeClose()
    {
        var wrapper = new FakeWrapper();
        var output = new MemoryStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: true);
        var input = Enumerable.Range(0, 53).Select(i => (byte)(i * 3 + 1)).ToArray();

        stream.Write(input, 0, input.Length);
        stream.Close();
        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();

        Assert(output.ToArray().SequenceEqual(expected),
            "async save must contain header, every input byte, and trailer");
        Assert(wrapper.EndCount == 1, "CompressEnd must run exactly once");
        Assert(wrapper.ContextFreeCount == 1, "compression context must be released exactly once");

        stream.Dispose();
        Assert(output.ToArray().SequenceEqual(expected),
            "repeated Dispose must not append or remove bytes");
        Assert(wrapper.EndCount == 1, "repeated Dispose must not finalize twice");
    }

    private static void DisposeFinalizesStream()
    {
        var wrapper = new FakeWrapper();
        var output = new MemoryStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: true);
        var input = new byte[] { 7, 8, 9 };

        stream.Write(input, 0, input.Length);
        stream.Dispose();

        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
        Assert(output.ToArray().SequenceEqual(expected),
            "Dispose must finalize the stream and preserve the complete save");
        Assert(wrapper.EndCount == 1, "Dispose must call CompressEnd exactly once");
    }

    private static void WriterDisposeThenStreamDisposeFinalizesStream()
    {
        var wrapper = new FakeWrapper();
        var output = new MemoryStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: true);
        var input = new byte[] { 10, 11, 12 };

        stream.Write(input, 0, input.Length);
        stream.BufferWriter.Dispose();
        stream.Dispose();

        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
        Assert(output.ToArray().SequenceEqual(expected),
            "disposing the game writer before the compression stream must preserve the complete save");
        Assert(wrapper.EndCount == 1, "writer-first disposal must call CompressEnd exactly once");
    }

    private static void SynchronousPathRemainsComplete()
    {
        var wrapper = new FakeWrapper();
        var output = new MemoryStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: false);
        var input = Enumerable.Range(0, 37).Select(i => (byte)(i + 20)).ToArray();

        stream.Write(input, 0, input.Length);
        stream.Close();

        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
        Assert(output.ToArray().SequenceEqual(expected),
            "the synchronous compression path must retain its original complete output");
    }

    private static void CloseWaitsForBlockedWorkerWrite()
    {
        var wrapper = new FakeWrapper();
        var output = new GateStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: true);
        var input = Enumerable.Range(0, 16).Select(i => (byte)(i + 90)).ToArray();
        stream.Write(input, 0, input.Length);
        // Match the game's order: BinaryWriter.Dispose runs before the injected
        // compression-stream cleanup.
        stream.BufferWriter.Dispose();
        Assert(output.DataWriteEntered.Wait(5000),
            "worker did not enter the blocked output write");

        Exception closeFailure = null;
        var closeDone = new ManualResetEventSlim();
        var closeThread = new Thread(() =>
        {
            try
            {
                stream.Close();
            }
            catch (Exception e)
            {
                closeFailure = e;
            }
            finally
            {
                closeDone.Set();
            }
        });
        closeThread.Start();

        Assert(!closeDone.Wait(100),
            "Close returned before the worker output completed");
        output.AllowDataWrite.Set();
        Assert(closeDone.Wait(5000), "Close did not finish after output was released");
        Assert(closeFailure == null, "blocked output close failed unexpectedly");

        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
        Assert(output.ToArray().SequenceEqual(expected),
            "blocked async output must remain complete and ordered");
        output.Close();
        Thread.Sleep(50);
        Assert(output.LateWriteCount == 0, "worker wrote after the output stream closed");
    }

    private static void ImmediateClosePreservesTail()
    {
        for (var iteration = 0; iteration < 50; iteration++)
        {
            var wrapper = new FakeWrapper();
            var output = new MemoryStream();
            var stream = new CompressionStream(
                wrapper,
                compressionLevel: 0,
                outputStream: output,
                compressBuffer: CompressionStream.CreateBuffer(1024, 16),
                multiThread: true);
            var input = new byte[] { (byte)iteration, 0xA5, 0x5A };

            stream.Write(input, 0, input.Length);
            stream.Close();

            var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
            Assert(output.ToArray().SequenceEqual(expected),
                "immediate close must not drop the final buffer");
        }
    }

    private static void CloseDrainsQueuedFinalBuffer()
    {
        var wrapper = new FakeWrapper();
        var output = new GateStream();
        var stream = new CompressionStream(
            wrapper,
            compressionLevel: 0,
            outputStream: output,
            compressBuffer: CompressionStream.CreateBuffer(1024, 16),
            multiThread: true);
        var input = Enumerable.Range(0, 17).Select(i => (byte)(i + 40)).ToArray();

        stream.Write(input, 0, input.Length);
        stream.BufferWriter.Dispose();
        Assert(output.DataWriteEntered.Wait(5000),
            "worker did not enter the first blocked output write");

        Exception closeFailure = null;
        var closeDone = new ManualResetEventSlim();
        var closeThread = new Thread(() =>
        {
            try
            {
                stream.Close();
            }
            catch (Exception e)
            {
                closeFailure = e;
            }
            finally
            {
                closeDone.Set();
            }
        });
        closeThread.Start();

        Assert(!closeDone.Wait(100),
            "Close returned before the queued final buffer was written");
        output.AllowDataWrite.Set();
        Assert(closeDone.Wait(5000), "Close did not finish after output was released");
        Assert(closeFailure == null, "queued final buffer close failed unexpectedly");

        var expected = FakeWrapper.Header.Concat(input).Concat(FakeWrapper.Trailer).ToArray();
        Assert(output.ToArray().SequenceEqual(expected),
            "Close must drain a final buffer queued while an earlier write is blocked");
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed unsafe class FakeWrapper : WrapperDefines
    {
        public static readonly byte[] Header = { 0xA1, 0xB2, 0xC3 };
        public static readonly byte[] Trailer = { 0xE1, 0xE2, 0xE3, 0xE4 };

        public int EndCount;
        public int ContextFreeCount;

        public FakeWrapper()
        {
            CompressBufferBound = size => size + 64;
            CompressBegin = (out IntPtr context, int level, byte[] output, long capacity, byte[] dictionary, long dictionarySize) =>
            {
                context = new IntPtr(1);
                Array.Copy(Header, output, Header.Length);
                return Header.Length;
            };
            CompressEnd = (IntPtr context, byte[] output, long capacity) =>
            {
                Interlocked.Increment(ref EndCount);
                Array.Copy(Trailer, output, Trailer.Length);
                return Trailer.Length;
            };
            CompressContextFree = context => Interlocked.Increment(ref ContextFreeCount);
            CompressUpdate = CopyInput;
        }

        private unsafe long CopyInput(IntPtr context, byte* destination, long destinationCapacity, byte* source, long sourceSize)
        {
            Thread.SpinWait(10000);
            for (var i = 0L; i < sourceSize; i++)
                destination[i] = source[i];
            return sourceSize;
        }
    }

    private sealed class GateStream : Stream
    {
        private readonly MemoryStream _inner = new();
        private readonly object _sync = new();
        private int _closed;
        private int _blockedDataWrite;

        public readonly ManualResetEventSlim DataWriteEntered = new();
        public readonly ManualResetEventSlim AllowDataWrite = new(false);
        public int LateWriteCount;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => Volatile.Read(ref _closed) == 0;
        public override long Length
        {
            get { lock (_sync) return _inner.Length; }
        }

        public override long Position
        {
            get { lock (_sync) return _inner.Position; }
            set { lock (_sync) _inner.Position = value; }
        }

        public override void Flush()
        {
            lock (_sync) _inner.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Volatile.Read(ref _closed) != 0)
            {
                Interlocked.Increment(ref LateWriteCount);
                throw new ObjectDisposedException(nameof(GateStream));
            }

            if (Length >= FakeWrapper.Header.Length &&
                Interlocked.Exchange(ref _blockedDataWrite, 1) == 0)
            {
                DataWriteEntered.Set();
                AllowDataWrite.Wait();
            }

            lock (_sync) _inner.Write(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_sync) return _inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (_sync) return _inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            lock (_sync) _inner.SetLength(value);
        }

        public byte[] ToArray()
        {
            lock (_sync) return _inner.ToArray();
        }

        public override void Close()
        {
            Interlocked.Exchange(ref _closed, 1);
            lock (_sync) _inner.Close();
            base.Close();
        }
    }
}
