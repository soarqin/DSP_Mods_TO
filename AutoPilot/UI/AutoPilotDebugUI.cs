using AutoPilot.Commons;
using CruiseAssist;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotDebugUI
{
    public static void OnInit()
    {
        _windowStyle = new GUIStyle(GUI.skin.window)
        {
            fontSize = 11
        };
        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        _toggleStyle = new GUIStyle(GUI.skin.toggle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
    }

    public static void OnGUI()
    {
        Rect = GUILayout.Window(99031293, Rect, WindowFunction, "AutoPilot - Debug", _windowStyle);

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
        GUILayout.BeginVertical();
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        GUILayout.Label($"GameMain.mainPlayer.uPosition={GameMain.mainPlayer.uPosition}", _labelStyle);
        if (GameMain.localPlanet != null && CruiseAssistPlugin.TargetUPos != VectorLF3.zero)
        {
            var mainPlayer = GameMain.mainPlayer;
            var targetUPos = CruiseAssistPlugin.TargetUPos;
            var magnitude = (targetUPos - mainPlayer.uPosition).magnitude;
            var magnitude2 = (targetUPos - GameMain.localPlanet.uPosition).magnitude;
            var vectorLF = mainPlayer.uPosition - GameMain.localPlanet.uPosition;
            var vectorLF2 = CruiseAssistPlugin.TargetUPos - GameMain.localPlanet.uPosition;
            GUILayout.Label("range1=" + RangeToString(magnitude), _labelStyle);
            GUILayout.Label("range2=" + RangeToString(magnitude2), _labelStyle);
            GUILayout.Label($"range1>range2={magnitude > magnitude2}", _labelStyle);
            GUILayout.Label($"angle={Vector3.Angle(vectorLF, vectorLF2)}", _labelStyle);
        }
        var mecha = GameMain.mainPlayer.mecha;
        GUILayout.Label($"mecha.coreEnergy={mecha.coreEnergy}", _labelStyle);
        GUILayout.Label($"mecha.coreEnergyCap={mecha.coreEnergyCap}", _labelStyle);
        var energyPer = mecha.coreEnergy / mecha.coreEnergyCap * 100.0;
        GUILayout.Label($"energyPer={energyPer}", _labelStyle);
        var speed = GameMain.mainPlayer.controller.actionSail.visual_uvel.magnitude;
        GUILayout.Label("spped=" + RangeToString(speed), _labelStyle);
        var movementStateInFrame = GameMain.mainPlayer.controller.movementStateInFrame;
        GUILayout.Label($"movementStateInFrame={movementStateInFrame}", _labelStyle);
        GUI.changed = false;
        AutoPilotPlugin.Conf.IgnoreGravityFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.IgnoreGravityFlag, "Ignore gravity.", _toggleStyle);
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

    private static GUIStyle _windowStyle;
    private static GUIStyle _labelStyle;
    private static GUIStyle _toggleStyle;

    public static bool Show = false;

    public static Rect Rect = new(0f, 0f, 400f, 400f);

    private static float _lastCheckWindowLeft = float.MinValue;

    private static float _lastCheckWindowTop = float.MinValue;

    private static Vector2 _scrollPos = Vector2.zero;
}