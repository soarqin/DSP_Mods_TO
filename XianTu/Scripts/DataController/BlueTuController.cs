using System;
using ToolScripts;
using UnityEngine;
using XianTu.UI;

namespace XianTu.Scripts.DataController
{
	internal class BlueTuController
	{
		public BlueTuController()
		{
			_oldData = BlueTuUIData.Instance.Clone();
			_data = BlueTuUIData.Instance;
			Init_OnUIOpen();
			_data.OnValueChange += OnUserChangeData;
			var blueTuUIData = _data;
			blueTuUIData.OnBuildBtn = (Action)Delegate.Combine(blueTuUIData.OnBuildBtn, new Action(OnUserBuildDetermine));
			var blueTuUIData2 = _data;
			blueTuUIData2.OnCopyBtn = (Action)Delegate.Combine(blueTuUIData2.OnCopyBtn, new Action(OnUserCopy));
			var blueTuUIData3 = _data;
			blueTuUIData3.OnResetBtn = (Action)Delegate.Combine(blueTuUIData3.OnResetBtn, new Action(OnReset));
			var blueprintData = BlueTuDatabase.Load("FoundationBlueTu");
			_foundation = blueprintData.buildings[0];
		}

		private void OnUserCopy()
		{
			_actionBuild.blueprintClipboard = _bPaste.blueprint;
			ResetBuildDuiDie();
			_bPaste.RefreshBlueprintUI();
		}

		private void Init_OnUIOpen()
		{
			UIManager.Instance.CanvasMonoEvent.onEnableEvent.AddListener(OnReset);
		}

		private void OnReset()
		{
			Player = GameMain.mainPlayer;
			var flag = Player == null;
			if (flag)
			{
				_data.Enable = false;
			}
			else
			{
				_playerController = Player.controller;
				_actionBuild = PlayerController.actionBuild;
				var flag2 = _actionBuild != null;
				if (flag2)
				{
					_activeTool = _actionBuild.activeTool;
					var buildToolBlueprintPaste = _activeTool as BuildTool_BlueprintPaste;
					var flag3 = buildToolBlueprintPaste != null;
					if (flag3)
					{
						_bPaste = buildToolBlueprintPaste;
						var mouseRay = _actionBuild.activeTool.mouseRay;
						_defaultMouseRay = new Ray(mouseRay.origin, mouseRay.direction);
						var flag4 = buildToolBlueprintPaste.blueprint != _blueprint;
						if (flag4)
						{
							_templateBlueTu = buildToolBlueprintPaste.blueprint.Clone();
							_blueprint = buildToolBlueprintPaste.blueprint;
						}
						_oldData = new BlueTuUIData();
						_data.Reset();
						_data.Enable = true;
						return;
					}
				}
				_data.Enable = false;
			}
		}

        private Player Player { get; set; }

		private void OnUserBuildDetermine()
		{
			var flag = _actionBuild.activeTool != null;
			if (flag)
			{
				var buildToolBlueprintPaste = _actionBuild.activeTool as BuildTool_BlueprintPaste;
				var flag2 = buildToolBlueprintPaste != null;
				if (flag2)
				{
					var flag3 = buildToolBlueprintPaste.CheckBuildConditionsPrestage();
					if (flag3)
					{
						ResetBuildDuiDie();
						Build(buildToolBlueprintPaste);
					}
				}
			}
		}

		private void ResetBuildDuiDie()
		{
			var buildToolBlueprintPaste = _bPaste;
			BlueprintBuilding blueprintBuilding = null;
			foreach (var blueprintBuilding2 in buildToolBlueprintPaste.blueprint.buildings)
			{
				var flag = Math.Abs(blueprintBuilding2.localOffset_z) < 1.5f;
				if (flag)
				{
					var itemProto = LDB.items.Select(blueprintBuilding2.itemId);
					Debug.Log($"基底查验:{itemProto.name}.{itemProto.ID}:{blueprintBuilding2.localOffset_x:2f}, {blueprintBuilding2.localOffset_y:2f}, {blueprintBuilding2.localOffset_z:2f}");
					blueprintBuilding = blueprintBuilding2;
					break;
				}
			}
			var flag2 = blueprintBuilding == null;
			if (flag2)
			{
				Debug.Log("没有基底");
			}
			var flag3 = blueprintBuilding == null;
			if (flag3)
			{
				var buildings2 = _bPaste.blueprint.buildings;
				var array = new BlueprintBuilding[_bPaste.blueprint.buildings.Length + 1];
				_bPaste.blueprint.buildings = array;
				buildings2.CopyTo(array, 0);
				blueprintBuilding = _foundation;
				blueprintBuilding.index = buildings2.Length;
				blueprintBuilding.localOffset_z = -0.5f;
				array[buildings2.Length] = blueprintBuilding;
			}
			foreach (var blueprintBuilding3 in _bPaste.blueprint.buildings)
			{
				var flag4 = blueprintBuilding3.IsBelt();
				if (!flag4)
				{
					var flag5 = blueprintBuilding3.IsSlot();
					if (!flag5)
					{
						var flag6 = blueprintBuilding3 == blueprintBuilding;
						if (!flag6)
						{
							blueprintBuilding3.inputToSlot = 14;
							blueprintBuilding3.outputFromSlot = 15;
							blueprintBuilding3.inputFromSlot = 15;
							blueprintBuilding3.outputToSlot = 14;
							blueprintBuilding3.inputObj = blueprintBuilding;
							blueprintBuilding3.inputFromSlot = -1;
						}
					}
				}
			}
			_bPaste.bpCursor = _bPaste.blueprint.buildings.Length;
			_bPaste.buildPreviews.Clear();
			_bPaste.ResetStates();
			_BuildTool_BluePrint_OnTick();
		}

