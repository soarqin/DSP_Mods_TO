using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AssetsLoader
{
	internal class ABEmbeddedAssetsLoad : ABLoad
	{
		public ABEmbeddedAssetsLoad(string filename, string assetsNamespace = null)
		{
			var stackTrace = new StackTrace();
			var frame = stackTrace.GetFrame(1);
			var assembly = frame.GetMethod().DeclaringType.Assembly;
			var flag = assetsNamespace == null;
			if (flag)
			{
				assetsNamespace = assembly.FullName.Split([','])[0];
			}
			LoadAssetsFromEmbedded(filename, assetsNamespace, assembly);
		}

		private void LoadAssetsFromEmbedded(string filename, string assetsNamespace, Assembly assembly)
		{
			var text = assetsNamespace + "." + filename;
			var manifestResourceStream = assembly.GetManifestResourceStream(text);
			var flag = manifestResourceStream == null;
			if (flag)
			{
				Debug.Log(string.Concat(["在", assembly.FullName, "找不到内嵌的资源", text, ",请检查内嵌资源中是否有它:"]));
				foreach (var text2 in assembly.GetManifestResourceNames())
				{
					Debug.Log(text2);
				}
				Debug.Log("------------------------");
			}
			else
			{
				Ab = AssetBundle.LoadFromStream(manifestResourceStream);
				Init();
			}
		}

		public ABEmbeddedAssetsLoad(string filename, string dllFilepath, string dllNamespace)
		{
			var assembly = Assembly.LoadFrom(dllFilepath);
			LoadAssetsFromEmbedded(filename, dllNamespace, assembly);
		}
	}
}
