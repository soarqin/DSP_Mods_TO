using System.Collections.Generic;
using AssetsLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XianTu;

public class LoadManager : Singleton<LoadManager>
{
    public void SetLoader(ILoad loader)
    {
        _loaders = [loader];
    }

    public void AddLoader(ILoad loader)
    {
        _loaders.Insert(0, loader);
    }


    public GameObject LoadPrefab(string path)
    {
        foreach (var load in _loaders)
        {
            var gameObject = load.LoadPrefab(path);
            var flag = gameObject != null;
            if (flag)
            {
                return gameObject;
            }
        }
        return null;
    }

    public GameObject LoadPrefab(string path, Transform parent)
    {
        foreach (var load in _loaders)
        {
            var gameObject = load.LoadPrefab(path);
            var flag = gameObject != null;
            if (flag)
            {
                return Object.Instantiate(gameObject, parent);
            }
        }
        return null;
    }

    public string LoadText(string path)
    {
        foreach (var load in _loaders)
        {
            var text = load.LoadText(path);
            var flag = text != "";
            if (flag)
            {
                return text;
            }
        }
        return "";
    }

    private List<ILoad> _loaders = [new ResourceLoad()];
}