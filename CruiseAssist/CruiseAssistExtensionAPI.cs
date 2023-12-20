namespace CruiseAssist;

public interface ICruiseAssistExtensionAPI
{
    void CheckConfig(string step);

    void SetTargetAstroId(int astroId);

    bool OperateWalk(PlayerMove_Walk __instance);

    bool OperateDrift(PlayerMove_Drift __instance);

    bool OperateFly(PlayerMove_Fly __instance);

    bool OperateSail(PlayerMove_Sail __instance);

    void SetInactive();

    void CancelOperate();

    void OnGUI();
}