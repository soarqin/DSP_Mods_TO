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
        AutoPilotPlugin.State = AutoPilotPlugin.Conf.AutoStartFlag ? AutoPilotState.Active : AutoPilotState.Inactive;
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
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        if (AutoPilotPlugin.Conf.IgnoreGravityFlag)
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
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        if (AutoPilotPlugin.Conf.IgnoreGravityFlag)
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
        AutoPilotPlugin.WarperCount = mecha.warpStorage.GetItemCount(1210);
        AutoPilotPlugin.LeavePlanet = true;
        AutoPilotPlugin.SpeedUp = false;
        AutoPilotPlugin.InputSailSpeedUp = false;
        if (AutoPilotPlugin.Conf.IgnoreGravityFlag)
        {
            player.controller.universalGravity = VectorLF3.zero;
            player.controller.localGravity = VectorLF3.zero;
        }

        var controller = __instance.controller;
        controller.input0.y += 1f;
        controller.input1.y += 1f;
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

        var speed = player.controller.actionSail.visual_uvel.magnitude;
        if (speed < AutoPilotPlugin.Conf.MaxSpeed)
        {
            AutoPilotPlugin.InputSailSpeedUp = true;
            AutoPilotPlugin.SpeedUp = true;
        }

        if (GameMain.localPlanet == null)
        {
            if (!AutoPilotPlugin.Conf.LocalWarpFlag && GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id == GameMain.localStar.id) return false;
            if (!(AutoPilotPlugin.Conf.WarpMinRangeAu * 40000.0 <= CruiseAssistPlugin.TargetRange) || !(AutoPilotPlugin.Conf.SpeedToWarp <= speed) ||
                1 > AutoPilotPlugin.WarperCount) return false;
            if (!(mecha.coreEnergy > mecha.warpStartPowerPerSpeed * mecha.maxWarpSpeed)) return false;
            if (!mecha.UseWarper()) return false;
            player.warpCommand = true;
            VFAudio.Create("warp-begin", player.transform, Vector3.zero, true);
            return false;
        }

        speed = player.uVelocity.magnitude;
        var vec = player.uPosition - GameMain.localPlanet.uPosition;
        if (340.0 < speed && Math.Max(GameMain.localPlanet.realRadius, 900f) < vec.magnitude - GameMain.localPlanet.realRadius)
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

        var angle = Vector3.Angle(result, player.uVelocity);
        if (angle > 1.6f)
        {
            player.uVelocity = Vector3.Slerp(player.uVelocity, result.normalized * Math.Min(speed, 320.0), 1.6f / Mathf.Max(10f, angle));
        }
        else
        {
            player.uVelocity = result.normalized * Math.Min(speed, 320.0);
        }

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
        if (!_initialized)
        {
            _initialized = true;
            AutoPilotMainUI.OnInit();
            AutoPilotConfigUI.OnInit();
            AutoPilotDebugUI.OnInit();
        }

        var scale = CruiseAssistMainUI.Scale / 100f;

        AutoPilotMainUI.OnGUI();
        if (AutoPilotConfigUI.Show[CruiseAssistMainUI.WIdx])
        {
            AutoPilotConfigUI.OnGUI();
        }

        if (AutoPilotDebugUI.Show)
        {
            AutoPilotDebugUI.OnGUI();
        }

        var resetInput = ResetInput(AutoPilotMainUI.Rect[CruiseAssistMainUI.WIdx], scale);
        if (!resetInput && AutoPilotConfigUI.Show[CruiseAssistMainUI.WIdx])
        {
            resetInput = ResetInput(AutoPilotConfigUI.Rect[CruiseAssistMainUI.WIdx], scale);
        }

        if (!resetInput && AutoPilotDebugUI.Show)
        {
            ResetInput(AutoPilotDebugUI.Rect, scale);
        }
    }

    private static bool ResetInput(Rect rect, float scale)
    {
        var xMin = rect.xMin * scale;
        var xMax = rect.xMax * scale;
        var yMin = rect.yMin * scale;
        var yMax = rect.yMax * scale;
        var x = Input.mousePosition.x;
        var num5 = Screen.height - Input.mousePosition.y;
        if (!(xMin <= x) || !(x <= xMax) || !(yMin <= num5) || !(num5 <= yMax)) return false;
        if (!MouseButtonArray.Any(Input.GetMouseButton) && Input.mouseScrollDelta.y == 0f) return false;
        Input.ResetInputAxes();
        return true;
    }

    private bool _initialized;
    private static readonly int[] MouseButtonArray = [0, 1, 2];
}