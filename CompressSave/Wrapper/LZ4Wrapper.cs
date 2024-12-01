using System;

namespace CompressSave.Wrapper;

public class LZ4API: WrapperDefines
{
    public static readonly bool Avaliable;
    public static readonly LZ4API Instance = new();

    static LZ4API()
    {
        try
        {
            Avaliable = Instance.ResolveDllImports("lz4wrap.dll");
        }
        catch (Exception e)
        {
            Avaliable = false;
            Console.WriteLine($"Error: {e}");
        }
    }
}
