using System;
using CruiseAssist.Enums;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistConfigUI
{
    public static void OnGUI()
    {
        _wIdx = CruiseAssistMainUI.WIdx;
        Rect[_wIdx] = GUILayout.Window(99030293, Rect[_wIdx], WindowFunction, "CruiseAssist - Config", CruiseAssistMainUI.WindowStyle, Array.Empty<GUILayoutOption>());
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
                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        LastCheckWindowLeft[_wIdx] = Rect[_wIdx].x;
        LastCheckWindowTop[_wIdx] = Rect[_wIdx].y;
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Label("Main Window Style :", new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 120f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        }, Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(CruiseAssistMainUI.BaseToolbarButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        var array = new[] { "FULL", "MINI" };
        var num = CruiseAssistMainUI.ViewMode == CruiseAssistMainUIViewMode.Full ? 0 : 1;
        GUI.changed = false;
        var num2 = GUILayout.Toolbar(num, array, guistyle, Array.Empty<GUILayoutOption>());
        var changed = GUI.changed;
        if (changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
        }
        if (num2 != num)
        {
            if (num2 != 0)
            {
                if (num2 == 1)
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
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Label("UI Scale :", new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 60f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        }, Array.Empty<GUILayoutOption>());
        var guistyle2 = new GUIStyle(CruiseAssistMainUI.BaseHorizontalSliderStyle)
        {
            fixedWidth = 180f,
            margin =
            {
                top = 10
            },
            alignment = TextAnchor.MiddleLeft
        };
        var guistyle3 = new GUIStyle(CruiseAssistMainUI.BaseHorizontalSliderThumbStyle)
        {
            border = new RectOffset(1, 1, 8, 8)
        };
        TempScale = GUILayout.HorizontalSlider(TempScale, 80f, 240f, guistyle2, guistyle3, Array.Empty<GUILayoutOption>());
        TempScale = (int)TempScale / 5 * 5;
        var guistyle4 = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = 40f,
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft
        };
        GUILayout.Label(TempScale.ToString("0") + "%", guistyle4, Array.Empty<GUILayoutOption>());
        var ok = GUILayout.Button("SET", new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 60f,
            fixedHeight = 18f,
            margin = 
            {
                top = 6
            },
            fontSize = 12
        }, Array.Empty<GUILayoutOption>());
        if (ok)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.Scale = TempScale;
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.EndHorizontal();
        var guistyle5 = new GUIStyle(CruiseAssistMainUI.BaseToggleStyle)
        {
            fixedHeight = 20f,
            fontSize = 12,
            alignment = TextAnchor.LowerLeft
        };
        GUI.changed = false;
        CruiseAssistPlugin.Conf.MarkVisitedFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.MarkVisitedFlag, "Mark the visited system and planet.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.SelectFocusFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.SelectFocusFlag, "Focus when select target.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag, "Hide duplicate history.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag, "Disable lock cursor when starting sail mode.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag, "Show main window when target selected, even not sail mode.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag, "Close StarList when set target planet.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUI.changed = false;
        CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag = GUILayout.Toggle(CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag, "Hide bottom close button.".Translate(), guistyle5, Array.Empty<GUILayoutOption>());
        if (GUI.changed)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        var guistyle6 = new GUIStyle(CruiseAssistMainUI.BaseButtonStyle)
        {
            fixedWidth = 80f,
            fixedHeight = 20f,
            fontSize = 12
        };
        ok = !CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag && GUILayout.Button("Close", guistyle6, Array.Empty<GUILayoutOption>());
        if (ok)
        {
            VFAudio.Create("ui-click-0", null, Vector3.zero, true);
            Show[_wIdx] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        ok = GUI.Button(new Rect(Rect[_wIdx].width - 16f, 1f, 16f, 16f), "", CruiseAssistMainUI.CloseButtonStyle);
        if (ok)
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

    public static readonly Rect[] Rect = {
        new Rect(0f, 0f, 400f, 400f),
        new Rect(0f, 0f, 400f, 400f)
    };

    private static readonly float[] LastCheckWindowLeft = { float.MinValue, float.MinValue };

    private static readonly float[] LastCheckWindowTop = { float.MinValue, float.MinValue };

    public static float TempScale;
}