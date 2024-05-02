using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace XianTu.UI
{
	internal class BlueTuPatchUI : XianTuBasePanel
	{
		public BlueTuUIData Data { get; private set; }

		public static BlueTuPatchUI Instance { get; private set; }

		public override void OnEnter()
		{
			base.OnEnter();
			Data = BlueTuUIData.Instance;
			enableToggle = UITool.GetComponentInChild<Toggle>("XianTuEnableTg");
			enableToggle.isOn = Data.Enable;
			enableToggle.onValueChanged.AddListener(delegate(bool value)
			{
				Data.Enable = value;
			});
			_tipText = UITool.GetComponentInChild<Button>("Tx_Tip");
			_buildButton = UITool.GetComponentInChild<Button>("Btn_Build");
			_buildButton.onClick.AddListener(delegate
			{
				var onBuildBtn = Data.OnBuildBtn;
				if (onBuildBtn != null)
				{
					onBuildBtn();
				}
			});
			_resetButton = UITool.GetComponentInChild<Button>("Btn_Reset");
			_resetButton.onClick.AddListener(delegate
			{
				var onResetBtn = Data.OnResetBtn;
				if (onResetBtn != null)
				{
					onResetBtn();
				}
				ResetValue();
			});
			_copyButton = UITool.GetComponentInChild<Button>("Btn_Copy");
			_copyButton.onClick.AddListener(delegate
			{
				var onCopyBtn = Data.OnCopyBtn;
				if (onCopyBtn != null)
				{
					onCopyBtn();
				}
			});
			LayerNumber = CreateUIValue(Data.LayerNumber, "LayerNumber");
			LayerHeight = CreateUIValue(Data.LayerHeight, "LayerHeight", 20f);
			var text = "ScaleGroup/Group/ScaleX";
			var text2 = "ScaleGroup/Group/ScaleY";
			ScaleX = CreateUIValue(Data.Scale.x, text);
			ScaleY = CreateUIValue(Data.Scale.y, text2);
			var text3 = "ScaleGroup/Group/ScaleZ";
			ScaleZ = CreateUIValue(Data.Scale.z, text3);
			var text4 = "BiasGroup/Group/BiasX";
			var text5 = "BiasGroup/Group/BiasY";
			var text6 = "BiasGroup/Group/BiasZ";
			_biasX = CreateUIValue(Data.Bias.x, text4, 20f);
			_biasY = CreateUIValue(Data.Bias.y, text5, 20f);
			BiasZ = CreateUIValue(Data.Bias.z, text6, 20f);
			Rotate = CreateUIValue(Data.Rotate, "Rotate", 180f);
			_pivotX = CreateUIValue(Data.Pivot.x, "PivotX");
			_pivotY = CreateUIValue(Data.Pivot.y, "PivotY");
			AddDragControl(Rotate, "Name");
			AddDragControl(LayerNumber, "Name");
			AddDragControl(LayerHeight, "Name");
			AddDragControl(ScaleX, "Name");
			AddDragControl(ScaleY, "Name");
			AddDragControl(ScaleZ, "Name");
			AddDragControl(_pivotX, _pivotY, "Name");
			AddDragControl(_biasX, "Name");
			AddDragControl(_biasY, "Name");
			AddDragControl(BiasZ, "Name");
			BindScaleData();
			BindBiasData();
			BindPivot();
			BindLayerNumber();
			BindLayerHeight();
			BindRotate();
			Instance = this;
			var initFinish = InitFinish;
			if (initFinish != null)
			{
				initFinish(this);
			}
			BindOnEnable_MoveCenter();
		}

		private void ResetValue()
		{
			_biasX.Value = Data.Bias.x;
			_biasY.Value = Data.Bias.y;
			BiasZ.Value = Data.Bias.z;
			ScaleX.Value = Data.Scale.x;
			ScaleY.Value = Data.Scale.y;
			ScaleZ.Value = Data.Scale.z;
			_pivotX.Value = Data.Pivot.x;
			_pivotY.Value = Data.Pivot.y;
			Rotate.Value = Data.Rotate;
			enableToggle.isOn = Data.Enable;
			LayerHeight.Value = Data.LayerHeight;
			LayerNumber.Value = Data.LayerNumber;
		}

		private void BindOnEnable_MoveCenter()
		{
			var canvasMonoEvent = UIManager.CanvasMonoEvent;
			var rectTransform = (RectTransform)transform;
			var centerBias = new Vector3(-rectTransform.rect.width / 2f, rectTransform.rect.height / 2f);
			canvasMonoEvent.onEnableEvent.AddListener(delegate
			{
				var isOn = enableToggle.isOn;
				if (isOn)
				{
					UITool.GameObject.transform.position = Input.mousePosition + centerBias;
				}
			});
		}

		private void BindRotate()
		{
			Rotate.OnValueChange += delegate(float f)
			{
				Data.Rotate = f;
			};
		}

		private void BindLayerHeight()
		{
			LayerHeight.OnValueChange += delegate(float f)
			{
				Data.LayerHeight = f;
			};
		}

		private void BindLayerNumber()
		{
			LayerNumber.OnValueChange += delegate(int i)
			{
				Data.LayerNumber = i;
			};
		}

		private void BindPivot()
		{
			var tempPivot = Data.Pivot;
			var cd = Time.time;
			_pivotX.OnValueChange += delegate(float f)
			{
				var flag = Time.time - cd < Time.fixedDeltaTime;
				if (!flag)
				{
					cd = Time.time;
					tempPivot.x = f;
					Data.Pivot = tempPivot;
				}
			};
			_pivotY.OnValueChange += delegate(float f)
			{
				var flag2 = Time.time - cd < Time.fixedDeltaTime;
				if (!flag2)
				{
					cd = Time.time;
					tempPivot.y = f;
					Data.Pivot = tempPivot;
				}
			};
		}

		private void BindBiasData()
		{
			var tempBias = Data.Bias;
			_biasX.OnValueChange += delegate(float f)
			{
				tempBias.x = f;
				Data.Bias = tempBias;
			};
			_biasY.OnValueChange += delegate(float f)
			{
				tempBias.y = f;
				Data.Bias = tempBias;
			};
			BiasZ.OnValueChange += delegate(float f)
			{
				tempBias.z = f;
				Data.Bias = tempBias;
			};
		}

		private void BindScaleData()
		{
			var tempScale = Data.Scale;
			ScaleX.OnValueChange += delegate(float f)
			{
				tempScale.x = f;
				Data.Scale = tempScale;
			};
			ScaleY.OnValueChange += delegate(float f)
			{
				tempScale.y = f;
				Data.Scale = tempScale;
			};
			ScaleZ.OnValueChange += delegate(float f)
			{
				tempScale.z = f;
				Data.Scale = tempScale;
			};
		}

		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static event Action<BlueTuPatchUI> InitFinish;

		public static event Action<BlueTuPatchUI> OnInitFinish
		{
			add
			{
				InitFinish += value;
				var flag = Instance != null;
				if (flag)
				{
					value(Instance);
				}
			}
			remove => InitFinish -= value;
        }

		private void AddDragControl<T>(UIValue<T> uiValue, string childrenName) where T : IComparable
		{
			var flag = uiValue.Components.Length != 0;
			if (flag)
			{
				var parent = uiValue.Components[0].gameObject.transform.parent;
				var transform = parent.Find(childrenName);
				bool flag2 = transform;
				if (flag2)
				{
					uiValue.BindDragControl(transform.gameObject);
				}
			}
		}

		private void AddDragControl<T>(UIValue<T> uiValueX, UIValue<T> uiValueY, string childrenName) where T : IComparable
		{
			var flag = uiValueX.Components.Length != 0;
			if (flag)
			{
				var parent = uiValueX.Components[0].gameObject.transform.parent;
				var transform = parent.Find(childrenName);
				bool flag2 = transform;
				if (flag2)
				{
					uiValueX.BindDragControl(transform.gameObject);
					uiValueY.BindDragControl(transform.gameObject);
					UIValue<T>.BindDragControl(transform.gameObject, uiValueX, uiValueY);
				}
			}
			var flag3 = uiValueY.Components.Length != 0;
			if (flag3)
			{
				var parent2 = uiValueY.Components[0].gameObject.transform.parent;
				var transform2 = parent2.Find(childrenName);
				bool flag4 = transform2;
				if (flag4)
				{
					uiValueX.BindDragControl(transform2.gameObject);
					uiValueY.BindDragControl(transform2.gameObject);
					UIValue<T>.BindDragControl(transform2.gameObject, uiValueX, uiValueY);
				}
			}
		}

		private UIValue<T> CreateUIValue<T>(T defaultValue, string groupPath, float maxValue = 10f) where T : IComparable
		{
			var uivalue = new UIValue<T>(defaultValue, Array.Empty<Component>());
			var gameObject = UITool.FindChildGameObject(groupPath);
			var flag = gameObject == null;
			UIValue<T> uivalue2;
			if (flag)
			{
				Debug.LogError("没有找到" + groupPath);
				uivalue2 = uivalue;
			}
			else
			{
				uivalue.Bind(gameObject.GetComponentInChildren<Toggle>());
				uivalue.Bind(gameObject.GetComponentInChildren<Slider>());
				uivalue.Bind(gameObject.GetComponentInChildren<Scrollbar>());
				var componentsInChildren = gameObject.GetComponentsInChildren<Text>();
				for (var i = 0; i < componentsInChildren.Length; i++)
				{
					var flag2 = componentsInChildren[i].name == "ValueText";
					if (flag2)
					{
						uivalue.Bind(componentsInChildren[i]);
						break;
					}
				}
				uivalue.Bind(gameObject.GetComponentInChildren<InputField>());
				uivalue.MaxValue = maxValue;
				uivalue2 = uivalue;
			}
			return uivalue2;
		}

		public Toggle enableToggle;

		private Button _tipText;

		private Button _buildButton;

		private Button _resetButton;

		public UIValue<int> LayerNumber;

		public UIValue<float> LayerHeight;

		public UIValue<float> ScaleX;

		public UIValue<float> ScaleY;

		public UIValue<float> ScaleZ;

		public UIValue<float> BiasZ;

		public UIValue<float> Rotate;

		private UIValue<float> _pivotX;

		private UIValue<float> _pivotY;

		private UIValue<float> _biasX;

		private UIValue<float> _biasY;

		private Button _copyButton;
	}
}
