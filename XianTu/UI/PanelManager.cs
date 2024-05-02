using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XianTu.UI
{
	public class PanelManager
	{
		public LoadManager Loader { get; private set; }

		public PanelManager(GameObject rootCanvas)
		{
			_panelStack = new Stack<BasePanel>();
			_panelDict = new Dictionary<string, BasePanel>();
			this._rootCanvas = rootCanvas;
			Loader = Singleton<LoadManager>.Instance;
			_uiAssembly = GetType().Assembly;
		}

		public void Push(string nextPanelName)
		{
			var flag = _panelStack.Count > 0;
			if (flag)
			{
				_panelStack.Peek().OnPause();
			}
			var flag2 = _panelDict.ContainsKey(nextPanelName);
			BasePanel basePanel;
			if (flag2)
			{
				basePanel = _panelDict[nextPanelName];
				basePanel.OnResume();
			}
			else
			{
				var text = Path.Combine(UIPrefabDir, nextPanelName);
				var gameObject = Loader.LoadPrefab(text);
				var flag3 = gameObject == null;
				if (flag3)
				{
					Debug.LogError("没有找到" + text);
					var flag4 = _panelStack.Count > 0;
					if (flag4)
					{
						_panelStack.Peek().OnResume();
					}
					return;
				}
				gameObject = Object.Instantiate(gameObject, _rootCanvas.transform);
				var type = _uiAssembly.GetType(_panelNameSapce + "." + nextPanelName);
				basePanel = (BasePanel)gameObject.AddComponent(type);
				var uitool = new UITool(gameObject);
				basePanel.Init(this);
				basePanel.Init(uitool);
				_panelDict.Add(nextPanelName, basePanel);
				basePanel.OnEnter();
			}
			_panelStack.Push(basePanel);
		}

		public void Pop()
		{
			var flag = _panelStack.Count > 0;
			if (flag)
			{
				var basePanel = _panelStack.Pop();
				basePanel.OnPause();
			}
			var flag2 = _panelStack.Count > 0;
			if (flag2)
			{
				_panelStack.Peek().OnResume();
			}
		}

		public void Exit()
		{
			for (var i = 0; i < _panelDict.Count; i++)
			{
				_panelDict.ElementAt(i).Value.OnExit();
			}
		}

		private Stack<BasePanel> _panelStack;

		private Dictionary<string, BasePanel> _panelDict;

		private GameObject _rootCanvas;

		private Assembly _uiAssembly;

		private string _panelNameSapce = "XianTu.UI";

		public readonly string UIPrefabDir = "Prefabs/UI";
	}
}
