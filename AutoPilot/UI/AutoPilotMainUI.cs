using AutoPilot.Commons;
using AutoPilot.Enums;
using CruiseAssist;
using CruiseAssist.Enums;
using CruiseAssist.UI;
using UnityEngine;

namespace AutoPilot.UI;

internal static class AutoPilotMainUI
{
    public static void OnInit()
    {
        _windowStyle = new GUIStyle(CruiseAssistMainUI.WindowStyle)
        {
            fontSize = 11
        };

        _baseLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        
        _highlightedLabelStyle = new GUIStyle(_baseLabelStyle)
        {
            normal = { textColor = Color.cyan }
        };
        
        _ngLabelStyle = new GUIStyle(_baseLabelStyle)
        {
            normal = { textColor = Color.red }
        };

        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 160f,
            fixedHeight = 32f,
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };

        _buttonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 55f,
            fixedHeight = 18f,
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter
        };
    }

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

        Rect[_wIdx] = GUILayout.Window(99031291, Rect[_wIdx], WindowFunction, "AutoPilot", _windowStyle);
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
            if (Rect[_wIdx].x != LastCheckWindowLeft[_wIdx] || Rect[_wIdx].y != LastCheckWindowTop[_wIdx])
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
        GUILayout.BeginVertical();
        if (CruiseAssistMainUI.ViewMode == CruiseAssistMainUIViewMode.Full)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            int status;
            if (AutoPilotPlugin.State == AutoPilotState.Inactive)
                status = 0;
            else if (AutoPilotPlugin.EnergyPer <= AutoPilotPlugin.Conf.MinEnergyPer)
                status = 1;
            else
                status = 2;
            switch (status)
            {
                case 1:
                    GUILayout.Label(Strings.Get(3) + " : " + Strings.Get(7), _ngLabelStyle);
                    break;
                case 2:
                    GUILayout.Label(Strings.Get(3) + " : " + Strings.Get(8), _highlightedLabelStyle);
                    break;
                default:
                    GUILayout.Label(Strings.Get(3) + " : ---", _baseLabelStyle);
                    break;
            }
            if (AutoPilotPlugin.State == AutoPilotState.Inactive || CruiseAssistPlugin.TargetStar == null || GameMain.mainPlayer.warping ||
                (!AutoPilotPlugin.Conf.LocalWarpFlag && GameMain.localStar != null && CruiseAssistPlugin.TargetStar.id == GameMain.localStar.id) ||
                CruiseAssistPlugin.TargetRange < AutoPilotPlugin.Conf.WarpMinRangeAu * 40000)
                status = 0;
            else if (AutoPilotPlugin.WarperCount < 1)
                status = 1;
            else
                status = 2;
            switch (status)
            {
                case 1:
                    GUILayout.Label(Strings.Get(4) + " : " + Strings.Get(7), _ngLabelStyle);
                    break;
                case 2:
                    GUILayout.Label(Strings.Get(4) + " : " + Strings.Get(8), _highlightedLabelStyle);
                    break;
                default:
                    GUILayout.Label(Strings.Get(4) + " : ---", _baseLabelStyle);
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (AutoPilotPlugin.State == AutoPilotState.Inactive)
                status = 0;
            else if (AutoPilotPlugin.LeavePlanet)
                status = 2;
            else
                status = 1;
            switch (status)
            {
                case 1:
                    GUILayout.Label(Strings.Get(5) + " : " + Strings.Get(10), _baseLabelStyle);
                    break;
                case 2:
                    GUILayout.Label(Strings.Get(5) + " : " + Strings.Get(9), _highlightedLabelStyle);
                    break;
                default:
                    GUILayout.Label(Strings.Get(5) + " : ---", _baseLabelStyle);
                    break;
            }

            if (AutoPilotPlugin.State == AutoPilotState.Inactive)
                status = 0;
            else if (AutoPilotPlugin.SpeedUp)
                status = 2;
            else
                status = 1;
            switch (status)
            {
                case 1:
                    GUILayout.Label(Strings.Get(6) + " : " + Strings.Get(12), _baseLabelStyle);
                    break;
                case 2:
                    GUILayout.Label(Strings.Get(6) + " : " + Strings.Get(11), _highlightedLabelStyle);
                    break;
                default:
                    GUILayout.Label(Strings.Get(6) + " : ---", _baseLabelStyle);
                    break;
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        GUILayout.BeginHorizontal();
        if (AutoPilotPlugin.State == AutoPilotState.Inactive)
        {
            GUILayout.Label(Strings.Get(13), _labelStyle);
        }
        else
        {
            _labelStyle.normal.textColor = Color.cyan;
            GUILayout.Label(Strings.Get(14), _labelStyle);
        }

        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        if (GUILayout.Button(Strings.Get(0), _buttonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            var show = AutoPilotConfigUI.Show;
            var num = _wIdx;
            show[num] = !show[num];
            if (AutoPilotConfigUI.Show[_wIdx])
            {
                AutoPilotConfigUI.TempMinEnergyPer = AutoPilotPlugin.Conf.MinEnergyPer.ToString();
                AutoPilotConfigUI.TempMaxSpeed = AutoPilotPlugin.Conf.MaxSpeed.ToString();
                AutoPilotConfigUI.TempWarpMinRangeAu = AutoPilotPlugin.Conf.WarpMinRangeAu.ToString();
                AutoPilotConfigUI.TempSpeedToWarp = AutoPilotPlugin.Conf.SpeedToWarp.ToString();
            }
        }

        if (GUILayout.Button(Strings.Get(1), _buttonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            AutoPilotPlugin.State = AutoPilotState.Active;
        }

        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Button("-", _buttonStyle);
        if (GUILayout.Button(Strings.Get(2), _buttonStyle))
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

    public const float WindowWidthFull = 408f;

    public const float WindowHeightFull = 150f;

    public const float WindowWidthMini = 298f;

    public const float WindowHeightMini = 70f;

    public static readonly Rect[] Rect =
    [
        new Rect(0f, 0f, 398f, 150f),
        new Rect(0f, 0f, 398f, 150f)
    ];

    private static GUIStyle _windowStyle;
    private static GUIStyle _baseLabelStyle;
    private static GUIStyle _highlightedLabelStyle;
    private static GUIStyle _ngLabelStyle;
    private static GUIStyle _labelStyle;
    private static GUIStyle _buttonStyle;

    private static readonly float[] LastCheckWindowLeft = [float.MinValue, float.MinValue];

    private static readonly float[] LastCheckWindowTop = [float.MinValue, float.MinValue];

    public static long NextCheckGameTick = long.MaxValue;
}