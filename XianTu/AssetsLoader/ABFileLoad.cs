using UnityEngine;

namespace AssetsLoader
{
	internal class ABFileLoad : ABLoad
	{
		public ABFileLoad(string filepath)
		{
			Ab = AssetBundle.LoadFromFile(filepath);
			Init();
		}
	}
}
