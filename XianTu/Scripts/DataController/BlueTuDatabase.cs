using UnityEngine;

namespace XianTu.Scripts.DataController
{
	internal static class BlueTuDatabase
	{
		public static BlueprintData Load(string path)
		{
			var flag = !path.EndsWith(".txt");
			if (flag)
			{
				path += ".txt";
			}
			var text = Singleton<LoadManager>.Instance.LoadText(path);
			var flag2 = text == "";
			BlueprintData blueprintData;
			if (flag2)
			{
				Debug.Log("没有找到蓝图文件");
				blueprintData = null;
			}
			else
			{
				var blueprintData2 = BlueprintData.CreateNew(text);
				var flag3 = blueprintData2 == null;
				if (flag3)
				{
					Debug.Log("蓝图文件加载失败");
				}
				blueprintData = blueprintData2;
			}
			return blueprintData;
		}
	}
}