		private void Build(BuildTool_BlueprintPaste bp)
		{
			PlayerController.cmd.stage = 1;
			bp.GenerateBlueprintGratBoxes();
			bp.DeterminePreviewsPrestage(true);
			bp.ActiveColliders(_actionBuild.model);
			bp.buildCondition = bp.CheckBuildConditions();
			bp.DeterminePreviews();
			bp.DeactiveColliders(_actionBuild.model);
			var buildCondition = bp.buildCondition;
			if (buildCondition)
			{
				bp.CreatePrebuilds();
				bp.ResetStates();
			}
			else
			{
				bp.isDragging = false;
				bp.startGroundPosSnapped = bp.castGroundPosSnapped;
				_BuildTool_BluePrint_OnTick();
			}
		}

		private void OnUserChangeData()
		{
			var flag = !_data.Enable;
			if (!flag)
			{
				var flag2 = PlayerController == null;
				if (!flag2)
				{
					var flag3 = _actionBuild == null;
					if (!flag3)
					{
						var flag4 = _bPaste == null;
						if (!flag4)
						{
							var vector = _data.Bias - _oldData.Bias;
                            var num = _data.LayerHeight - _oldData.LayerHeight;
							var num2 = _data.LayerNumber - _oldData.LayerNumber;
							var num3 = _data.Rotate - _oldData.Rotate;
							var flag5 = _actionBuild != null;
							if (flag5)
							{
								CtrlLayerNumber(num2);
								CtrlLayerHeight(num);
								CtrlRotate(num3);
								CtrlBiasData(vector);
								CtrlScale();
								_BuildTool_BluePrint_OnTick();
							}
							_oldData.Scale = _data.Scale;
							_oldData.Pivot = _data.Pivot;
							_oldData.LayerHeight = _data.LayerHeight;
							_oldData.LayerNumber = _data.LayerNumber;
							_oldData.Rotate = _data.Rotate;
							_oldData.Bias = _data.Bias;
						}
					}
				}
			}
		}

		private void CtrlLayerNumber(int bLayerNumber)
		{
			var flag = bLayerNumber == 0;
			if (!flag)
			{
				var num = _data.LayerNumber * _templateBlueTu.buildings.Length;
				var buildings = _bPaste.blueprint.buildings;
				var array = new BlueprintBuilding[num];
				var flag2 = buildings.Length > num;
				if (flag2)
				{
					Array.Copy(buildings, array, num);
				}
				else
				{
					buildings.CopyTo(array, 0);
					var num2 = _templateBlueTu.buildings.Length;
					for (var i = _bPaste.bpCursor; i < array.Length; i += num2)
					{
						_templateBlueTu.Clone().buildings.CopyTo(array, i);
					}
					for (var j = _bPaste.bpCursor; j < array.Length; j++)
					{
						array[j].localOffset_z = array[j - num2].localOffset_z + _data.LayerHeight;
						array[j].localOffset_z2 = array[j - num2].localOffset_z2 + _data.LayerHeight;
					}
					for (var k = _bPaste.bpCursor; k < array.Length; k++)
					{
						array[k].index = k;
					}
				}
				_bPaste.blueprint.buildings = array;
				_bPaste.bpCursor = _bPaste.blueprint.buildings.Length;
				_bPaste.buildPreviews.Clear();
				_bPaste.ResetStates();
			}
		}

		private void CtrlLayerHeight(float bLayerHeight)
		{
			var flag = bLayerHeight == 0f;
			if (!flag)
			{
				var num = _templateBlueTu.buildings.Length;
				var num2 = 0;
				var buildings = _bPaste.blueprint.buildings;
				for (var i = 0; i < _bPaste.bpCursor; i++)
				{
					var flag2 = i == num * (num2 + 1);
					if (flag2)
					{
						num2++;
					}
					buildings[i].localOffset_z += bLayerHeight * num2;
					buildings[i].localOffset_z2 += bLayerHeight * num2;
				}
			}
		}

