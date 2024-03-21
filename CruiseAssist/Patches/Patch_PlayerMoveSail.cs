using HarmonyLib;
using UnityEngine;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(PlayerMove_Sail))]
internal class Patch_PlayerMoveSail
{
    [HarmonyPatch("GameTick")]
    [HarmonyPrefix]
    public static void GameTick_Prefix(PlayerMove_Sail __instance)
    {
        if (!CruiseAssistPlugin.Enable) return;
        if (!CruiseAssistPlugin.TargetSelected) return;
        var player = __instance.player;
        if (!player.sailing) return;
        var controller = player.controller;
        if (controller.input0 != Vector4.zero || controller.input1 != Vector4.zero)
        {
            CruiseAssistPlugin.Interrupt = true;
            CruiseAssistPlugin.Extensions.ForEach(extension => extension.CancelOperate());
        }
        else
        {
            if (CruiseAssistPlugin.TargetPlanet != null)
            {
                CruiseAssistPlugin.TargetUPos = CruiseAssistPlugin.TargetPlanet.uPosition;
            }
            else
            {
                if (CruiseAssistPlugin.TargetStar == null)
                {
                    return;
                }
                CruiseAssistPlugin.TargetUPos = CruiseAssistPlugin.TargetStar.uPosition;
            }
            var operate = false;
            CruiseAssistPlugin.Extensions.ForEach(extension => operate |= extension.OperateSail(__instance));
            if (operate) return;
            var vec = CruiseAssistPlugin.TargetUPos - player.uPosition;
            var magnitude = controller.actionSail.visual_uvel.magnitude;
            var angle = Vector3.Angle(vec, player.uVelocity);
            if (angle > 1.6f)
            {
                player.uVelocity = Vector3.Slerp(player.uVelocity, vec.normalized * magnitude, 1.6f / Mathf.Max(10f, angle));
            }
            else
            {
                player.uVelocity = vec.normalized * magnitude;
            }
        }
    }
}