using System.Collections.Generic;
using System.Linq;

namespace CruiseAssist.Commons;

internal static class ListUtils
{
    public static string ToString(List<int> list)
    {
        return list == null || list.Count == 0 ? "" : list.Select(id => id.ToString()).Aggregate((a, b) => a + "," + b);
    }

    public static string ToString(List<string> list)
    {
        return list == null || list.Count == 0 ? "" : list.Aggregate((a, b) => a + "," + b);
    }

    public static List<int> ParseToIntList(string str)
    {
        return string.IsNullOrEmpty(str)
            ? []
            : str.Split(',').Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();
    }

    public static List<string> ParseToStringList(string str)
    {
        return string.IsNullOrEmpty(str) ? [] : str.Split(',').ToList();
    }
}