using System.IO;
using AssetsLoader;
using UnityEngine;
using XianTu.UI;

namespace XianTu
{
	internal class XianTuTest : MonoBehaviour
	{
		private void Awake()
		{
			var abload = ABLoader.LoadFromFile("xiantu", "Assets/AB");
			Singleton<LoadManager>.Instance.SetLoader(abload);
			_canvas = new UIManager();
			cube = GameObject.Find("Cube");
			_canvas.PanelManager.Push("BlueTuPatchUI");
			var data = BlueTuUIData.Instance;
			var trans = cube.transform;
			BlueTuUIData.Instance.OnValueChange += delegate
			{
				trans.localScale = data.Scale;
				trans.position = data.Bias;
				trans.eulerAngles = new Vector3(0f, data.Rotate, 0f);
			};
		}

		private static void LoadTest()
		{
			var text = Path.Combine("Assets/XianTu/AB", "xiantu");
			var assetBundle = AssetBundle.LoadFromFile(text);
			var allAssetNames = assetBundle.GetAllAssetNames();
			foreach (var text2 in allAssetNames)
            {
                Debug.Log("全路径: " + text2);
                var text3 = text2.Substring(0, text2.IndexOf("prefabs"));
                Debug.Log("测试切割" + text3);
                var text4 = text2.Substring(text2.LastIndexOf('/') + 1);
                text4 = text4.Split(['.'])[0];
                Debug.Log("名字: " + text4);
                var gameObject = assetBundle.LoadAsset<GameObject>(text4);
                bool flag = gameObject;
                if (flag)
                {
                    var gameObject2 = Instantiate(gameObject);
                    gameObject2.name = "路径: " + text4;
                }
            }
			Debug.Log("路径streamingAssetsPath：" + Application.streamingAssetsPath);
			Debug.Log("dataPath：" + Application.dataPath);
			Debug.Log("persistentDataPath：" + Application.persistentDataPath);
		}

		private void Update()
		{
			var keyDown = Input.GetKeyDown(hotKey);
			if (keyDown)
			{
				_canvas.SetActive(!_canvas.ActiveSelf);
			}
		}

        private UIManager _canvas;

		public KeyCode hotKey = KeyCode.F2;

		public GameObject cube;
	}
}
