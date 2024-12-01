using System;

namespace CompressSave.Wrapper;

public class NoneAPI: WrapperDefines
{
    public static readonly bool Avaliable;
    public static readonly NoneAPI Instance = new();

    static NoneAPI()
    {
        try
        {
            Avaliable = Instance.ResolveDllImports("nonewrap.dll");
        }
        catch (Exception e)
        {
            Avaliable = false;
            Console.WriteLine($"Error: {e}");
        }
    }
}
