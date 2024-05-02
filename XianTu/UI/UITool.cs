using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace XianTu.UI
{
	internal class UITool
	{
		public UITool(GameObject uiGameObject)
		{
			GameObject = uiGameObject;
			Transform = GameObject.transform;
		}

		public T GetComponentInChild<T>(string path) where T : class
		{
			var gameObject = FindChildGameObject(path);
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

		public GameObject FindChildGameObject(string path)
		{
			var array = path.Split(['/']);
			var flag = array.Length == 0;
			GameObject gameObject;
			if (flag)
			{
				Debug.LogError("空路径");
				gameObject = null;
			}
			else
			{
				var flag2 = array[0] == "";
				GameObject gameObject2;
				if (flag2)
				{
					gameObject2 = StepByStepFindChild(Transform, array.Skip(1).ToArray());
					var flag3 = gameObject2 != null;
					if (flag3)
					{
						return gameObject2.gameObject;
					}
				}
				else
				{
					gameObject2 = BreadthFindChild(Transform, array[0]);
					var flag4 = gameObject2 != null && array.Length > 1;
					if (flag4)
					{
						gameObject2 = StepByStepFindChild(gameObject2.transform, array.Skip(1).ToArray());
					}
				}
				var flag5 = gameObject2 == null;
				if (flag5)
				{
					Debug.LogError("找不到" + path);
				}
				gameObject = gameObject2;
			}
			return gameObject;
		}

		private static GameObject BreadthFindChild(Transform parent, string name)
		{
			var flag = parent == null;
			GameObject gameObject;
			if (flag)
			{
				Debug.LogError("不要传个空的过来！");
				gameObject = null;
			}
			else
			{
				var queue = new Queue<Transform>();
				queue.Enqueue(parent);
				while (queue.Count > 0)
				{
					var transform = queue.Dequeue();
					var transform2 = transform.Find(name);
					var flag2 = transform2 != null;
					if (flag2)
					{
						return transform2.gameObject;
					}
					for (var i = 0; i < transform.childCount; i++)
					{
						queue.Enqueue(transform.GetChild(i));
					}
				}
				gameObject = null;
			}
			return gameObject;
		}

		public T GetOrAddComponentInChildren<T>(string name) where T : Component
		{
			var gameObject = FindChildGameObject(name);
			var flag = gameObject != null;
			T t2;
			if (flag)
			{
				var t = gameObject.GetComponent<T>();
				var flag2 = t == null;
				if (flag2)
				{
					t = gameObject.AddComponent<T>();
				}
				t2 = t;
			}
			else
			{
				t2 = default(T);
			}
			return t2;
		}

		[CompilerGenerated]
		internal static GameObject StepByStepFindChild(Transform parent, string[] filepaths)
		{
			var transform = parent;
			for (var i = 0; i < filepaths.Length; i++)
			{
				transform = transform.Find(filepaths[i]);
				var flag = transform == null;
				if (flag)
				{
					return null;
				}
			}
			return transform.gameObject;
		}

		public readonly GameObject GameObject;

		public readonly Transform Transform;
	}
}
