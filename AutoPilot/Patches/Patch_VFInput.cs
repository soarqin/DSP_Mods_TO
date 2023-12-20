using AutoPilot.Enums;
using HarmonyLib;

namespace AutoPilot.Patches;

[HarmonyPatch(typeof(VFInput))]
internal class Patch_VFInput
{
    [HarmonyPatch("_sailSpeedUp", MethodType.Getter)]
    [HarmonyPostfix]
    public static void SailSpeedUp_Postfix(ref bool __result)
    {
        if (AutoPilotPlugin.State == AutoPilotState.Inactive) return;
        if (AutoPilotPlugin.InputSailSpeedUp)
            __result = true;
    }
}