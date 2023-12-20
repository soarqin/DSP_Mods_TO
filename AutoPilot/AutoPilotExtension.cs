using System;
using System.Linq;
using AutoPilot.Commons;
using AutoPilot.Enums;
using AutoPilot.UI;
using CruiseAssist;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot;

internal class AutoPilotExtension : ICruiseAssistExtensionAPI
{
    public void CheckConfig(string step)
    {
        EnumUtils.TryParse(step, out ConfigManager.Step step2);
        ConfigManager.CheckConfig(step2);
    }

    public void SetTargetAstroId(int astroId)
    {
        AutoPilotPlugin.State = (AutoPilotPlugin.Conf.AutoStartFlag ? AutoPilotState.Active : AutoPilotState.Inactive);
        AutoPilotPlugin.InputSailSpeedUp = false;
    }

    public bool OperateWalk(PlayerMove_Walk __instance)
    {
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            return false;
        }

        var player = __instance.player;
        var mecha = player.mecha;
        AutoPilotPlugin.EnergyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        AutoPilotPlugin.Speed = player.controller.actionSail.visual_uvel.magnitude;
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        var ignoreGravityFlag = AutoPilotPlugin.Conf.IgnoreGravityFlag;
        if (ignoreGravityFlag)
        {
            player.controller.universalGravity = VectorLF3.zero;
            player.controller.localGravity = VectorLF3.zero;
        }
        __instance.controller.input0.z = 1f;
        return true;
    }

    public bool OperateDrift(PlayerMove_Drift __instance)
    {
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            return false;
        }

        var player = __instance.player;
        var mecha = player.mecha;
        AutoPilotPlugin.EnergyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        AutoPilotPlugin.Speed = player.controller.actionSail.visual_uvel.magnitude;
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        var ignoreGravityFlag = AutoPilotPlugin.Conf.IgnoreGravityFlag;
        if (ignoreGravityFlag)
        {
            player.controller.universalGravity = VectorLF3.zero;
            player.controller.localGravity = VectorLF3.zero;
        }
        __instance.controller.input0.z = 1f;
        return true;
    }

    public bool OperateFly(PlayerMove_Fly __instance)
    {
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            return false;
        }

        var player = __instance.player;
        var mecha = player.mecha;
        AutoPilotPlugin.EnergyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        AutoPilotPlugin.Speed = player.controller.actionSail.visual_uvel.magnitude;
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        var ignoreGravityFlag = AutoPilotPlugin.Conf.IgnoreGravityFlag;
        if (ignoreGravityFlag)
        {
            player.controller.universalGravity = VectorLF3.zero;
            player.controller.localGravity = VectorLF3.zero;
        }
        var controller = __instance.controller;
        controller.input0.y = controller.input0.y + 1f;
        var controller2 = __instance.controller;
        controller2.input1.y = controller2.input1.y + 1f;
        return true;
    }

    public bool OperateSail(PlayerMove_Sail __instance)
    {
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            return false;
        }

        var player = __instance.player;
        var mecha = player.mecha;
        AutoPilotPlugin.EnergyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        AutoPilotPlugin.Speed = player.controller.actionSail.visual_uvel.magnitude;
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = false;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        if (player.warping)
        {
            return false;
        }

        if (AutoPilotPlugin.EnergyPer < AutoPilotPlugin.Conf.MinEnergyPer)
        {
            return false;
        }

        if (AutoPilotPlugin.Conf.IgnoreGravityFlag)
        {
            player.controller.universalGravity = VectorLF3.zero;
            player.controller.localGravity = VectorLF3.zero;
        }

        if (AutoPilotPlugin.Speed < AutoPilotPlugin.Conf.MaxSpeed)
        {
            AutoPilotPlugin.InputSailSpeedUp = true;
            AutoPilotPlugin.SpeedUp = true;
        }

        if (GameMain.localPlanet == null)
        {
            if (!AutoPilotPlugin.Conf.LocalWarpFlag && GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id == GameMain.localStar.id) return false;
            if (!(AutoPilotPlugin.Conf.WarpMinRangeAU * 40000.0 <= CruiseAssistPlugin.TargetRange) || !(AutoPilotPlugin.Conf.SpeedToWarp <= AutoPilotPlugin.Speed) ||
                1 > AutoPilotPlugin.WarperCount) return false;
            if (!(mecha.coreEnergy > mecha.warpStartPowerPerSpeed * mecha.maxWarpSpeed)) return false;
            if (!mecha.UseWarper()) return false;
            player.warpCommand = true;
            VFAudio.Create("warp-begin", player.transform, Vector3.zero, true);
            return false;
        }

        var vec = player.uPosition - GameMain.localPlanet.uPosition;
        if (120.0 < AutoPilotPlugin.Speed && Math.Max(GameMain.localPlanet.realRadius, 800f) < vec.magnitude - GameMain.localPlanet.realRadius)
        {
            return false;
        }
        VectorLF3 result;
        if (Vector3.Angle(player.uPosition - GameMain.localPlanet.uPosition, CruiseAssistPlugin.TargetUPos - GameMain.localPlanet.uPosition) > 90f)
        {
            AutoPilotPlugin.LeavePlanet = true;
            result = vec;
        }
        else
        {
            result = CruiseAssistPlugin.TargetUPos - player.uPosition;
        }
        var num = Vector3.Angle(result, player.uVelocity);
        var num2 = 1.6f / Mathf.Max(10f, num);
        var num3 = Math.Min(AutoPilotPlugin.Speed, 120.0);
        player.uVelocity = Vector3.Slerp(player.uVelocity, result.normalized * num3, num2);
        return true;
    }

    public void SetInactive()
    {
        AutoPilotPlugin.State = AutoPilotState.Inactive;
        AutoPilotPlugin.InputSailSpeedUp = false;
    }

    public void CancelOperate()
    {
        AutoPilotPlugin.State = AutoPilotState.Inactive;
        AutoPilotPlugin.InputSailSpeedUp = false;
    }

    public void OnGUI()
    {
        var uiGame = UIRoot.instance.uiGame;
        var num = CruiseAssistMainUI.Scale / 100f;
        AutoPilotMainUI.OnGUI();
        var flag = AutoPilotConfigUI.Show[CruiseAssistMainUI.WIdx];
        if (flag)
        {
            AutoPilotConfigUI.OnGUI();
        }
        var show = AutoPilotDebugUI.Show;
        if (show)
        {
            AutoPilotDebugUI.OnGUI();
        }
        var flag2 = ResetInput(AutoPilotMainUI.Rect[CruiseAssistMainUI.WIdx], num);
        var flag3 = !flag2 && AutoPilotConfigUI.Show[CruiseAssistMainUI.WIdx];
        if (flag3)
        {
            flag2 = ResetInput(AutoPilotConfigUI.Rect[CruiseAssistMainUI.WIdx], num);
        }
        var flag4 = !flag2 && AutoPilotDebugUI.Show;
        if (flag4)
        {
            flag2 = ResetInput(AutoPilotDebugUI.Rect, num);
        }
    }

    private bool ResetInput(Rect rect, float scale)
    {
        var num = rect.xMin * scale;
        var num2 = rect.xMax * scale;
        var num3 = rect.yMin * scale;
        var num4 = rect.yMax * scale;
        var x = Input.mousePosition.x;
        var num5 = Screen.height - Input.mousePosition.y;
        var flag = num <= x && x <= num2 && num3 <= num5 && num5 <= num4;
        if (flag)
        {
            int[] array = { 0, 1, 2 };
            var flag2 = array.Any(Input.GetMouseButton) || Input.mouseScrollDelta.y != 0f;
            if (flag2)
            {
                Input.ResetInputAxes();
                return true;
            }
        }
        return false;
    }
}