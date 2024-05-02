using UnityEngine;

namespace AssetsLoader
{
	public class ResourceLoad : ILoad
	{
		public GameObject LoadPrefab(string path)
		{
			return Resources.Load<GameObject>(path);
		}

		public string LoadText(string path)
		{
			return Resources.Load<TextAsset>(path).text;
		}
	}
}