		private void CtrlScale()
		{
			var flag = _oldData.Scale == _data.Scale;
			if (!flag)
			{
				var buildings = _templateBlueTu.buildings;
				var vector = _data.Scale - _oldData.Scale;
				var pivot = _data.Pivot;
				var num = 0;
				for (var i = 0; i < _bPaste.bpCursor; i++)
				{
					var flag2 = num == buildings.Length;
					if (flag2)
					{
						num = 0;
					}
					var blueprintBuilding = buildings[num];
					var blueprintBuilding2 = _bPaste.blueprint.buildings[i];
					blueprintBuilding2.localOffset_x += (blueprintBuilding.localOffset_x - pivot.x) * vector.x + pivot.x;
					blueprintBuilding2.localOffset_y += (blueprintBuilding.localOffset_y - pivot.y) * vector.y + pivot.y;
					blueprintBuilding2.localOffset_z += blueprintBuilding.localOffset_z * vector.z;
					blueprintBuilding2.localOffset_x2 += (blueprintBuilding.localOffset_x2 - pivot.x) * vector.x + pivot.x;
					blueprintBuilding2.localOffset_y2 += (blueprintBuilding.localOffset_y2 - pivot.y) * vector.y + pivot.y;
					blueprintBuilding2.localOffset_z2 += blueprintBuilding.localOffset_z2 * vector.z;
					num++;
				}
			}
		}

		private void CtrlRotate(float bRotate)
		{
			var flag = Math.Abs(bRotate) < 0.001f;
			if (!flag)
			{
				var buildToolBlueprintPaste = _actionBuild.activeTool as BuildTool_BlueprintPaste;
				var flag2 = buildToolBlueprintPaste != null;
				if (flag2)
				{
					var pivot = _data.Pivot;
					var buildings = buildToolBlueprintPaste.blueprint.buildings;
					var quaternion = Quaternion.AngleAxis(bRotate, Vector3.forward);
					for (var i = 0; i < _bPaste.bpCursor; i++)
					{
						var blueprintBuilding = buildings[i];
						var vector = new Vector3(blueprintBuilding.localOffset_x - pivot.x, blueprintBuilding.localOffset_y - pivot.y, 0f);
						vector = quaternion * vector;
						blueprintBuilding.localOffset_x = vector.x + pivot.x;
						blueprintBuilding.localOffset_y = vector.y + pivot.y;
						blueprintBuilding.yaw -= bRotate;
                        vector = new Vector3(blueprintBuilding.localOffset_x2 - pivot.x, blueprintBuilding.localOffset_y2 - pivot.y, 0f);
						vector = quaternion * vector;
						blueprintBuilding.localOffset_x2 = vector.x + pivot.x;
						blueprintBuilding.localOffset_y2 = vector.y + pivot.y;
						blueprintBuilding.yaw2 -= bRotate;
					}
				}
			}
		}

        private PlayerController PlayerController => _playerController;

		private void CtrlBiasData(Vector3 bBias)
		{
			var flag = bBias == Vector3.zero;
			if (!flag)
			{
				var playerActionBuild = _actionBuild;
				var buildTool = playerActionBuild.activeTool;
				var buildToolBlueprintPaste = buildTool as BuildTool_BlueprintPaste;
				var flag2 = buildToolBlueprintPaste != null;
				if (flag2)
				{
					for (var i = 0; i < buildToolBlueprintPaste.bpCursor; i++)
					{
						var blueprintBuilding = buildToolBlueprintPaste.blueprint.buildings[i];
						blueprintBuilding.localOffset_x += bBias.x;
						blueprintBuilding.localOffset_x2 += bBias.x;
						blueprintBuilding.localOffset_y += bBias.y;
						blueprintBuilding.localOffset_y2 += bBias.y;
						blueprintBuilding.localOffset_z += bBias.z;
						blueprintBuilding.localOffset_z2 += bBias.z;
					}
				}
			}
		}

		private void _BuildTool_BluePrint_OnTick()
		{
			VFInput.onGUI = false;
			_bPaste.mouseRay = _defaultMouseRay;
			var buildToolBlueprintPaste = _activeTool as BuildTool_BlueprintPaste;
			var flag = buildToolBlueprintPaste != null;
			if (flag)
			{
				buildToolBlueprintPaste.ClearErrorMessage();
				buildToolBlueprintPaste.UpdateRaycast();
				var flag2 = PlayerController.cmd.stage == 0;
				if (flag2)
				{
					buildToolBlueprintPaste.OperatingPrestage();
				}
				else
				{
					var flag3 = PlayerController.cmd.stage == 1;
					if (flag3)
					{
						buildToolBlueprintPaste.Operating();
					}
				}
				buildToolBlueprintPaste.UpdatePreviewModels(_actionBuild.model);
			}
		}

		private readonly BlueTuUIData _data;

		private BlueTuUIData _oldData;

		private PlayerController _playerController;

		private Ray _defaultMouseRay;

		private BlueprintData _templateBlueTu;

		private PlayerAction_Build _actionBuild;

		private BuildTool _activeTool;

		private BuildTool_BlueprintPaste _bPaste;

		private readonly BlueprintBuilding _foundation;

		private BlueprintData _blueprint;
	}
}
