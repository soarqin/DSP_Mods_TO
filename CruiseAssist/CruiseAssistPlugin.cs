using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BepInEx;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using CruiseAssist.Patches;
using CruiseAssist.UI;
using HarmonyLib;
using UnityEngine;

namespace CruiseAssist;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class CruiseAssistPlugin : BaseUnityPlugin
{
    public void Awake()
    {
        LogManager.Logger = Logger;
        new CruiseAssistConfigManager(Config);
        ConfigManager.CheckConfig(ConfigManager.Step.Awake);
        _harmony = new Harmony("tanu.CruiseAssist.Patch");
        _harmony.PatchAll(typeof(Patch_GameMain));
        _harmony.PatchAll(typeof(Patch_UISailPanel));
        _harmony.PatchAll(typeof(Patch_UIStarmap));
        _harmony.PatchAll(typeof(Patch_UITechTree));
        _harmony.PatchAll(typeof(Patch_PlayerMoveWalk));
        _harmony.PatchAll(typeof(Patch_PlayerMoveDrift));
        _harmony.PatchAll(typeof(Patch_PlayerMoveFly));
        _harmony.PatchAll(typeof(Patch_PlayerMoveSail));
    }

    public void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }

    public static void RegistExtension(ICruiseAssistExtensionAPI extension)
    {
        Extensions.Add(extension);
    }

    public static void UnregistExtension(Type type)
    {
        Extensions.RemoveAll(extension => extension.GetType().FullName == type.FullName);
    }

    public void OnGUI()
    {
        if (DSPGame.IsMenuDemo) return;
        var mainPlayer = GameMain.mainPlayer;
        if (mainPlayer == null) return;
        if (UIMechaEditor.isOpened) return;
        if (!_initialized)
        {
            _initialized = true;
            CruiseAssistMainUI.OnInit();
            CruiseAssistConfigUI.OnInit();
            CruiseAssistStarListUI.OnInit();
            CruiseAssistDebugUI.OnInit();
        }

        var uiRoot = UIRoot.instance;
        var uiGame = uiRoot.uiGame;
        if (!uiGame.guideComplete || uiGame.techTree.active || uiGame.escMenu.active || uiGame.dysonEditor.active || uiGame.hideAllUI0 || uiGame.hideAllUI1) return;
        var uiMilkyWayLoadingSplash = UIMilkyWayLoadingSplash.instance;
        if (uiMilkyWayLoadingSplash != null && uiMilkyWayLoadingSplash.active) return;
        var uiRootUIMilkyWay = uiRoot.uiMilkyWay;
        if (uiRootUIMilkyWay != null && uiRootUIMilkyWay.active) return;
        var starmapActive = uiGame.starmap.active;
        if (!(mainPlayer.sailing || starmapActive || (Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag && TargetSelected))) return;
        if (Seed != GameMain.galaxy.seed)
            ConfigManager.CheckConfig(ConfigManager.Step.ChangeSeed);
        CruiseAssistMainUI.WIdx = starmapActive ? 1 : 0;
        var scale = CruiseAssistMainUI.Scale / 100f;
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
        CruiseAssistMainUI.OnGUI();
        if (CruiseAssistStarListUI.Show[CruiseAssistMainUI.WIdx])
        {
            CruiseAssistStarListUI.OnGUI();
        }
        if (CruiseAssistConfigUI.Show[CruiseAssistMainUI.WIdx])
        {
            CruiseAssistConfigUI.OnGUI();
        }
        if (CruiseAssistDebugUI.Show)
        {
            CruiseAssistDebugUI.OnGUI();
        }
        var ok = ResetInput(CruiseAssistMainUI.Rect[CruiseAssistMainUI.WIdx], scale);
        if (!ok && CruiseAssistStarListUI.Show[CruiseAssistMainUI.WIdx])
        {
            ok = ResetInput(CruiseAssistStarListUI.Rect[CruiseAssistMainUI.WIdx], scale);
        }
        if (!ok && CruiseAssistConfigUI.Show[CruiseAssistMainUI.WIdx])
        {
            ok = ResetInput(CruiseAssistConfigUI.Rect[CruiseAssistMainUI.WIdx], scale);
        }
        if (!ok && CruiseAssistDebugUI.Show)
        {
            ResetInput(CruiseAssistDebugUI.Rect, scale);
        }
        Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
        {
            extension.OnGUI();
        });
    }

    private static bool ResetInput(Rect rect, float scale)
    {
        var num = rect.xMin * scale;
        var num2 = rect.xMax * scale;
        var num3 = rect.yMin * scale;
        var num4 = rect.yMax * scale;
        var x = Input.mousePosition.x;
        var num5 = Screen.height - Input.mousePosition.y;
        var flag = num <= x && x <= num2 && num3 <= num5 && num5 <= num4;
        if (!flag) return false;
        var array = new[] { 0, 1, 2 };
        var flag2 = array.Any(Input.GetMouseButton) || Input.mouseScrollDelta.y != 0f;
        if (!flag2) return false;
        Input.ResetInputAxes();
        return true;
    }

    public static bool Enable = true;
    public static bool TargetSelected = false;
    public static StarData ReticuleTargetStar = null;
    public static PlanetData ReticuleTargetPlanet = null;
    public static StarData SelectTargetStar = null;
    public static PlanetData SelectTargetPlanet = null;
    public static int SelectTargetAstroId = 0;
    public static StarData TargetStar = null;
    public static PlanetData TargetPlanet = null;
    public static VectorLF3 TargetUPos = VectorLF3.zero;
    public static double TargetRange = 0.0;
    public static CruiseAssistState State = CruiseAssistState.Inactive;
    public static bool Interrupt = false;
    public static int Seed = -1;
    public static readonly Func<StarData, string> GetStarName = star => star.displayName;
    public static readonly Func<PlanetData, string> GetPlanetName = planet => planet.displayName;
    internal static readonly List<ICruiseAssistExtensionAPI> Extensions = [];
    private Harmony _harmony;
    private static bool _initialized;

    public static class Conf
    {
        public static bool MarkVisitedFlag = true;
        public static bool SelectFocusFlag = true;
        public static bool HideDuplicateHistoryFlag = true;
        public static bool AutoDisableLockCursorFlag = false;
        public static bool ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag = true;
        public static bool CloseStarListWhenSetTargetPlanetFlag = false;
        public static bool HideBottomCloseButtonFlag = true;
    }
}