using AutoPilot.Commons;
using AutoPilot.UI;
using BepInEx.Configuration;

namespace AutoPilot;

internal class AutoPilotConfigManager : ConfigManager
{
    internal AutoPilotConfigManager(ConfigFile Config)
        : base(Config)
    {
    }

    protected override void CheckConfigImplements(Step step)
    {
        var ok = false;
        if (step == Step.Awake)
        {
            var configEntry = Bind("Base", "ModVersion", "0.0.4", "Don't change.");
            configEntry.Value = "0.0.4";
            ok = true;
        }

        if (step is Step.Awake or Step.GameMainBegin)
        {
            AutoPilotDebugUI.Show = Bind("Debug", "DebugWindowShow", false).Value;
            AutoPilotPlugin.Conf.MinEnergyPer = Bind("Setting", "MinEnergyPer", 20).Value;
            AutoPilotPlugin.Conf.MaxSpeed = Bind("Setting", "MaxSpeed", 2000).Value;
            AutoPilotPlugin.Conf.WarpMinRangeAU = Bind("Setting", "WarpMinRangeAU", 2).Value;
            AutoPilotPlugin.Conf.SpeedToWarp = Bind("Setting", "WarpSpeed", 1200).Value;
            AutoPilotPlugin.Conf.LocalWarpFlag = Bind("Setting", "LocalWarp", false).Value;
            AutoPilotPlugin.Conf.AutoStartFlag = Bind("Setting", "AutoStart", true).Value;
            AutoPilotPlugin.Conf.MainWindowJoinFlag = Bind("Setting", "MainWindowJoin", true).Value;
            for (var i = 0; i < 2; i++)
            {
                AutoPilotMainUI.Rect[i].x = Bind("State", $"MainWindow{i}Left", 100).Value;
                AutoPilotMainUI.Rect[i].y = Bind("State", $"MainWindow{i}Top", 100).Value;
                AutoPilotConfigUI.Rect[i].x = Bind("State", $"ConfigWindow{i}Left", 100).Value;
                AutoPilotConfigUI.Rect[i].y = Bind("State", $"ConfigWindow{i}Top", 100).Value;
            }
            AutoPilotDebugUI.Rect.x = Bind("State", "DebugWindowLeft", 100).Value;
            AutoPilotDebugUI.Rect.y = Bind("State", "DebugWindowTop", 100).Value;
        }
        else if (step == Step.State)
        {
            LogManager.LogInfo("check state.");
            ok |= UpdateEntry("Setting", "MinEnergyPer", AutoPilotPlugin.Conf.MinEnergyPer);
            ok |= UpdateEntry("Setting", "MaxSpeed", AutoPilotPlugin.Conf.MaxSpeed);
            ok |= UpdateEntry("Setting", "WarpMinRangeAU", AutoPilotPlugin.Conf.WarpMinRangeAU);
            ok |= UpdateEntry("Setting", "WarpSpeed", AutoPilotPlugin.Conf.SpeedToWarp);
            ok |= UpdateEntry("Setting", "LocalWarp", AutoPilotPlugin.Conf.LocalWarpFlag);
            ok |= UpdateEntry("Setting", "AutoStart", AutoPilotPlugin.Conf.AutoStartFlag);
            ok |= UpdateEntry("Setting", "MainWindowJoin", AutoPilotPlugin.Conf.MainWindowJoinFlag);
            for (var j = 0; j < 2; j++)
            {
                ok |= UpdateEntry("State", $"MainWindow{j}Left", (int)AutoPilotMainUI.Rect[j].x);
                ok |= UpdateEntry("State", $"MainWindow{j}Top", (int)AutoPilotMainUI.Rect[j].y);
                ok |= UpdateEntry("State", $"ConfigWindow{j}Left", (int)AutoPilotConfigUI.Rect[j].x);
                ok |= UpdateEntry("State", $"ConfigWindow{j}Top", (int)AutoPilotConfigUI.Rect[j].y);
            }
            ok |= UpdateEntry("State", "DebugWindowLeft", (int)AutoPilotDebugUI.Rect.x);
            ok |= UpdateEntry("State", "DebugWindowTop", (int)AutoPilotDebugUI.Rect.y);
            AutoPilotMainUI.NextCheckGameTick = long.MaxValue;
        }

        if (ok)
        {
            Save();
        }
    }
}