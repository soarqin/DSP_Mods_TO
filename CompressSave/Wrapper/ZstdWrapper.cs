using System;

namespace CompressSave.Wrapper;

public class ZstdAPI: WrapperDefines
{
    public static readonly bool Avaliable;
    public static readonly ZstdAPI Instance = new();

    static ZstdAPI()
    {
        try
        {
            Avaliable = Instance.ResolveDllImports("zstdwrap.dll");
        }
        catch (Exception e)
        {
            Avaliable = false;
            Console.WriteLine($"Error: {e}");
        }
    }
}
