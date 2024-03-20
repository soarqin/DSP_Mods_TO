using CruiseAssist.Commons;
using CruiseAssist.UI;
using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(GameMain))]
internal class Patch_GameMain
{
    [HarmonyPatch("Begin")]
    [HarmonyPostfix]
    public static void Begin_Postfix()
    {
        CruiseAssistStarListUI.OnReset();
        ConfigManager.CheckConfig(ConfigManager.Step.GameMainBegin);
        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
        {
            extension.CheckConfig(ConfigManager.Step.GameMainBegin.ToString());
        });
    }

    [HarmonyPatch("Pause")]
    [HarmonyPrefix]
    public static void Pause_Prefix()
    {
        ConfigManager.CheckConfig(ConfigManager.Step.State);
        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
        {
            extension.CheckConfig(ConfigManager.Step.State.ToString());
        });
    }
}