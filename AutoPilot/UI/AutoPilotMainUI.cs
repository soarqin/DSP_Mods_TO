using System;
using AutoPilot.Commons;
using AutoPilot.Enums;
using CruiseAssist;
using CruiseAssist.Enums;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal class AutoPilotMainUI
{
    public static void OnGUI()
    {
        wIdx = CruiseAssistMainUI.WIdx;
        var viewMode = CruiseAssistMainUI.ViewMode;
        var cruiseAssistMainUIViewMode = viewMode;
        if (cruiseAssistMainUIViewMode != CruiseAssistMainUIViewMode.Full)
        {
            if (cruiseAssistMainUIViewMode == CruiseAssistMainUIViewMode.Mini)
            {
                Rect[wIdx].width = CruiseAssistMainUI.Rect[wIdx].width;
                Rect[wIdx].height = 70f;
            }
        }
        else
        {
            Rect[wIdx].width = CruiseAssistMainUI.Rect[wIdx].width;
            Rect[wIdx].height = 150f;
        }
        var guistyle = new GUIStyle(CruiseAssistMainUI.WindowStyle)
        {
            fontSize = 11
        };
        Rect[wIdx] = GUILayout.Window(99031291, Rect[wIdx], WindowFunction, "AutoPilot", guistyle, Array.Empty<GUILayoutOption>());
        var num = CruiseAssistMainUI.Scale / 100f;
        var mainWindowJoinFlag = AutoPilotPlugin.Conf.MainWindowJoinFlag;
        if (mainWindowJoinFlag)
        {
            Rect[wIdx].x = CruiseAssistMainUI.Rect[CruiseAssistMainUI.WIdx].x;
            Rect[wIdx].y = CruiseAssistMainUI.Rect[CruiseAssistMainUI.WIdx].yMax;
        }
        var flag = Screen.width / num < Rect[wIdx].xMax;
        if (flag)
        {
            Rect[wIdx].x = Screen.width / num - Rect[wIdx].width;
        }
        var flag2 = Rect[wIdx].x < 0f;
        if (flag2)
        {
            Rect[wIdx].x = 0f;
        }
        var flag3 = Screen.height / num < Rect[wIdx].yMax;
        if (flag3)
        {
            Rect[wIdx].y = Screen.height / num - Rect[wIdx].height;
        }
        var flag4 = Rect[wIdx].y < 0f;
        if (flag4)
        {
            Rect[wIdx].y = 0f;
        }
        var flag5 = lastCheckWindowLeft[wIdx] != float.MinValue;
        if (flag5)
        {
            var flag6 = Rect[wIdx].x != lastCheckWindowLeft[wIdx] || Rect[wIdx].y != lastCheckWindowTop[wIdx];
            if (flag6)
            {
                NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        lastCheckWindowLeft[wIdx] = Rect[wIdx].x;
        lastCheckWindowTop[wIdx] = Rect[wIdx].y;
        var flag7 = NextCheckGameTick <= GameMain.gameTick;
        if (flag7)
        {
            ConfigManager.CheckConfig(ConfigManager.Step.State);
        }
    }

    public static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        var flag = CruiseAssistMainUI.ViewMode == CruiseAssistMainUIViewMode.Full;
        if (flag)
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
        var flag2 = AutoPilotPlugin.State == AutoPilotState.Inactive;
        if (flag2)
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
        var flag3 = GUILayout.Button("Config", guistyle3, Array.Empty<GUILayoutOption>());
        if (flag3)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            var show = AutoPilotConfigUI.Show;
            var num = wIdx;
            show[num] = !show[num];
            var flag4 = AutoPilotConfigUI.Show[wIdx];
            if (flag4)
            {
                AutoPilotConfigUI.TempMinEnergyPer = AutoPilotPlugin.Conf.MinEnergyPer.ToString();
                AutoPilotConfigUI.TempMaxSpeed = AutoPilotPlugin.Conf.MaxSpeed.ToString();
                AutoPilotConfigUI.TempWarpMinRangeAU = AutoPilotPlugin.Conf.WarpMinRangeAU.ToString();
                AutoPilotConfigUI.TempSpeedToWarp = AutoPilotPlugin.Conf.SpeedToWarp.ToString();
            }
        }
        var flag5 = GUILayout.Button("Start", guistyle3, Array.Empty<GUILayoutOption>());
        if (flag5)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotPlugin.State = AutoPilotState.Active;
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.Button("-", guistyle3, Array.Empty<GUILayoutOption>());
        var flag6 = GUILayout.Button("Stop", guistyle3, Array.Empty<GUILayoutOption>());
        if (flag6)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotPlugin.State = AutoPilotState.Inactive;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    private static int wIdx;

    public const float WindowWidthFull = 398f;

    public const float WindowHeightFull = 150f;

    public const float WindowWidthMini = 288f;

    public const float WindowHeightMini = 70f;

    public static Rect[] Rect = {
        new Rect(0f, 0f, 398f, 150f),
        new Rect(0f, 0f, 398f, 150f)
    };

    private static float[] lastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static float[] lastCheckWindowTop = { float.MinValue, float.MinValue };

    public static long NextCheckGameTick = long.MaxValue;
}