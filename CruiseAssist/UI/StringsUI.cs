using System;
using CruiseAssist.Commons;
using HarmonyLib;

namespace CruiseAssist.UI;

public static class Strings
{
    private static readonly string[] EngUs = [
        // 0
        "Target\n System:",
        "Target\n Planet:",
        "Cruise Assist Disabled.",
        "Cruise Assist Inactive.",
        "Cruise Assist Active.",
        // 5
        "Config",
        "Disable",
        "Enable",
        "StarList",
        "Cancel",
        // 10
        "Normal",
        "History",
        "Bookmark",
        "Target",
        "SET",
        // 15
        "Delete",
        "Sort",
        "SET",
        "ADD",
        "DEL",
        // 20
        "Close",
        "Main Window Style :",
        "UI Scale :",
        "Mark the visited system and planet.",
        "Focus when select target.",
        // 25
        "Hide duplicate history.",
        "Disable lock cursor when starting sail mode.",
        "Show main window when target selected, even not sail mode.",
        "Close StarList when set target planet.",
        "Hide bottom close button.",
        // 30
        "FULL",
        "MINI"
    ];

    private static readonly string[] ZhoCn =
    [
        // 0
        "目标\n 星系:",
        "目标\n 行星:",
        "Cruise Assist 已禁用",
        "Cruise Assist 未在工作",
        "Cruise Assist 正在工作",
        // 5
        "设置",
        "禁用",
        "启用",
        "天体列表",
        "取消",
        // 10
        "常规",
        "历史记录",
        "书签",
        "目标",
        "设置",
        // 15
        "删除",
        "排序",
        "设为目标",
        "添加",
        "删除",
        // 20
        "关闭",
        "主窗口样式 :",
        "界面缩放 :",
        "标记已访问的星系和行星",
        "设置目标时聚焦该天体",
        // 25
        "隐藏重复历史记录",
        "进入航行模式时不锁定鼠标光标",
        "选定目标天体时即使不在航行模式也显示主窗口",
        "设置目标后关闭天体列表窗口",
        "隐藏底部关闭按钮",
        // 30
        "完整",
        "迷你"
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