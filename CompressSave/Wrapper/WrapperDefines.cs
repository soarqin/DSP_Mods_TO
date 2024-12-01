using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CompressSave.Wrapper;

public struct DecompressStatus
{
    public long WriteLen;
    public long ReadLen;
    public long Expect;
}

public class WrapperDefines
{
    public delegate long CompressBufferBoundFunc(long inBufferSize);
    public delegate long CompressBeginFunc(out IntPtr ctx, int compressionLevel, byte[] outBuff, long outCapacity, byte[] dictBuffer = null,
        long dictSize = 0);
    public delegate long CompressEndFunc(IntPtr ctx, byte[] dstBuffer, long dstCapacity);
    public delegate void CompressContextFreeFunc(IntPtr ctx);
    public delegate long DecompressBeginFunc(ref IntPtr pdctx, byte[] inBuffer, ref int inBufferSize, out int blockSize, byte[] dict = null, long dictSize = 0);
    public delegate long DecompressEndFunc(IntPtr dctx);
    public delegate void DecompressContextResetFunc(IntPtr dctx);
    protected unsafe delegate long CompressUpdateFunc(IntPtr ctx, byte* dstBuffer, long dstCapacity, byte* srcBuffer,
        long srcSize);
    protected unsafe delegate long DecompressUpdateFunc(IntPtr dctx, byte* dstBuffer, ref long dstCapacity, byte* srcBuffer,
        ref long srcSize);
    
    public CompressBufferBoundFunc CompressBufferBound;
    public CompressBeginFunc CompressBegin;
    public CompressEndFunc CompressEnd;
    public CompressContextFreeFunc CompressContextFree;
    public DecompressBeginFunc DecompressBegin;
    public DecompressEndFunc DecompressEnd;
    public DecompressContextResetFunc DecompressContextReset;
    protected CompressUpdateFunc CompressUpdate;
    protected DecompressUpdateFunc DecompressUpdate;

    public bool ResolveDllImports(string dllName)
    {
        var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(ZstdAPI)).Location;
        string[] dlls;
        if (string.IsNullOrEmpty(assemblyPath))
        {
            dlls = [
                dllName, "x64/" + dllName, "plugins/x64/" + dllName, "BepInEx/scripts/x64/" + dllName
            ];
        }
        else
        {
            var root = Path.GetDirectoryName(assemblyPath) ?? string.Empty;
            dlls = [
                dllName, "x64/" + dllName, "plugins/x64/" + dllName, "BepInEx/scripts/x64/" + dllName,
                Path.Combine(root, dllName), Path.Combine(root, "x64/" + dllName), Path.Combine(root, "plugins/x64/" + dllName)
            ];
        }
        foreach (var dll in dlls)
        {
            var lib = WinApi.LoadLibrary(dll);
            if (lib == IntPtr.Zero) continue;
            CompressBufferBound = Marshal.GetDelegateForFunctionPointer<CompressBufferBoundFunc>(WinApi.GetProcAddress(lib, "CompressBufferBound"));
            CompressBegin = Marshal.GetDelegateForFunctionPointer<CompressBeginFunc>(WinApi.GetProcAddress(lib, "CompressBegin"));
            CompressEnd = Marshal.GetDelegateForFunctionPointer<CompressEndFunc>(WinApi.GetProcAddress(lib, "CompressEnd"));
            CompressUpdate = Marshal.GetDelegateForFunctionPointer<CompressUpdateFunc>(WinApi.GetProcAddress(lib, "CompressUpdate"));
            CompressContextFree = Marshal.GetDelegateForFunctionPointer<CompressContextFreeFunc>(WinApi.GetProcAddress(lib, "CompressContextFree"));
            DecompressBegin = Marshal.GetDelegateForFunctionPointer<DecompressBeginFunc>(WinApi.GetProcAddress(lib, "DecompressBegin"));
            DecompressEnd = Marshal.GetDelegateForFunctionPointer<DecompressEndFunc>(WinApi.GetProcAddress(lib, "DecompressEnd"));
            DecompressUpdate = Marshal.GetDelegateForFunctionPointer<DecompressUpdateFunc>(WinApi.GetProcAddress(lib, "DecompressUpdate"));
            DecompressContextReset = Marshal.GetDelegateForFunctionPointer<DecompressContextResetFunc>(WinApi.GetProcAddress(lib, "DecompressContextReset"));
            if (CompressBufferBound != null && CompressBegin != null && CompressEnd != null && CompressUpdate != null && CompressContextFree != null &&
                DecompressBegin != null && DecompressEnd != null && DecompressUpdate != null && DecompressContextReset != null) return true;
            WinApi.FreeLibrary(lib);
        }

        return false;
    }

    public unsafe long CompressUpdateEx(IntPtr ctx, byte[] dstBuffer, long dstOffset, byte[] srcBuffer,
        long srcOffset, long srcLen)
    {
        fixed (byte* pdst = dstBuffer, psrc = srcBuffer)
        {
            return CompressUpdate(ctx, pdst + dstOffset, dstBuffer.Length - dstOffset, psrc + srcOffset,
                srcLen - srcOffset);
        }
    }

    public unsafe DecompressStatus DecompressUpdateEx(IntPtr dctx, byte[] dstBuffer, int dstOffset, int dstCount,
        byte[] srcBuffer, long srcOffset, long count)
    {
        long dstLen = Math.Min(dstCount, dstBuffer.Length - dstOffset);
        long errCode;
        fixed (byte* pdst = dstBuffer, psrc = srcBuffer)
        {
            errCode = DecompressUpdate(dctx, pdst + dstOffset, ref dstLen, psrc + srcOffset, ref count);
        }

        return new DecompressStatus
        {
            Expect = errCode,
            ReadLen = count,
            WriteLen = dstLen,
        };
    }
}
