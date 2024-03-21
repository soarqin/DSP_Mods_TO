using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using HarmonyLib;

namespace AutoPilot.Commons;

internal abstract class ConfigManager
{
    private static ConfigFile Config { get; set; }
    
    protected static void Init<T>(ConfigFile config) where T: ConfigManager, new()
    {
        _instance = new T();
        Config = config;
        Config.SaveOnConfigSet = false;
    }

    public static void CheckConfig(Step step)
    {
        _instance.CheckConfigImplements(step);
    }

    protected abstract void CheckConfigImplements(Step step);

    public static ConfigEntry<T> Bind<T>(ConfigDefinition configDefinition, T defaultValue, ConfigDescription configDescription = null)
    {
        return Config.Bind(configDefinition, defaultValue, configDescription);
    }

    public static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, ConfigDescription configDescription = null)
    {
        return Config.Bind(section, key, defaultValue, configDescription);
    }

    public static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
    {
        return Config.Bind(section, key, defaultValue, description);
    }

    public static ConfigEntry<T> GetEntry<T>(ConfigDefinition configDefinition)
    {
        ConfigEntry<T> configEntry;
        try
        {
            configEntry = (ConfigEntry<T>)Config[configDefinition];
        }
        catch (KeyNotFoundException ex)
        {
            LogManager.LogError($"{ex.GetType()}: configDefinition={configDefinition}");
            throw;
        }
        return configEntry;
    }

    public static ConfigEntry<T> GetEntry<T>(string section, string key)
    {
        return GetEntry<T>(new ConfigDefinition(section, key));
    }

    public static T GetValue<T>(ConfigDefinition configDefinition)
    {
        return GetEntry<T>(configDefinition).Value;
    }

    public static T GetValue<T>(string section, string key)
    {
        return GetEntry<T>(section, key).Value;
    }

    public static bool ContainsKey(ConfigDefinition configDefinition)
    {
        return Config.ContainsKey(configDefinition);
    }

    public static bool ContainsKey(string section, string key)
    {
        return Config.ContainsKey(new ConfigDefinition(section, key));
    }

    public static bool UpdateEntry<T>(string section, string key, T value) where T : IComparable
    {
        var entry = GetEntry<T>(section, key);
        var value2 = entry.Value;
        var flag = value2.CompareTo(value) == 0;
        bool flag2;
        if (flag)
        {
            flag2 = false;
        }
        else
        {
            entry.Value = value;
            flag2 = true;
        }
        return flag2;
    }

    public static bool RemoveEntry(ConfigDefinition key)
    {
        return Config.Remove(key);
    }

    public static Dictionary<ConfigDefinition, string> GetOrphanedEntries()
    {
        var flag = _orphanedEntries == null;
        if (flag)
        {
            _orphanedEntries = Traverse.Create(Config).Property<Dictionary<ConfigDefinition, string>>("OrphanedEntries").Value;
        }
        return _orphanedEntries;
    }

    public static void Migration<T>(string newSection, string newKey, T defaultValue, string oldSection, string oldKey)
    {
        GetOrphanedEntries();
        var configDefinition = new ConfigDefinition(oldSection, oldKey);
        var flag = _orphanedEntries.TryGetValue(configDefinition, out var text);
        if (!flag) return;
        Bind(newSection, newKey, defaultValue).SetSerializedValue(text);
        _orphanedEntries.Remove(configDefinition);
        LogManager.LogInfo(string.Concat("migration ", oldSection, ".", oldKey, "(", text, ") => ", newSection, ".", newKey));
    }

    public static void Save(bool clearOrphanedEntries = false)
    {
        if (clearOrphanedEntries)
        {
            GetOrphanedEntries().Clear();
        }
        Config.Save();
        LogManager.LogInfo("save config.");
    }

    public static void Clear()
    {
        Config.Clear();
    }

    public static void Reload()
    {
        Config.Reload();
    }

    private static ConfigManager _instance;

    private static Dictionary<ConfigDefinition, string> _orphanedEntries;

    public enum Step
    {
        Awake,
        GameMainBegin,
        UniverseGenCreateGalaxy,
        State,
        ChangeSeed
    }
}