using AutoPilot.Commons;
using AutoPilot.Enums;
using AutoPilot.Patches;
using BepInEx;
using CruiseAssist;
using HarmonyLib;

namespace AutoPilot;

[BepInDependency("tanu.CruiseAssist")]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class AutoPilotPlugin : BaseUnityPlugin
{
    public void Awake()
    {
        LogManager.Logger = Logger;
        new AutoPilotConfigManager(Config);
        ConfigManager.CheckConfig(ConfigManager.Step.Awake);
        _harmony = new Harmony("tanu.AutoPilot.Patch");
        _harmony.PatchAll(typeof(Patch_VFInput));
        CruiseAssistPlugin.RegistExtension(new AutoPilotExtension());
    }

    public void OnDestroy()
    {
        CruiseAssistPlugin.UnregistExtension(typeof(AutoPilotExtension));
        _harmony.UnpatchSelf();
    }

    public static double EnergyPer = 0.0;

    public static double Speed = 0.0;

    public static int WarperCount = 0;

    public static bool LeavePlanet = false;

    public static bool SpeedUp = false;

    public static AutoPilotState State = AutoPilotState.Inactive;

    public static bool InputSailSpeedUp = false;

    private Harmony _harmony;

    public static class Conf
    {
        public static int MinEnergyPer = 20;

        public static int MaxSpeed = 2000;

        public static int WarpMinRangeAU = 2;

        public static int SpeedToWarp = 1200;

        public static bool LocalWarpFlag = false;

        public static bool AutoStartFlag = true;

        public static bool IgnoreGravityFlag = true;

        public static bool MainWindowJoinFlag = true;
    }
}