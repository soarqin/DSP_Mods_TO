using System;
using System.IO;
using UnityEngine;

namespace AssetsLoader
{
	public abstract class ABLoad : ILoad
	{
		protected void Init()
		{
			foreach (var text in Ab.GetAllAssetNames())
			{
				var flag = text.Contains("prefabs");
				if (flag)
				{
					PrefabPath = text.Substring(0, text.IndexOf("prefabs", StringComparison.OrdinalIgnoreCase));
					break;
				}
			}
		}

		public GameObject LoadPrefab(string path)
		{
			var flag = Ab == null;
			GameObject gameObject;
			if (flag)
			{
				Debug.Log("内嵌的AB包没有找到");
				gameObject = null;
			}
			else
			{
				var flag2 = path.Contains("/");
				if (flag2)
				{
					path = Path.Combine(PrefabPath, path);
					var flag3 = !path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase);
					if (flag3)
					{
						path += ".prefab";
					}
				}
				gameObject = Ab.LoadAsset<GameObject>(path);
			}
			return gameObject;
		}

		public string LoadText(string path)
		{
			var flag = Ab == null;
			string text;
			if (flag)
			{
				Debug.Log("内嵌的AB包没有找到");
				text = null;
			}
			else
			{
				var flag2 = path.Contains("/");
				if (flag2)
				{
					path = Path.Combine(_txtPath, path);
					var flag3 = !path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase);
					if (flag3)
					{
						path += ".prefab";
					}
				}
				text = Ab.LoadAsset<TextAsset>(path).text;
			}
			return text;
		}

		protected string PrefabPath;

		protected AssetBundle Ab;

		private string _txtPath;
	}
}
