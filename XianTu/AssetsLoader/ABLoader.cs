using System.IO;

namespace AssetsLoader
{
	public static class ABLoader
	{
		public static ABLoad LoadFromFile(string abFilename, string dir)
		{
			var text = Path.Combine(dir, abFilename);
			return new ABFileLoad(text);
		}

		public static ABLoad LoadFromFile(string filepath)
		{
			return new ABFileLoad(filepath);
		}

		public static ABLoad LoadFromEmbeddedAssets(string abFilename, string defaultNamespace = null)
		{
			return new ABEmbeddedAssetsLoad(abFilename, defaultNamespace);
		}

		public static ABLoad LoadFromDll(string abFilename, string dllFilepath, string dllDefaultNameSpace)
		{
			return new ABEmbeddedAssetsLoad(abFilename, dllFilepath, dllDefaultNameSpace);
		}
	}
}
