using CruiseAssist.Commons;
using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(UIStarmap))]
internal class Patch_UIStarmap
{
    [HarmonyPatch("_OnClose")]
    [HarmonyPrefix]
    public static void OnClose_Prefix()
    {
        ConfigManager.CheckConfig(ConfigManager.Step.State);
        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
        {
            extension.CheckConfig(ConfigManager.Step.State.ToString());
        });
    }
}