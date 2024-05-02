using UnityEngine;

namespace AssetsLoader
{
	public interface ILoad
	{
		GameObject LoadPrefab(string path);

		string LoadText(string path);
	}
}
