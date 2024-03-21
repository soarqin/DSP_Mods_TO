using CruiseAssist.Commons;
using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(UITechTree))]
internal class Patch_UITechTree
{
    [HarmonyPatch("_OnOpen")]
    [HarmonyPrefix]
    public static void OnOpen_Prefix()
    {
        ConfigManager.CheckConfig(ConfigManager.Step.State);
        CruiseAssistPlugin.Extensions.ForEach(extension => extension.CheckConfig(ConfigManager.Step.State.ToString()));
    }
}