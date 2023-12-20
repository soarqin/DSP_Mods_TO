using System;
using System.Linq;
using CruiseAssist.Commons;
using UnityEngine;

namespace CruiseAssist.UI;

public static class CruiseAssistDebugUI
{
    public static void OnGUI()
    {
        var guistyle = new GUIStyle(GUI.skin.window)
        {
            fontSize = 11
        };
        Rect = GUILayout.Window(99030294, Rect, WindowFunction, "CruiseAssist - Debug", guistyle, Array.Empty<GUILayoutOption>());
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
            var flag6 = Rect.x != _lastCheckWindowLeft || Rect.y != _lastCheckWindowTop;
            if (flag6)
            {
                CruiseAssistMainUI.NextCheckGameTick = GameMain.gameTick + 300L;
            }
        }
        _lastCheckWindowLeft = Rect.x;
        _lastCheckWindowTop = Rect.y;
    }

    private static void WindowFunction(int windowId)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        var guistyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, Array.Empty<GUILayoutOption>());
        var reticuleTargetStar = CruiseAssistPlugin.ReticuleTargetStar;
        GUILayout.Label($"CruiseAssistPlugin.ReticuleTargetStar.id={(reticuleTargetStar != null ? new int?(reticuleTargetStar.id) : null)}", guistyle, Array.Empty<GUILayoutOption>());
        var reticuleTargetPlanet = CruiseAssistPlugin.ReticuleTargetPlanet;
        GUILayout.Label($"CruiseAssistPlugin.ReticuleTargetPlanet.id={(reticuleTargetPlanet != null ? new int?(reticuleTargetPlanet.id) : null)}", guistyle, Array.Empty<GUILayoutOption>());
        var selectTargetStar = CruiseAssistPlugin.SelectTargetStar;
        GUILayout.Label($"CruiseAssistPlugin.SelectTargetStar.id={(selectTargetStar != null ? new int?(selectTargetStar.id) : null)}", guistyle, Array.Empty<GUILayoutOption>());
        var selectTargetPlanet = CruiseAssistPlugin.SelectTargetPlanet;
        GUILayout.Label($"CruiseAssistPlugin.SelectTargetPlanet.id={(selectTargetPlanet != null ? new int?(selectTargetPlanet.id) : null)}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.navigation.indicatorAstroId={GameMain.mainPlayer.navigation.indicatorAstroId}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input0.w={GameMain.mainPlayer.controller.input0.w}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input0.x={GameMain.mainPlayer.controller.input0.x}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input0.y={GameMain.mainPlayer.controller.input0.y}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input0.z={GameMain.mainPlayer.controller.input0.z}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input1.w={GameMain.mainPlayer.controller.input1.w}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input1.x={GameMain.mainPlayer.controller.input1.x}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input1.y={GameMain.mainPlayer.controller.input1.y}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GameMain.mainPlayer.controller.input1.z={GameMain.mainPlayer.controller.input1.z}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"VFInput._sailSpeedUp={VFInput._sailSpeedUp}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"CruiseAssistPlugin.Enable={CruiseAssistPlugin.Enable}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"CruiseAssistPlugin.History={CruiseAssistPlugin.History.Count()}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label("CruiseAssistPlugin.History=" + ListUtils.ToString(CruiseAssistPlugin.History), guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GUI.skin.window.margin.top={GUI.skin.window.margin.top}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GUI.skin.window.border.top={GUI.skin.window.border.top}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GUI.skin.window.padding.top={GUI.skin.window.padding.top}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.Label($"GUI.skin.window.overflow.top={GUI.skin.window.overflow.top}", guistyle, Array.Empty<GUILayoutOption>());
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    public static bool Show = false;

    public static Rect Rect = new Rect(0f, 0f, 400f, 400f);

    private static float _lastCheckWindowLeft = float.MinValue;

    private static float _lastCheckWindowTop = float.MinValue;

    private static Vector2 _scrollPos = Vector2.zero;
}