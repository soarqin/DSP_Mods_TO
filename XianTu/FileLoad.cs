using System.IO;
using AssetsLoader;
using UnityEngine;

namespace XianTu
{
	internal class FileLoad(string dirPath) : ILoad
    {

		public GameObject LoadPrefab(string path)
		{
			return null;
		}

		public string LoadText(string path)
		{
			return File.ReadAllText(Path.Combine(_dirPath, path));
		}

		private readonly string _dirPath = dirPath ?? "";
	}
}
