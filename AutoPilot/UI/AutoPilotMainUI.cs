using System;
using AutoPilot.Commons;
using AutoPilot.Enums;
using CruiseAssist;
using CruiseAssist.Enums;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotMainUI
{
    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        var viewMode = CruiseAssistMainUI.ViewMode;
        if (viewMode != CruiseAssistMainUIViewMode.Full)
        {
            if (viewMode == CruiseAssistMainUIViewMode.Mini)
            {
                Rect[_wIdx].width = CruiseAssistMainUI.Rect[_wIdx].width;
                Rect[_wIdx].height = 70f;
            }
        }
        else
        {
            Rect[_wIdx].width = CruiseAssistMainUI.Rect[_wIdx].width;
            Rect[_wIdx].height = 150f;
        }
        var guistyle = new GUIStyle(CruiseAssistMainUI.WindowStyle)
        {
            fontSize = 11
        };
        Rect[_wIdx] = GUILayout.Window(99031291, Rect[_wIdx], WindowFunction, "AutoPilot", guistyle, Array.Empty<GUILayoutOption>());
        var scale = CruiseAssistMainUI.Scale / 100f;
        if (AutoPilotPlugin.Conf.MainWindowJoinFlag)
        {
            Rect[_wIdx].x = CruiseAssistMainUI.Rect[CruiseAssistMainUI.WIdx].x;
            Rect[_wIdx].y = CruiseAssistMainUI.Rect[CruiseAssistMainUI.WIdx].yMax;
        }

        if (Screen.width / scale < Rect[_wIdx].xMax)
        {
            Rect[_wIdx].x = Screen.width / scale - Rect[_wIdx].width;
        }

        if (Rect[_wIdx].x < 0f)
        {
            Rect[_wIdx].x = 0f;
        }

        if (Screen.height / scale < Rect[_wIdx].yMax)
        {
            Rect[_wIdx].y = Screen.height / scale - Rect[_wIdx].height;
        }

        if (Rect[_wIdx].y < 0f)
        {
            Rect[_wIdx].y = 0f;
        }

        if (LastCheckWindowLeft[_wIdx] != float.MinValue)
        {
            var flag6 = Rect[_wIdx].x != LastCheckWindowLeft[_wIdx] || Rect[_wIdx].y != LastCheckWindowTop[_wIdx];
            if (flag6)
            {
                NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
        LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
        if (NextCheckGameTick <= GameMain.gameTick)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        if (CruiseAssistMainUI.ViewMode == CruiseAssistMainUIViewMode.Full)
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            var guistyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12
            };
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            var text = ((AutoPilotPlugin.State == AutoPilotState.Inactive) ? "---" : ((AutoPilotPlugin.Conf.MinEnergyPer < AutoPilotPlugin.EnergyPer) ? "OK" : "NG"));
            guistyle.normal.textColor = ((text == "OK") ? Color.cyan : ((text == "NG") ? Color.red : Color.white));
            GUILayout.Label("Energy : " + text, guistyle, Array.Empty<GUILayoutOption>());
            var text2 = ((AutoPilotPlugin.State == AutoPilotState.Inactive) ? "---" : ((CruiseAssistPlugin.TargetStar == null) ? "---" : (GameMain.mainPlayer.warping ? "---" : ((!AutoPilotPlugin.Conf.LocalWarpFlag && GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id == GameMain.localStar.id) ? "---" : ((CruiseAssistPlugin.TargetRange < (double)(AutoPilotPlugin.Conf.WarpMinRangeAU * 40000)) ? "---" : ((AutoPilotPlugin.WarperCount < 1) ? "NG" : "OK"))))));
            guistyle.normal.textColor = ((text2 == "OK") ? Color.cyan : ((text2 == "NG") ? Color.red : Color.white));
            GUILayout.Label("Warper : " + text2, guistyle, Array.Empty<GUILayoutOption>());
            GUILayout.EndVertical();
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            var text3 = ((AutoPilotPlugin.State == AutoPilotState.Inactive) ? "---" : (AutoPilotPlugin.LeavePlanet ? "ON" : "OFF"));
            guistyle.normal.textColor = ((text3 == "ON") ? Color.cyan : Color.white);
            GUILayout.Label("Leave Planet : " + text3, guistyle, Array.Empty<GUILayoutOption>());
            var text4 = ((AutoPilotPlugin.State == AutoPilotState.Inactive) ? "---" : (AutoPilotPlugin.SpeedUp ? "ON" : "OFF"));
            guistyle.normal.textColor = ((text4 == "ON") ? Color.cyan : Color.white);
            GUILayout.Label("Spee UP : " + text4, guistyle, Array.Empty<GUILayoutOption>());
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        var guistyle2 = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 160f,
            fixedHeight = 32f,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            GUILayout.Label("Auto Pilot Inactive.", guistyle2, Array.Empty<GUILayoutOption>());
        }
        else
        {
            guistyle2.normal.textColor = Color.cyan;
            GUILayout.Label("Auto Pilot Active.", guistyle2, Array.Empty<GUILayoutOption>());
        }
        GUILayout.FlexibleSpace();
        var guistyle3 = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 50f,
            fixedHeight = 18f,
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("Config", guistyle3, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            var show = AutoPilotConfigUI.Show;
            var num = _wIdx;
            show[num] = !show[num];
            var flag4 = AutoPilotConfigUI.Show[_wIdx];
            if (flag4)
            {
                AutoPilotConfigUI.TempMinEnergyPer = AutoPilotPlugin.Conf.MinEnergyPer.ToString();
                AutoPilotConfigUI.TempMaxSpeed = AutoPilotPlugin.Conf.MaxSpeed.ToString();
                AutoPilotConfigUI.TempWarpMinRangeAU = AutoPilotPlugin.Conf.WarpMinRangeAU.ToString();
                AutoPilotConfigUI.TempSpeedToWarp = AutoPilotPlugin.Conf.SpeedToWarp.ToString();
            }
        }

        if (GUILayout.Button("Start", guistyle3, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotPlugin.State = AutoPilotState.Active;
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.Button("-", guistyle3, Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("Stop", guistyle3, Array.Empty<GUILayoutOption>()))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotPlugin.State = AutoPilotState.Inactive;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    private static int _wIdx;

    public const float WindowWidthFull = 398f;

    public const float WindowHeightFull = 150f;

    public const float WindowWidthMini = 288f;

    public const float WindowHeightMini = 70f;

    public static readonly Rect[] Rect = {
        new Rect(0f, 0f, 398f, 150f),
        new Rect(0f, 0f, 398f, 150f)
    };

    private static readonly float[] LastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static readonly float[] LastCheckWindowTop = { float.MinValue, float.MinValue };

    public static long NextCheckGameTick = long.MaxValue;
}