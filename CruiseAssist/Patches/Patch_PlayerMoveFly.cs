using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(PlayerMove_Fly))]
internal class Patch_PlayerMoveFly
{
    [HarmonyPatch("GameTick")]
    [HarmonyPrefix]
    public static void GameTick_Prefix(PlayerMove_Fly __instance)
    {
        if (!CruiseAssistPlugin.Enable) return;
        if (!CruiseAssistPlugin.TargetSelected) return;
        if (__instance.controller.movementStateInFrame != EMovementState.Fly) return;
        if (VFInput._moveForward.pressing || VFInput._pullUp.pressing)
        {
            CruiseAssistPlugin.Interrupt = true;
            CruiseAssistPlugin.Extensions.ForEach(extension => extension.CancelOperate());
        }
        else
        {
            CruiseAssistPlugin.Extensions.ForEach(extension => extension.OperateFly(__instance));
        }
    }
}