using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ToolScripts
{
	internal static class EventTriggerExpand
	{
		public static void Add(this EventTrigger trigger, EventTriggerType eventID, UnityAction<BaseEventData> callback)
		{
			var flag = trigger.triggers == null;
			if (flag)
			{
				trigger.triggers = [];
			}
			var entry = new EventTrigger.Entry();
			entry.eventID = eventID;
			entry.callback.AddListener(callback);
			trigger.triggers.Add(entry);
		}
	}
}
