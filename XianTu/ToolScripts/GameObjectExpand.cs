using UnityEngine;

namespace ToolScripts
{
	public static class GameObjectExpand
	{
		public static T Find<T>(this GameObject obj, string name) where T : class
		{
			var gameObject = GameObject.Find(name);
			bool flag = gameObject;
			T t;
			if (flag)
			{
				t = gameObject.GetComponent<T>();
			}
			else
			{
				t = default(T);
			}
			return t;
		}

		public static T Find<T>(this Transform obj, string name) where T : class
		{
			var transform = obj.Find(name);
			bool flag = transform;
			T t;
			if (flag)
			{
				t = transform.GetComponent<T>();
			}
			else
			{
				t = default(T);
			}
			return t;
		}

		public static bool TryFind<T>(this GameObject obj, string name, out T result) where T : class
		{
			var gameObject = GameObject.Find(name);
			bool flag = gameObject;
			bool flag2;
			if (flag)
			{
				result = gameObject.GetComponent<T>();
				flag2 = true;
			}
			else
			{
				result = default(T);
				flag2 = false;
			}
			return flag2;
		}

		public static bool TryFind<T>(this Transform obj, string name, out T result) where T : class
		{
			var transform = obj.Find(name);
			bool flag = transform;
			bool flag2;
			if (flag)
			{
				result = transform.GetComponent<T>();
				flag2 = true;
			}
			else
			{
				result = default(T);
				flag2 = false;
			}
			return flag2;
		}
	}
}
