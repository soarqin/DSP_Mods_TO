using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XianTu.UI
{
	internal class XianTuBasePanel : BasePanel
	{
		public override void OnEnter()
		{
			base.OnEnter();
			UIManager = UIManager.Instance;
			BindBaseEvent();
			_uRect = UITool.GameObject.GetComponent<RectTransform>();
			_defaultRect = _uRect.rect;
		}

		public UIManager UIManager { get; set; }

		private void BindBaseEvent()
		{
			BindCloseWindowBtn();
			var eventTrigger = UITool.GameObject.AddComponent<EventTrigger>();
			eventTrigger.triggers = [];
			var transform = UITool.Transform;
			TriggerAdd(eventTrigger, EventTriggerType.Drag, delegate(BaseEventData x)
			{
				var pointerEventData = x as PointerEventData;
				var flag = pointerEventData != null;
				if (flag)
				{
					var vector = pointerEventData.position - _dragBias;
					var width = _defaultRect.width;
					var height = _defaultRect.height;
					switch (_dragMode)
					{
					case EDragMode.Drag:
						transform.position = _sourceDragPoint + vector;
						break;
					case EDragMode.ScaleX:
						_uRect.sizeDelta = new Vector2(width + vector.x, height);
						break;
					case EDragMode.ScaleY:
						_uRect.sizeDelta = new Vector2(width, height - vector.y);
						break;
					case EDragMode.ScaleXY:
						_uRect.sizeDelta = new Vector2(width + vector.x, height - vector.y);
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			});
			TriggerAdd(eventTrigger, EventTriggerType.PointerDown, delegate(BaseEventData x)
			{
				var pointerEventData2 = x as PointerEventData;
				_sourceDragPoint = transform.position;
				_dragBias = pointerEventData2.position;
				var position = pointerEventData2.position;
				var position2 = transform.position;
				var num = position.x - position2.x;
				var num2 = position.y - position2.y;
				var rect = _uRect.rect;
				var flag2 = rect.height - 20f < num2;
				var flag3 = flag2;
				if (flag3)
				{
					_dragMode = EDragMode.Drag;
				}
				else
				{
					var flag4 = num > rect.width - 20f;
					var flag5 = -num2 > rect.height - 20f;
					var flag6 = flag4 && flag5;
					if (flag6)
					{
						_dragMode = EDragMode.ScaleXY;
					}
					else
					{
						var flag7 = flag4;
						if (flag7)
						{
							_dragMode = EDragMode.ScaleX;
						}
						else
						{
							var flag8 = flag5;
							if (flag8)
							{
								_dragMode = EDragMode.ScaleY;
							}
							else
							{
								_dragMode = EDragMode.Drag;
							}
						}
					}
				}
			});
		}

		private void BindCloseWindowBtn()
		{
			var orAddComponentInChildren = UITool.GetOrAddComponentInChildren<Button>("Btn_Close");
			var flag = orAddComponentInChildren != null;
			if (flag)
			{
				var component = orAddComponentInChildren.gameObject.GetComponent<Button>();
				component.onClick.AddListener(delegate
				{
					UIManager.SetActive(false);
				});
			}
			else
			{
				Debug.LogError("没有找到关闭按钮");
			}
		}

		private static void TriggerAdd(EventTrigger trigger, EventTriggerType eventID, UnityAction<BaseEventData> callback)
		{
			var entry = new EventTrigger.Entry();
			entry.eventID = eventID;
			entry.callback.AddListener(callback);
			trigger.triggers.Add(entry);
		}

		private Vector2 _dragBias;

		private Vector2 _sourceDragPoint;

		private RectTransform _uRect;

		private EDragMode _dragMode = EDragMode.Drag;

		private Rect _defaultRect;

		private enum EDragMode
		{
			Drag,
			ScaleX,
			ScaleY,
			ScaleXY
		}
	}
}
