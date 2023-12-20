using System;
using AutoPilot.Commons;
using CruiseAssist;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotDebugUI
{
    public static void OnGUI()
    {
        var guistyle = new GUIStyle(GUI.skin.window)
        {
            fontSize = 11
        };
        Rect = GUILayout.Window(99031293, Rect, WindowFunction, "AutoPilot - Debug", guistyle, Array.Empty<GUILayoutOption>());

        if (Screen.width < Rect.xMax)
        {
            Rect.x = Screen.width - Rect.width;
        }

        if (Rect.x < 0f)
        {
            Rect.x = 0f;
        }

        if (Screen.height < Rect.yMax)
        {
            Rect.y = Screen.height - Rect.height;
        }

        if (Rect.y < 0f)
        {
            Rect.y = 0f;
        }

        if (_lastCheckWindowLeft != float.MinValue)
        {
            if (Rect.x != _lastCheckWindowLeft || Rect.y != _lastCheckWindowTop)
            {
                AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        _lastCheckWindowLeft = Rect.x;
        _lastCheckWindowTop = Rect.y;
        if (AutoPilotMainUI.NextCheckGameTick <= GameMain.gameTick)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.uPosition={GameMain.mainPlayer.uPosition}", guistyle, Array.Empty<GUILayoutOption>());
        if (GameMain.localPlanet != null && CruiseAssistPlugin.TargetUPos != VectorLF3.zero)
        {
            var mainPlayer = GameMain.mainPlayer;
            var targetUPos = CruiseAssistPlugin.TargetUPos;
            var magnitude = (targetUPos - mainPlayer.uPosition).magnitude;
            var magnitude2 = (targetUPos - GameMain.localPlanet.uPosition).magnitude;
            var vectorLF = mainPlayer.uPosition - GameMain.localPlanet.uPosition;
            var vectorLF2 = CruiseAssistPlugin.TargetUPos - GameMain.localPlanet.uPosition;
            GUILayout.Label("range1=" + RangeToString(magnitude), guistyle, Array.Empty<GUILayoutOption>());
            GUILayout.Label("range2=" + RangeToString(magnitude2), guistyle, Array.Empty<GUILayoutOption>());
            GUILayout.Label($"range1>range2={magnitude > magnitude2}", guistyle, Array.Empty<GUILayoutOption>());
            GUILayout.Label($"angle={Vector3.Angle(vectorLF, vectorLF2)}", guistyle, Array.Empty<GUILayoutOption>());
        }
        var mecha = GameMain.mainPlayer.mecha;
        GUILayout.Label($"mecha.coreEnergy={mecha.coreEnergy}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"mecha.coreEnergyCap={mecha.coreEnergyCap}", guistyle, Array.Empty<GUILayoutOption>());
        var energyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        GUILayout.Label($"energyPer={energyPer}", guistyle, Array.Empty<GUILayoutOption>());
        var speed = GameMain.mainPlayer.controller.actionSail.visual_uvel.magnitude;
        GUILayout.Label("spped=" + RangeToString(speed), guistyle, Array.Empty<GUILayoutOption>());
        var movementStateInFrame = GameMain.mainPlayer.controller.movementStateInFrame;
        GUILayout.Label($"movementStateInFrame={movementStateInFrame}", guistyle, Array.Empty<GUILayoutOption>());
        var guistyle2 = new GUIStyle(GUI.skin.toggle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
        GUI.changed = false;
        AutoPilotPlugin.Conf.IgnoreGravityFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.IgnoreGravityFlag, "Ignore gravity.", guistyle2, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    private static string RangeToString(double range)
    {
        if (range < 10000.0)
        {
            return (int)(range + 0.5) + "m ";
        }

        if (range < 600000.0)
        {
            return (range / 40000.0).ToString("0.00") + "AU";
        }

        return (range / 2400000.0).ToString("0.00") + "Ly";
    }

    public static bool Show = false;

    public static Rect Rect = new(0f, 0f, 400f, 400f);

    private static float _lastCheckWindowLeft = float.MinValue;

    private static float _lastCheckWindowTop = float.MinValue;

    private static Vector2 _scrollPos = Vector2.zero;
}