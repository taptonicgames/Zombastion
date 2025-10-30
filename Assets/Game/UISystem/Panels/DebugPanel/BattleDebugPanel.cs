using UnityEngine;

public class BattleDebugPanel : AbstractPanel
{
    public override PanelType Type => PanelType.BattleDebugPanel;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Win });
        else if (Input.GetKeyDown(KeyCode.LeftShift))
            EventBus<RoundCompleteEvnt>.Publish(new() { type = RoundCompleteType.Fail });
#endif
    }
}