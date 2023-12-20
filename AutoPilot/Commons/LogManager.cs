using System.Reflection;
using BepInEx.Logging;

namespace AutoPilot.Commons;

internal static class LogManager
{
    public static ManualLogSource Logger { private get; set; }

    public static void LogInfo(object data)
    {
        Logger.LogInfo(data);
    }

    public static void LogInfo(MethodBase method)
    {
        Logger.LogInfo(method.DeclaringType.Name + "." + method.Name);
    }

    public static void LogInfo(MethodBase method, object data)
    {
        Logger.LogInfo(string.Concat(method.DeclaringType.Name, ".", method.Name, ": ", data?.ToString()));
    }

    public static void LogError(object data)
    {
        Logger.LogError(data);
    }

    public static void LogError(MethodBase method, object data)
    {
        Logger.LogError(string.Concat(method.DeclaringType.Name, ".", method.Name, ": ", data?.ToString()));
    }
}