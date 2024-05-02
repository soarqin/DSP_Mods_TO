using System.Diagnostics;
using System.IO;
using System.Reflection;
using AssetsLoader;
using UnityEngine;

namespace XianTu
{
	internal class EmbeddedFileLoad : ILoad
	{
		public EmbeddedFileLoad(string assetsNamespace = null)
		{
			var stackTrace = new StackTrace();
			var frame = stackTrace.GetFrame(1);
			var assembly = frame.GetMethod().DeclaringType.Assembly;
			var flag = assetsNamespace == null;
			if (flag)
			{
				assetsNamespace = assembly.FullName.Split([','])[0];
			}
			this._assetsNamespace = assetsNamespace;
			this._assembly = assembly;
		}

		public GameObject LoadPrefab(string path)
		{
			return null;
		}

		public string LoadText(string path)
		{
            using var manifestResourceStream = _assembly.GetManifestResourceStream(_assetsNamespace + "." + path.Replace('/', '.'));
            using var streamReader = new StreamReader(manifestResourceStream);
            return streamReader.ReadToEnd();
		}

		private readonly string _assetsNamespace;

		private readonly Assembly _assembly;
	}
}
