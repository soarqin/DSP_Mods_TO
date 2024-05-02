using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XianTu.UI
{
	public class UIManager
	{
		public GameObject CanvasObj { get; private set; }

		public bool ActiveSelf => CanvasObj.activeSelf;

		public void SetActive(bool value)
		{
			CanvasObj.SetActive(value);
		}

		public void SwitchDisplay()
		{
			CanvasObj.SetActive(CanvasObj.activeSelf);
		}

		public UIManager(Action<object> debugLog = null)
		{
			Instance = this;
			var flag = debugLog == null;
			if (flag)
			{
				DebugLog = new Action<object>(Debug.Log);
			}
			var flag2 = !LoadCanvas();
			if (flag2)
			{
				DebugLog("UI管理器加载失败");
			}
			else
			{
				PanelManager = new PanelManager(CanvasObj);
			}
		}

		public PanelManager PanelManager { get; private set; }

		public static UIManager Instance { get; private set; }

		public CanvasMonoEvent CanvasMonoEvent { get; private set; }

		private bool LoadCanvas()
		{
			var gameObject = Singleton<LoadManager>.Instance.LoadPrefab("Prefabs/XianTuCanvas");
			var flag = gameObject != null;
			bool flag2;
			if (flag)
			{
				var gameObject2 = Object.Instantiate(gameObject);
				CanvasObj = gameObject2;
				CanvasMonoEvent = gameObject2.AddComponent<CanvasMonoEvent>();
				gameObject2.SetActive(false);
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		private const string CanvasPrefabPath = "Prefabs/XianTuCanvas";

		public readonly Action<object> DebugLog;
	}
}
