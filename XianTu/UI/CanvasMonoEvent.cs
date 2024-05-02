using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace XianTu.UI
{
	public class CanvasMonoEvent : MonoBehaviour
	{
		private void Awake()
		{
			onEnableEvent = new UnityEvent();
		}

		private void OnEnable()
		{
			var onEnableEvent = this.onEnableEvent;
			if (onEnableEvent != null)
			{
				onEnableEvent.Invoke();
			}
		}

		[FormerlySerializedAs("OnEnableEvent")] public UnityEvent onEnableEvent;
	}
}
