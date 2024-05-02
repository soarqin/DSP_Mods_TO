using UnityEngine;

namespace XianTu.UI
{
	internal class BasePanel : MonoBehaviour
	{
		public virtual void OnEnter()
		{
		}

		public virtual void OnPause()
		{
			UITool.GameObject.SetActive(false);
		}

		public virtual void OnResume()
		{
			UITool.GameObject.SetActive(true);
		}

		public virtual void OnExit()
		{
			Destroy(UITool.GameObject);
		}

		public void Init(PanelManager panelManager)
		{
			PanelManager = panelManager;
		}

		public PanelManager PanelManager { get; private set; }

		public void Init(UITool uiTool)
		{
			UITool = uiTool;
		}

		public UITool UITool { get; private set; }
	}
}
