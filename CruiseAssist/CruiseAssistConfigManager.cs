using System.Collections.Generic;
using BepInEx.Configuration;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using CruiseAssist.UI;

namespace CruiseAssist;

internal class CruiseAssistConfigManager(ConfigFile config) : ConfigManager(config)
{

    protected override void CheckConfigImplements(Step step)
    {
        var saveFlag = false;
        if (step == Step.Awake)
        {
            var configEntry = Bind("Base", "ModVersion", "0.0.37", "Don't change.");
            configEntry.Value = "0.0.37";
            Migration("State", "MainWindow0Left", 100, "State", "InfoWindowLeft");
            Migration("State", "MainWindow0Top", 100, "State", "InfoWindowTop");
            Migration("State", "MainWindow0Left", 100, "State", "MainWindowLeft");
            Migration("State", "MainWindow0Top", 100, "State", "MainWindowTop");
            Migration("State", "StarListWindow0Left", 100, "State", "StarListWindowLeft");
            Migration("State", "StarListWindow0Top", 100, "State", "StarListWindowTop");
            Migration("Setting", "CloseStarListWhenSetTargetPlanet", false, "Setting", "HideStarListWhenSetTargetPlanet");
            saveFlag = true;
        }
        if (step is Step.Awake or Step.GameMainBegin)
        {
            CruiseAssistDebugUI.Show = Bind("Debug", "DebugWindowShow", false).Value;
            CruiseAssistPlugin.Enable = Bind("Setting", "Enable", true).Value;
            CruiseAssistPlugin.Conf.MarkVisitedFlag = Bind("Setting", "MarkVisited", true).Value;
            CruiseAssistPlugin.Conf.SelectFocusFlag = Bind("Setting", "SelectFocus", true).Value;
            CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag = Bind("Setting", "HideDuplicateHistory", true).Value;
            CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag = Bind("Setting", "AutoDisableLockCursor", false).Value;
            CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag = Bind("Setting", "ShowMainWindowWhenTargetSelectedEvenNotSailMode", true).Value;
            CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag = Bind("Setting", "CloseStarListWhenSetTargetPlanet", false).Value;
            CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag = Bind("Setting", "HideBottomCloseButton", true).Value;
            CruiseAssistMainUI.Scale = Bind("Setting", "UIScale", 150).Value;
            var viewModeStr = Bind("Setting", "MainWindowViewMode", CruiseAssistMainUIViewMode.Full.ToString()).Value;
            EnumUtils.TryParse(viewModeStr, out CruiseAssistMainUI.ViewMode);
            for (var i = 0; i < 2; i++)
            {
                CruiseAssistMainUI.Rect[i].x = Bind("State", $"MainWindow{i}Left", 100).Value;
                CruiseAssistMainUI.Rect[i].y = Bind("State", $"MainWindow{i}Top", 100).Value;
                CruiseAssistStarListUI.Rect[i].x = Bind("State", $"StarListWindow{i}Left", 100).Value;
                CruiseAssistStarListUI.Rect[i].y = Bind("State", $"StarListWindow{i}Top", 100).Value;
                CruiseAssistConfigUI.Rect[i].x = Bind("State", $"ConfigWindow{i}Left", 100).Value;
                CruiseAssistConfigUI.Rect[i].y = Bind("State", $"ConfigWindow{i}Top", 100).Value;
            }
            CruiseAssistStarListUI.ListSelected = Bind("State", "StarListWindowListSelected", 0).Value;
            CruiseAssistDebugUI.Rect.x = Bind("State", "DebugWindowLeft", 100).Value;
            CruiseAssistDebugUI.Rect.y = Bind("State", "DebugWindowTop", 100).Value;
        }
        if (step is Step.Awake or Step.GameMainBegin or Step.State or Step.ChangeSeed)
        {
            if (DSPGame.IsMenuDemo || GameMain.galaxy == null)
            {
                var flag6 = CruiseAssistPlugin.Seed != -1;
                if (flag6)
                {
                    CruiseAssistPlugin.Seed = -1;
                    CruiseAssistPlugin.History = new List<int>();
                    CruiseAssistPlugin.Bookmark = new List<int>();
                    LogManager.LogInfo("clear seed.");
                }
            }
            else
            {
                if (CruiseAssistPlugin.Seed != GameMain.galaxy.seed)
                {
                    CruiseAssistPlugin.Seed = GameMain.galaxy.seed;
                    CruiseAssistPlugin.History = ListUtils.ParseToIntList(Bind("Save", $"History_{CruiseAssistPlugin.Seed}", "").Value);
                    CruiseAssistPlugin.Bookmark = ListUtils.ParseToIntList(Bind("Save", $"Bookmark_{CruiseAssistPlugin.Seed}", "").Value);
                    LogManager.LogInfo($"change seed {CruiseAssistPlugin.Seed}.");
                }
            }
        }
        if (step == Step.State)
        {
            LogManager.LogInfo("check state.");
            saveFlag |= UpdateEntry("Setting", "Enable", CruiseAssistPlugin.Enable);
            saveFlag |= UpdateEntry("Setting", "MarkVisited", CruiseAssistPlugin.Conf.MarkVisitedFlag);
            saveFlag |= UpdateEntry("Setting", "SelectFocus", CruiseAssistPlugin.Conf.SelectFocusFlag);
            saveFlag |= UpdateEntry("Setting", "HideDuplicateHistory", CruiseAssistPlugin.Conf.HideDuplicateHistoryFlag);
            saveFlag |= UpdateEntry("Setting", "AutoDisableLockCursor", CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag);
            saveFlag |= UpdateEntry("Setting", "ShowMainWindowWhenTargetSelectedEvenNotSailMode", CruiseAssistPlugin.Conf.ShowMainWindowWhenTargetSelectedEvenNotSailModeFlag);
            saveFlag |= UpdateEntry("Setting", "CloseStarListWhenSetTargetPlanet", CruiseAssistPlugin.Conf.CloseStarListWhenSetTargetPlanetFlag);
            saveFlag |= UpdateEntry("Setting", "HideBottomCloseButton", CruiseAssistPlugin.Conf.HideBottomCloseButtonFlag);
            saveFlag |= UpdateEntry("Setting", "UIScale", (int)CruiseAssistMainUI.Scale);
            saveFlag |= UpdateEntry("Setting", "MainWindowViewMode", CruiseAssistMainUI.ViewMode.ToString());
            for (var j = 0; j < 2; j++)
            {
                saveFlag |= UpdateEntry("State", $"MainWindow{j}Left", (int)CruiseAssistMainUI.Rect[j].x);
                saveFlag |= UpdateEntry("State", $"MainWindow{j}Top", (int)CruiseAssistMainUI.Rect[j].y);
                saveFlag |= UpdateEntry("State", $"StarListWindow{j}Left", (int)CruiseAssistStarListUI.Rect[j].x);
                saveFlag |= UpdateEntry("State", $"StarListWindow{j}Top", (int)CruiseAssistStarListUI.Rect[j].y);
                saveFlag |= UpdateEntry("State", $"ConfigWindow{j}Left", (int)CruiseAssistConfigUI.Rect[j].x);
                saveFlag |= UpdateEntry("State", $"ConfigWindow{j}Top", (int)CruiseAssistConfigUI.Rect[j].y);
            }
            saveFlag |= UpdateEntry("State", "StarListWindowListSelected", CruiseAssistStarListUI.ListSelected);
            saveFlag |= UpdateEntry("State", "DebugWindowLeft", (int)CruiseAssistDebugUI.Rect.x);
            saveFlag |= UpdateEntry("State", "DebugWindowTop", (int)CruiseAssistDebugUI.Rect.y);
            if (CruiseAssistPlugin.Seed != -1)
            {
                saveFlag |= UpdateEntry("Save", $"History_{CruiseAssistPlugin.Seed}", ListUtils.ToString(CruiseAssistPlugin.History));
                saveFlag |= UpdateEntry("Save", $"Bookmark_{CruiseAssistPlugin.Seed}", ListUtils.ToString(CruiseAssistPlugin.Bookmark));
            }
            CruiseAssistMainUI.NextCheckGameTick = long.MaxValue;
        }
        if (saveFlag)
        {
            Save();
        }
    }
}