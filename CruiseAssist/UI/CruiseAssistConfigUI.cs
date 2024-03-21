using System;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistConfigUI
{
    public static void OnInit()
    {
        Strings.OnLanguageChanged += LangChanged;
        LangChanged();
        _labelStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 120f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        };
        _toolbarButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseToolbarButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        _uiScaleStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 60f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        };
        _horizontalSliderStyle = new GUIStyle(CruiseAssistMainUI.BaseHorizontalSliderStyle)
        {
            fixedWidth = 180f,
            margin =
            {
                top = 10
            },
            alignment = TextAnchor.MiddleLeft
        };
        _horizontalSliderThunbStyle = new GUIStyle(CruiseAssistMainUI.BaseHorizontalSliderThumbStyle)
        {
            border = new RectOffset(1, 1, 8, 8)
        };
        _percentStyle = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 40f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        };
        _toggleStyle = new GUIStyle(CruiseAssistMainUI.BaseToggleStyle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
        _setButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 60f,
            fixedHeight = 18f,
            margin = 
            {
                top = 6
            },
            fontSize = 12
        };
        _closeButtonStyle = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        return;

        void LangChanged()
        {
            _viewModeTexts = [Strings.Get(30), Strings.Get(31)];
        }
    }

    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99030293, Rect[_wIdx], WindowFunction, "CruiseAssist - " + Strings.Get(5), CruiseAssistMainUI.WindowStyle);
        var num = CruiseAssistMainUI.Scale / 100f;
        if (Screen.width / num < Rect[_wIdx].xMax)
        {
            Rect[_wIdx].x = Screen.width / num - Rect[_wIdx].width;
        }
        if (Rect[_wIdx].x < 0f)
        {
            Rect[_wIdx].x = 0f;
        }
        if (Screen.height / num < Rect[_wIdx].yMax)
        {
            Rect[_wIdx].y = Screen.height / num - Rect[_wIdx].height;
        }
        if (Rect[_wIdx].y < 0f)
        {
            Rect[_wIdx].y = 0f;
        }
        if (LastCheckWindowLeft[_wIdx] != float.MinValue)
        {
            if (Rect[_wIdx].x != LastCheckWindowLeft[_wIdx] || Rect[_wIdx].y != LastCheckWindowTop[_wIdx])
            {
                LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
                LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        else
        {
            LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
            LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
        }
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label(Strings.Get(21), _labelStyle);
        var viewMode = CruiseAssistMainUI.ViewMode == CruiseAssistMainUIViewMode.Full ? 0 : 1;
        GUI.changed = false;
        var value = GUILayout.Toolbar(viewMode, _viewModeTexts, _toolbarButtonStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
        }
        if (value != viewMode)
        {
            if (value != 0)
            {
                if (value == 1)
                {
                    CruiseAssistMainUI.ViewMode = CruiseAssistMainUIViewMode.Mini;
                }
            }
            else
            {
                CruiseAssistMainUI.ViewMode = CruiseAssistMainUIViewMode.Full;
            }
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(Strings.Get(22), _uiScaleStyle);
        TempScale = GUILayout.HorizontalSlider(TempScale, 80f, 240f, _horizontalSliderStyle, _horizontalSliderThunbStyle);
        TempScale = (int)TempScale / 5 * 5;
        GUILayout.Label(TempScale.ToString("0") + "%", _percentStyle);
        if (GUILayout.Button(Strings.Get(14), _setButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.Scale = TempScale;
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndHorizontal();
        GUI.changed = false;
        CruiseAssistPlugin.Conf.MarkVisitedFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.MarkVisitedFlag, Strings.Get(23), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.SelectFocusFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.SelectFocusFlag, Strings.Get(24), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag, Strings.Get(25), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag, Strings.Get(26), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag, Strings.Get(27), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag, Strings.Get(28), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag, Strings.Get(29), _toggleStyle);
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (!CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag && GUILayout.Button(Strings.Get(20), _closeButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        if (GUI.Button(new Rect(Rect[_wIdx].width - 16f, 1f, 16f, 16f), "", CruiseAssistMainUI.CloseButtonStyle))
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUI.DragWindow();
    }

    private static int _wIdx;
    public const float WindowWidth = 400f;
    public const float WindowHeight = 400f;
    public static readonly bool[] Show = new bool[2];
    public static readonly Rect[] Rect =
    [
        new Rect(0f, 0f, 400f, 400f),
        new Rect(0f, 0f, 400f, 400f)
    ];
    private static readonly float[] LastCheckWindowLeft = [float.MinValue, float.MinValue];
    private static readonly float[] LastCheckWindowTop = [float.MinValue, float.MinValue];
    public static float TempScale;
    private static GUIStyle _labelStyle;
    private static GUIStyle _toolbarButtonStyle;
    private static GUIStyle _uiScaleStyle;
    private static GUIStyle _horizontalSliderStyle;
    private static GUIStyle _horizontalSliderThunbStyle;
    private static GUIStyle _percentStyle;
    private static GUIStyle _toggleStyle;
    private static GUIStyle _setButtonStyle;
    private static GUIStyle _closeButtonStyle;
    private static string[] _viewModeTexts = [Strings.Get(30), Strings.Get(31)];
}