using System;
using System.Diagnostics;
using System.Linq;
using ToolScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XianTu.UI
{
	internal class UIValue<T> where T : IComparable
	{
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<object> ValueUpdate;

		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T> OnValueChange;

		public T Value
		{
			get => _value;
            set
			{
				var publicValueLock = _publicValueLock;
				if (!publicValueLock)
				{
					_publicValueLock = true;
					ValueUpdate(value);
					_publicValueLock = false;
				}
			}
		}

		public float MaxValue
		{
			get => _maxValue;
            set
			{
				_maxValue = value;
				for (var i = 0; i < Components.Length; i++)
				{
					var component = Components[i];
					var component2 = component;
					var slider = component2 as Slider;
					if (slider != null)
					{
						slider.maxValue = value;
					}
				}
			}
		}

		public UIValue(T defaultValue, params Component[] components)
		{
			var flag = typeof(T) == typeof(bool);
			if (flag)
			{
				_valueType = EValueType.Bool;
			}
			else
			{
				var flag2 = typeof(T) == typeof(int);
				if (flag2)
				{
					_valueType = EValueType.Int;
				}
				else
				{
					var flag3 = typeof(T) == typeof(float);
					if (flag3)
					{
						_valueType = EValueType.Float;
					}
					else
					{
						var flag4 = typeof(T) == typeof(string);
						if (flag4)
						{
							_valueType = EValueType.String;
						}
						else
						{
							var flag5 = typeof(T) == typeof(double);
							if (flag5)
							{
								_valueType = EValueType.Double;
							}
						}
					}
				}
			}
			ValueUpdate = delegate(object v)
			{
				switch (_valueType)
				{
				case EValueType.Bool:
					_value = (T)((object)Convert.ToBoolean(v));
					break;
				case EValueType.Int:
					_value = (T)((object)Convert.ToInt32(v));
					break;
				case EValueType.Float:
					_value = (T)((object)Convert.ToSingle(v));
					break;
				case EValueType.Double:
					_value = (T)((object)Convert.ToDouble(v));
					break;
				case EValueType.String:
					_value = (T)((object)_valueType.ToString());
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange(_value);
				}
			};
			foreach (var component in components)
			{
				var component2 = component;
				var component3 = component2;
				var inputField = component3 as InputField;
				if (inputField == null)
				{
					var toggle = component3 as Toggle;
					if (toggle == null)
					{
						var slider = component3 as Slider;
						if (slider == null)
						{
							var text = component3 as Text;
							if (text == null)
							{
								var scrollbar = component3 as Scrollbar;
								if (scrollbar != null)
								{
									Bind(scrollbar);
								}
							}
							else
							{
								Bind(text);
							}
						}
						else
						{
							Bind(slider);
						}
					}
					else
					{
						Bind(toggle);
					}
				}
				else
				{
					Bind(inputField);
				}
			}
		}

		public void Bind(Scrollbar scrollBar)
		{
			var flag = scrollBar == null;
			if (!flag)
			{
				Components = Components.Append(scrollBar).ToArray();
				var hasSet = false;
				var justUpdate = false;
				ValueUpdate += delegate(object value)
				{
					var hasSet2 = hasSet;
					if (!hasSet2)
					{
						justUpdate = true;
						scrollBar.value = Convert.ToSingle(value);
						justUpdate = false;
					}
				};
				scrollBar.onValueChanged.AddListener(delegate(float value)
				{
					var justUpdate2 = justUpdate;
					if (!justUpdate2)
					{
						hasSet = true;
						ValueUpdate(value);
						hasSet = false;
					}
				});
			}
		}

		public void Bind(Text text)
		{
			var flag = text == null;
			if (!flag)
			{
				Components = Components.Append(text).ToArray();
				var justUpdate = false;
				ValueUpdate += delegate(object value)
				{
					var justUpdate2 = justUpdate;
					if (!justUpdate2)
					{
						justUpdate = true;
						text.text = string.Format("{0:f2}", value);
						justUpdate = false;
					}
				};
			}
		}

		public void Bind(Slider slider)
		{
			var flag = slider == null;
			if (!flag)
			{
				Components = Components.Append(slider).ToArray();
				var hasSet = false;
				var justUpdate = false;
				ValueUpdate += delegate(object value)
				{
					var hasSet2 = hasSet;
					if (!hasSet2)
					{
						justUpdate = true;
						slider.value = Convert.ToSingle(value);
						justUpdate = false;
					}
				};
				slider.onValueChanged.AddListener(delegate(float value)
				{
					var justUpdate2 = justUpdate;
					if (!justUpdate2)
					{
						hasSet = true;
						ValueUpdate(value);
						hasSet = false;
					}
				});
			}
		}

		public void Bind(Toggle toggle)
		{
			var flag = toggle == null;
			if (!flag)
			{
				Components = Components.Append(toggle).ToArray();
				var hasSet = false;
				var justUpdate = false;
				ValueUpdate += delegate(object value)
				{
					var hasSet2 = hasSet;
					if (!hasSet2)
					{
						justUpdate = true;
						toggle.isOn = Convert.ToBoolean(value);
						justUpdate = false;
					}
				};
				toggle.onValueChanged.AddListener(delegate(bool value)
				{
					var justUpdate2 = justUpdate;
					if (!justUpdate2)
					{
						hasSet = true;
						ValueUpdate(value);
						hasSet = false;
					}
				});
			}
		}

		public void Bind(InputField inputField)
		{
			var flag = inputField == null;
			if (!flag)
			{
				Components = Components.Append(inputField).ToArray();
				var hasSet = false;
				var justUpdate = false;
				ValueUpdate += delegate(object value)
				{
					var hasSet2 = hasSet;
					if (!hasSet2)
					{
						justUpdate = true;
						inputField.text = string.Format("{0:f2}", value);
						justUpdate = false;
					}
				};
				inputField.onValueChanged.AddListener(delegate(string value)
				{
					var justUpdate2 = justUpdate;
					if (!justUpdate2)
					{
						var flag2 = value == "-" || value == "";
						if (!flag2)
						{
							hasSet = true;
							switch (_valueType)
							{
							case EValueType.Bool:
								ValueUpdate(Convert.ToBoolean(value));
								break;
							case EValueType.Int:
								ValueUpdate(Convert.ToInt32(value));
								break;
							case EValueType.Float:
								ValueUpdate(Convert.ToSingle(value));
								break;
							case EValueType.Double:
								ValueUpdate(Convert.ToDouble(value));
								break;
							case EValueType.String:
								ValueUpdate(value);
								break;
							default:
								throw new ArgumentOutOfRangeException();
							}
							hasSet = false;
						}
					}
				});
			}
		}

		public void BindDragControl(GameObject gameObject)
		{
			var startPosition = default(Vector2);
			var defaultValue = 0f;
			var eventTrigger = gameObject.GetComponent<EventTrigger>();
			var flag = eventTrigger == null;
			if (flag)
			{
				eventTrigger = gameObject.AddComponent<EventTrigger>();
			}
			var speed = 0f;
			eventTrigger.Add(EventTriggerType.BeginDrag, delegate(BaseEventData edata)
			{
				var pointerEventData = edata as PointerEventData;
				var flag2 = pointerEventData != null;
				if (flag2)
				{
					startPosition = pointerEventData.position;
					defaultValue = Convert.ToSingle(Value);
					speed = _maxValue / Screen.width * 4f;
				}
			});
			eventTrigger.Add(EventTriggerType.Drag, delegate(BaseEventData edata)
			{
				var pointerEventData2 = edata as PointerEventData;
				var flag3 = pointerEventData2 != null;
				if (flag3)
				{
					var num = (pointerEventData2.position.x - startPosition.x) * speed;
					ValueUpdate(defaultValue + num);
				}
			});
		}

		public static void BindDragControl(GameObject controller, UIValue<T> uiValueX, UIValue<T> uiValueY)
		{
			var startPosition = default(Vector2);
			var defaultValue1 = 0f;
			var defaultValue2 = 0f;
			var eventTrigger = controller.GetComponent<EventTrigger>();
			var flag = eventTrigger == null;
			if (flag)
			{
				eventTrigger = controller.AddComponent<EventTrigger>();
			}
			var speed = 0f;
			eventTrigger.Add(EventTriggerType.BeginDrag, delegate(BaseEventData edata)
			{
				var pointerEventData = edata as PointerEventData;
				var flag2 = pointerEventData != null;
				if (flag2)
				{
					startPosition = pointerEventData.position;
					defaultValue1 = Convert.ToSingle(uiValueX.Value);
					defaultValue2 = Convert.ToSingle(uiValueY.Value);
					speed = uiValueX._maxValue / Screen.width * 4f;
				}
			});
			eventTrigger.Add(EventTriggerType.Drag, delegate(BaseEventData edata)
			{
				var pointerEventData2 = edata as PointerEventData;
				var flag3 = pointerEventData2 != null;
				if (flag3)
				{
					var vector = (pointerEventData2.position - startPosition) * speed;
					uiValueX.ValueUpdate(defaultValue1 + vector.x);
					uiValueY.ValueUpdate(defaultValue2 + vector.y);
				}
			});
		}

		private T _value;

		private float _maxValue = 1f;

		private readonly EValueType _valueType;

		public Component[] Components = new Component[0];

		private bool _publicValueLock = false;

		private enum EValueType
		{
			Bool,
			Int,
			Float,
			Double,
			String
		}
	}
}
