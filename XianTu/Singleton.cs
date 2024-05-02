

namespace XianTu;

public class Singleton<T> where T : new()
{
    public static T Instance
    {
        get
        {
            var flag = _msInstance == null;
            if (flag)
            {
                _msInstance = new T();
            }
            return _msInstance;
        }
    }

    private static T _msInstance;
}