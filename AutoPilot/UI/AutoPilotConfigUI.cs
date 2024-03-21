using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotConfigUI
{
    public static void OnInit()
    {
        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            fixedHeight = 20f,
            alignment = TextAnchor.MiddleLeft
        };
        _textFieldStyle = new GUIStyle(CruiseAssistMainUI.BaseTextFieldStyle)
        {
            fontSize = 12,
            fixedWidth = 60f
        };
        _toggleStyle = new GUIStyle(CruiseAssistMainUI.BaseToggleStyle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
    }
    
    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99031292, Rect[_wIdx], WindowFunction, "AutoPilot - " + Strings.Get(0), CruiseAssistMainUI.WindowStyle);
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

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical();
        _labelStyle.fixedHeight = 20f;
        _textFieldStyle.alignment = TextAnchor.MiddleRight;
        GUILayout.BeginHorizontal();
        _labelStyle.fixedWidth = 240f;
        GUILayout.Label(Strings.Get(15), _labelStyle);
        GUILayout.FlexibleSpace();
        SetValue(ref TempMinEnergyPer, GUILayout.TextField(TempMinEnergyPer, _textFieldStyle), 0, 100, ref AutoPilotPlugin.Conf.MinEnergyPer);
        _labelStyle.fixedWidth = 20f;
        GUILayout.Label("%", _labelStyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _labelStyle.fixedWidth = 240f;
        GUILayout.Label(Strings.Get(16), _labelStyle);
        GUILayout.FlexibleSpace();
        SetValue(ref TempMaxSpeed, GUILayout.TextField(TempMaxSpeed, _textFieldStyle), 0, 2000, ref AutoPilotPlugin.Conf.MaxSpeed);
        _labelStyle.fixedWidth = 20f;
        GUILayout.Label("m/s", _labelStyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _labelStyle.fixedWidth = 240f;
        GUILayout.Label(Strings.Get(17), _labelStyle);
        GUILayout.FlexibleSpace();
        SetValue(ref TempWarpMinRangeAu, GUILayout.TextField(TempWarpMinRangeAu, _textFieldStyle), 1, 60, ref AutoPilotPlugin.Conf.WarpMinRangeAu);
        _labelStyle.fixedWidth = 20f;
        GUILayout.Label("AU", _labelStyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _labelStyle.fixedWidth = 240f;
        GUILayout.Label(Strings.Get(18), _labelStyle);
        GUILayout.FlexibleSpace();
        SetValue(ref TempSpeedToWarp, GUILayout.TextField(TempSpeedToWarp, _textFieldStyle), 0, 2000, ref AutoPilotPlugin.Conf.SpeedToWarp);
        _labelStyle.fixedWidth = 20f;
        GUILayout.Label("m/s", _labelStyle);
        GUILayout.EndHorizontal();
        GUI.changed = false;
        AutoPilotPlugin.Conf.LocalWarpFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.LocalWarpFlag, Strings.Get(19), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        AutoPilotPlugin.Conf.AutoStartFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.AutoStartFlag, Strings.Get(20), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        AutoPilotPlugin.Conf.MainWindowJoinFlag = GUILayout.Toggle(AutoPilotPlugin.Conf.MainWindowJoinFlag, Strings.Get(21), _toggleStyle);
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

    private static GUIStyle _labelStyle;
    private static GUIStyle _textFieldStyle;
    private static GUIStyle _toggleStyle;

    private static int _wIdx;

    public const float WindowWidth = 400f;

    public const float WindowHeight = 400f;

    public static readonly bool[] Show = new bool[2];

    public static readonly Rect[] Rect =
    [
        new(0f, 0f, 400f, 400f),
        new(0f, 0f, 400f, 400f)
    ];

    private static readonly float[] LastCheckWindowLeft = [float.MinValue, float.MinValue];

    private static readonly float[] LastCheckWindowTop = [float.MinValue, float.MinValue];

    public static string TempMinEnergyPer;

    public static string TempMaxSpeed;

    public static string TempWarpMinRangeAu;

    public static string TempSpeedToWarp;
}