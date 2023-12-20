using System;
using AutoPilot.Commons;
using CruiseAssist;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal class AutoPilotDebugUI
{
    public static void OnGUI()
    {
        var guistyle = new GUIStyle(GUI.skin.window)
        {
            fontSize = 11
        };
        Rect = GUILayout.Window(99031293, Rect, WindowFunction, "AutoPilot - Debug", guistyle, Array.Empty<GUILayoutOption>());
        var num = CruiseAssistMainUI.Scale / 100f;
        var flag = Screen.width < Rect.xMax;
        if (flag)
        {
            Rect.x = Screen.width - Rect.width;
        }
        var flag2 = Rect.x < 0f;
        if (flag2)
        {
            Rect.x = 0f;
        }
        var flag3 = Screen.height < Rect.yMax;
        if (flag3)
        {
            Rect.y = Screen.height - Rect.height;
        }
        var flag4 = Rect.y < 0f;
        if (flag4)
        {
            Rect.y = 0f;
        }
        var flag5 = lastCheckWindowLeft != float.MinValue;
        if (flag5)
        {
            var flag6 = Rect.x != lastCheckWindowLeft || Rect.y != lastCheckWindowTop;
            if (flag6)
            {
                AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        lastCheckWindowLeft = Rect.x;
        lastCheckWindowTop = Rect.y;
        var flag7 = AutoPilotMainUI.NextCheckGameTick <= GameMain.gameTick;
        if (flag7)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    public static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        scrollPos = GUILayout.BeginScrollView(scrollPos, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.uPosition={GameMain.mainPlayer.uPosition}", guistyle, Array.Empty<GUILayoutOption>());
        var flag = GameMain.localPlanet != null && CruiseAssistPlugin.TargetUPos != VectorLF3.zero;
        if (flag)
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
        var num = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        GUILayout.Label($"energyPer={num}", guistyle, Array.Empty<GUILayoutOption>());
        var magnitude3 = GameMain.mainPlayer.controller.actionSail.visual_uvel.magnitude;
        GUILayout.Label("spped=" + RangeToString(magnitude3), guistyle, Array.Empty<GUILayoutOption>());
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
        var changed = GUI.changed;
        if (changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    public static string RangeToString(double range)
    {
        var flag = range < 10000.0;
        string text;
        if (flag)
        {
            text = ((int)(range + 0.5)) + "m ";
        }
        else
        {
            var flag2 = range < 600000.0;
            if (flag2)
            {
                text = (range / 40000.0).ToString("0.00") + "AU";
            }
            else
            {
                text = (range / 2400000.0).ToString("0.00") + "Ly";
            }
        }
        return text;
    }

    public static bool Show = false;

    public static Rect Rect = new Rect(0f, 0f, 400f, 400f);

    private static float lastCheckWindowLeft = float.MinValue;

    private static float lastCheckWindowTop = float.MinValue;

    private static Vector2 scrollPos = Vector2.zero;
}