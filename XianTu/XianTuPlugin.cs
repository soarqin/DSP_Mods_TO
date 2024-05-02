using AssetsLoader;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using XianTu.Scripts.DataController;
using XianTu.UI;

namespace XianTu
{
	[BepInPlugin("me.GammaChineYov.plugin.Dyson.XianTu", "XianTu", "1.0.5")]
	internal class XianTuPlugin : BaseUnityPlugin
	{
        private static KeyCode HotKey => _hotKey.Value;

		private void Awake()
		{
            _logger = Logger;
			_hotKey = Config.Bind("config", "HotKey", KeyCode.F2, "显示UI的快捷键");
			var abload = ABLoader.LoadFromEmbeddedAssets("xiantu");
			Singleton<LoadManager>.Instance.SetLoader(abload);
			Singleton<LoadManager>.Instance.AddLoader(new EmbeddedFileLoad());
			UIManager = new UIManager();
			UIManager.SetActive(false);
			UIManager.PanelManager.Push("BlueTuPatchUI");
			new BlueTuController();
			_logger.LogInfo("仙图加载完毕");
		}

		public UIManager UIManager { get; set; }

		private void Update()
		{
			var keyDown = Input.GetKeyDown(HotKey);
			if (keyDown)
			{
				UIManager.SetActive(!UIManager.ActiveSelf);
			}
		}


        private static ManualLogSource _logger;

		private static ConfigEntry<KeyCode> _hotKey;

    }
}
