using System.Linq;
using CruiseAssist.Commons;
using CruiseAssist.Enums;
using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(PlayerMove_Walk))]
internal class Patch_PlayerMoveWalk
{
    [HarmonyPatch("GameTick")]
    [HarmonyPrefix]
    public static void GameTick_Prefix(PlayerMove_Walk __instance)
    {
        CruiseAssistPlugin.State = CruiseAssistState.Inactive;
        CruiseAssistPlugin.Interrupt = false;
        CruiseAssistPlugin.TargetStar = null;
        CruiseAssistPlugin.TargetPlanet = null;
        CruiseAssistPlugin.TargetUPos = VectorLF3.zero;
        CruiseAssistPlugin.TargetRange = 0.0;
        CruiseAssistPlugin.TargetSelected = false;
        if (GameMain.localPlanet != null)
        {
            if (CruiseAssistPlugin.History.Count == 0 || CruiseAssistPlugin.History.Last() != GameMain.localPlanet.id)
            {
                if (CruiseAssistPlugin.History.Count >= 128)
                {
                    CruiseAssistPlugin.History.RemoveAt(0);
                }
                CruiseAssistPlugin.History.Add(GameMain.localPlanet.id);
                ConfigManager.CheckConfig(ConfigManager.Step.State);
            }
        }

        if (!CruiseAssistPlugin.Enable) return;

        var indicatorAstroId = GameMain.mainPlayer.navigation.indicatorAstroId;
        if (indicatorAstroId != 0 && CruiseAssistPlugin.SelectTargetAstroId != indicatorAstroId)
        {
            CruiseAssistPlugin.SelectTargetAstroId = indicatorAstroId;
            if (indicatorAstroId % 100 != 0)
            {
                CruiseAssistPlugin.SelectTargetPlanet = GameMain.galaxy.PlanetById(indicatorAstroId);
                CruiseAssistPlugin.SelectTargetStar = CruiseAssistPlugin.SelectTargetPlanet.star;
            }
            else
            {
                CruiseAssistPlugin.SelectTargetPlanet = null;
                CruiseAssistPlugin.SelectTargetStar = GameMain.galaxy.StarById(indicatorAstroId / 100);
            }
            CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
            {
                extension.SetTargetAstroId(indicatorAstroId);
            });
        }

        if (CruiseAssistPlugin.SelectTargetStar != null)
        {
            if (GameMain.localStar != null && CruiseAssistPlugin.SelectTargetStar.id == GameMain.localStar.id)
            {
                if (CruiseAssistPlugin.SelectTargetPlanet != null)
                {
                    if (GameMain.localPlanet != null && CruiseAssistPlugin.SelectTargetPlanet.id == GameMain.localPlanet.id)
                    {
                        CruiseAssistPlugin.SelectTargetStar = null;
                        CruiseAssistPlugin.SelectTargetPlanet = null;
                        CruiseAssistPlugin.SelectTargetAstroId = 0;
                        GameMain.mainPlayer.navigation.indicatorAstroId = 0;
                        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
                        {
                            extension.SetInactive();
                        });
                        return;
                    }
                    CruiseAssistPlugin.TargetPlanet = CruiseAssistPlugin.SelectTargetPlanet;
                }
                else
                {
                    if (GameMain.localPlanet != null)
                    {
                        CruiseAssistPlugin.SelectTargetStar = null;
                        CruiseAssistPlugin.SelectTargetAstroId = 0;
                        GameMain.mainPlayer.navigation.indicatorAstroId = 0;
                        CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
                        {
                            extension.SetInactive();
                        });
                        return;
                    }
                    if (CruiseAssistPlugin.ReticuleTargetPlanet != null)
                    {
                        CruiseAssistPlugin.TargetPlanet = CruiseAssistPlugin.ReticuleTargetPlanet;
                    }
                }
            }
            else
            {
                CruiseAssistPlugin.TargetStar = CruiseAssistPlugin.SelectTargetStar;
            }
        }
        else
        {
            if (CruiseAssistPlugin.ReticuleTargetPlanet != null)
            {
                CruiseAssistPlugin.TargetPlanet = CruiseAssistPlugin.ReticuleTargetPlanet;
            }
            else if (CruiseAssistPlugin.ReticuleTargetStar != null)
            {
                CruiseAssistPlugin.TargetStar = CruiseAssistPlugin.ReticuleTargetStar;
            }
        }

        if (CruiseAssistPlugin.TargetPlanet != null)
        {
            CruiseAssistPlugin.State = CruiseAssistState.ToPlanet;
            CruiseAssistPlugin.TargetStar = CruiseAssistPlugin.TargetPlanet.star;
            CruiseAssistPlugin.TargetRange = (CruiseAssistPlugin.TargetPlanet.uPosition - GameMain.mainPlayer.uPosition).magnitude - CruiseAssistPlugin.TargetPlanet.realRadius;
            CruiseAssistPlugin.TargetSelected = true;
        }
        else
        {
            if (CruiseAssistPlugin.TargetStar == null)
            {
                CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
                {
                    extension.SetInactive();
                });
                return;
            }
            CruiseAssistPlugin.State = CruiseAssistState.ToStar;
            CruiseAssistPlugin.TargetRange = (CruiseAssistPlugin.TargetStar.uPosition - GameMain.mainPlayer.uPosition).magnitude - (CruiseAssistPlugin.TargetStar.viewRadius - 120f);
            CruiseAssistPlugin.TargetSelected = true;
        }
        var flag18 = __instance.controller.movementStateInFrame > EMovementState.Walk;
        if (flag18) return;
        if (VFInput._jump.pressing)
        {
            CruiseAssistPlugin.Interrupt = true;
            CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
            {
                extension.CancelOperate();
            });
        }
        else
        {
            CruiseAssistPlugin.Extensions.ForEach(delegate(ICruiseAssistExtensionAPI extension)
            {
                extension.OperateWalk(__instance);
            });
        }
    }
}