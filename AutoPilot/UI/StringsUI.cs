using System;
using HarmonyLib;

namespace AutoPilot.UI;

public static class Strings
{
    private static readonly string[] EngUs = [
        // 0
        "Config",
        "Start",
        "Stop",
        "Energy",
        "Warper",
        // 5
        "Leaving Planet",
        "Speed UP",
        "NG",
        "OK",
        "YES",
        // 10
        "NO",
        "ON",
        "OFF",
        "Auto Pilot Inactive.",
        "Auto Pilot Active.",
        // 15
        "Min Energy Percent (0-100 default:20)",
        "Max Speed (0-2000 default:2000)",
        "Warp Min Range (1-60 default:2)",
        "Speed to warp (0-2000 default:1200)",
        "Warp to planet in local system.",
        // 20
        "Start AutoPilot when set target planet.",
        "Join AutoPilot window to CruiseAssist window.",
    ];

    private static readonly string[] ZhoCn =
    [
        // 0
        "设置",
        "开始",
        "停止",
        "能量",
        "翘曲",
        // 5
        "离开行星",
        "加速",
        "不足",
        "OK",
        "是",
        // 10
        "否",
        "开",
        "关",
        "Auto Pilot 未在工作",
        "Auto Pilot 正在工作",
        // 15
        "最小能量百分比 (0-100 默认：20)",
        "最大速度 (0-2000 默认：2000)",
        "启用曲速最小距离 (1-60 默认：2)",
        "启动曲速的航行速度 (0-2000 默认：1200)",
        "即使本星系的行星也允许曲速",
        // 20
        "设置目标行星后立即开始导航",
        "合并 AutoPilot 和 CruiseAssist 窗口",
    ];
    
    public static Action OnLanguageChanged;
    
    private static string[] _langStr = EngUs;

    public static string Get(int index) => _langStr[index];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Localization), nameof(Localization.LoadSettings))]
    [HarmonyPatch(typeof(Localization), nameof(Localization.LoadLanguage))]
    [HarmonyPatch(typeof(Localization), nameof(Localization.NotifyLanguageChange))]
    private static void Localization_LanguageChanged_Postfix()
    {
        var newstr = Localization.Languages[Localization.currentLanguageIndex].lcId switch
        {
            2052 => ZhoCn,
            _ => EngUs
        };
        if (newstr == _langStr) return;
        _langStr = newstr;
        OnLanguageChanged?.Invoke();
    }
}