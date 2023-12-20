using System;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotConfigUI
{
    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99031292, Rect[_wIdx], WindowFunction, "AutoPilot - Config", CruiseAssistMainUI.WindowStyle, Array.Empty<GUILayoutOption>());
        var scale = CruiseAssistMainUI.Scale / 100f;
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
            if (Rect[_wIdx].x != LastCheckWindowLeft[_wIdx] || Rect[_wIdx].y != LastCheckWindowTop[_wIdx])
            {
                AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
        LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
    }

    public static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            fixedHeight = 20f,
            alignment = TextAnchor.MiddleLeft
        };
        var guistyle2 = new GUIStyle(CruiseAssistMainUI.BaseTextFieldStyle)
        {
            fontSize = 12,
            fixedWidth = 60f
        };
        guistyle.fixedHeight = 20f;
        guistyle2.alignment = TextAnchor.MiddleRight;
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        guistyle.fixedWidth = 240f;
        GUILayout.Label("Min Energy Percent (0-100 default:20)".Translate(), guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        SetValue(ref TempMinEnergyPer, GUILayout.TextField(TempMinEnergyPer, guistyle2, Array.Empty<GUILayoutOption>()), 0, 100, ref AutoPilotPlugin.Conf.MinEnergyPer);
        guistyle.fixedWidth = 20f;
        GUILayout.Label("%", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        guistyle.fixedWidth = 240f;
        GUILayout.Label("Max Speed (0-2000 default:2000)".Translate(), guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        SetValue(ref TempMaxSpeed, GUILayout.TextField(TempMaxSpeed, guistyle2, Array.Empty<GUILayoutOption>()), 0, 2000, ref AutoPilotPlugin.Conf.MaxSpeed);
        guistyle.fixedWidth = 20f;
        GUILayout.Label("m/s", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        guistyle.fixedWidth = 240f;
        GUILayout.Label("Warp Min Range (1-60 default:2)".Translate(), guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        SetValue(ref TempWarpMinRangeAU, GUILayout.TextArea(TempWarpMinRangeAU, guistyle2, Array.Empty<GUILayoutOption>()), 1, 60, ref AutoPilotPlugin.Conf.WarpMinRangeAU);
        guistyle.fixedWidth = 20f;
        GUILayout.Label("AU", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        guistyle.fixedWidth = 240f;
        GUILayout.Label("Speed to warp (0-2000 default:1200)".Translate(), guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        SetValue(ref TempSpeedToWarp, GUILayout.TextArea(TempSpeedToWarp, guistyle2, Array.Empty<GUILayoutOption>()), 0, 2000, ref AutoPilotPlugin.Conf.SpeedToWarp);
        guistyle.fixedWidth = 20f;
        GUILayout.Label("m/s", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        var guistyle3 = new GUIStyle(CruiseAssistMainUI.BaseToggleStyle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
        GUI.changed = false;
        AutoPilotPlugin.Conf.LocalWarpFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.LocalWarpFlag, "Warp to planet in local system.".Translate(), guistyle3, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        AutoPilotPlugin.Conf.AutoStartFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.AutoStartFlag, "Start AutoPilot when set target planet.".Translate(), guistyle3, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        AutoPilotPlugin.Conf.MainWindowJoinFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.MainWindowJoinFlag, "Join AutoPilot window to CruiseAssist window.".Translate(), guistyle3, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndVertical();
        if (GUI.Button(new Rect(Rect[_wIdx].width - 16f, 1f, 16f, 16f), "", CruiseAssistMainUI.CloseButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUI.DragWindow();
    }

    private static bool SetValue(ref string temp, string instr, int min, int max, ref int value)
    {
        if (string.IsNullOrEmpty(instr))
        {
            temp = string.Empty;
            return false;
        }

        if (!int.TryParse(instr, out var num)) return false;
        if (num < min)
        {
            num = min;
        }
        else if (max < num)
        {
            num = max;
        }
        value = num;
        temp = value.ToString();
        return true;

    }

    private static int _wIdx;

    public const float WindowWidth = 400f;

    public const float WindowHeight = 400f;

    public static readonly bool[] Show = new bool[2];

    public static readonly Rect[] Rect = {
        new Rect(0f, 0f, 400f, 400f),
        new Rect(0f, 0f, 400f, 400f)
    };

    private static readonly float[] LastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static readonly float[] LastCheckWindowTop = { float.MinValue, float.MinValue };

    public static string TempMinEnergyPer;

    public static string TempMaxSpeed;

    public static string TempWarpMinRangeAU;

    public static string TempSpeedToWarp;
}