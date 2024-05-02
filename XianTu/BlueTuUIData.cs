using System;
using System.Diagnostics;
using UnityEngine;

namespace XianTu
{
	public class BlueTuUIData
	{
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnValueChange;

		public static BlueTuUIData Instance { get; } = new();

		public Vector3 Bias
		{
			get => _bias;
            set
			{
				_bias = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public Vector3 Scale
		{
			get => _scale;
            set
			{
				_scale = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public Vector3 Pivot
		{
			get => _pivot;
            set
			{
				_pivot = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public float LayerHeight
		{
			get => _layerHeight;
            set
			{
				_layerHeight = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public float Rotate
		{
			get => _rotate;
            set
			{
				_rotate = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public int LayerNumber
		{
			get => _layerNumber;
            set
			{
				var flag = LayerNumber < 1;
				if (!flag)
				{
					_layerNumber = value;
					var onValueChange = OnValueChange;
					if (onValueChange != null)
					{
						onValueChange();
					}
				}
			}
		}

		public bool Enable
		{
			get => _enable;
            set
			{
				_enable = value;
				var onValueChange = OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
			}
		}

		public BlueTuUIData Clone()
		{
			var blueTuUIData = (BlueTuUIData)MemberwiseClone();
			blueTuUIData.OnValueChange = null;
			return blueTuUIData;
		}

		public void Reset()
		{
			var blueTuUIData = new BlueTuUIData();
			_bias = blueTuUIData._bias;
			_scale = blueTuUIData._scale;
			_pivot = blueTuUIData._pivot;
			_layerHeight = blueTuUIData._layerHeight;
			_layerNumber = blueTuUIData._layerNumber;
			_rotate = blueTuUIData._rotate;
			_enable = true;
		}


		private Vector3 _bias = new(0f, 0f, 0f);

		private Vector3 _scale = new(1f, 1f, 1f);

		private Vector3 _pivot = new(0f, 0f, 0f);

		private float _layerHeight = 5f;

		private int _layerNumber = 1;

		private float _rotate;

		private bool _enable = true;

		public Action OnBuildBtn;

		public Action OnResetBtn;

		public Action OnCopyBtn;
	}
}
