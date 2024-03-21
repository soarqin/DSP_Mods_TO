using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(PlayerMove_Drift))]
internal class Patch_PlayerMoveDrift
{
    [HarmonyPatch("GameTick")]
    [HarmonyPrefix]
    public static void GameTick_Prefix(PlayerMove_Drift __instance)
    {
        if (!CruiseAssistPlugin.Enable) return;
        if (!CruiseAssistPlugin.TargetSelected) return;
        if (__instance.controller.movementStateInFrame != EMovementState.Drift) return;
        if (VFInput._moveForward.pressing || VFInput._pullUp.pressing)
        {
            CruiseAssistPlugin.Interrupt = true;
            CruiseAssistPlugin.Extensions.ForEach(extension => extension.CancelOperate());
        }
        else
        {
            CruiseAssistPlugin.Extensions.ForEach(extension => extension.OperateDrift(__instance));
        }
    }
}