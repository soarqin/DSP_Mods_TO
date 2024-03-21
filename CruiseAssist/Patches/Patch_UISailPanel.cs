using System.Collections.Generic;
using System.Reflection.Emit;
using CruiseAssist.Commons;
using HarmonyLib;

namespace CruiseAssist.Patches;

[HarmonyPatch(typeof(UISailPanel))]
internal class Patch_UISailPanel
{
    [HarmonyPatch("_OnUpdate")]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> OnUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codeMatcher = new CodeMatcher(instructions);
        codeMatcher.MatchForward(true,
            new CodeMatch(OpCodes.Ldarg_0)
        ).InsertAndAdvance(
            Transpilers.EmitDelegate(delegate()
            {
                CruiseAssistPlugin.ReticuleTargetPlanet = null;
                CruiseAssistPlugin.ReticuleTargetStar = null;
            })
        );
        codeMatcher.MatchForward(true,
            new CodeMatch(OpCodes.Bge_Un),
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Stloc_S),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Stloc_S),
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Stloc_S)
        ).Advance(1).InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldloc_0),
            new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(StarData), "planets")),
            new CodeInstruction(OpCodes.Ldloc_S, 21),
            Transpilers.EmitDelegate(delegate(PlanetData[] planets, int planetIndex)
            {
                CruiseAssistPlugin.ReticuleTargetPlanet = planets[planetIndex];
            })
        );
        codeMatcher.MatchForward(true, 
            new CodeMatch(OpCodes.Bge_Un),
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Stloc_S),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Stloc_S),
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Stloc_S)
        ).Advance(1).InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldloc_S, 20),
            new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(GalaxyData), "stars")),
            new CodeInstruction(OpCodes.Ldloc_S, 24),
            Transpilers.EmitDelegate(delegate(StarData[] stars, int starIndex)
            {
                CruiseAssistPlugin.ReticuleTargetStar = stars[starIndex];
            })
        );
        return codeMatcher.InstructionEnumeration();
    }

    [HarmonyPatch("_OnOpen")]
    [HarmonyPrefix]
    public static void OnOpen_Prefix()
    {
        if (CruiseAssistPlugin.Conf.AutoDisableLockCursorFlag)
        {
            UIRoot.instance.uiGame.disableLockCursor = true;
        }
    }

    [HarmonyPatch("_OnClose")]
    [HarmonyPrefix]
    public static void OnClose_Prefix()
    {
        ConfigManager.CheckConfig(ConfigManager.Step.State);
    }
}