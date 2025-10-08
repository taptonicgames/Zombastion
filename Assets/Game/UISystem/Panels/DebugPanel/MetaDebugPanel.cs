using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MetaDebugPanel : AbstractPanel
{
    [SerializeField] private Button addInsertButton;
    [SerializeField] private Button addClothButton;

    [Inject] private SharedObjects sharedObjects;
    [Inject] private EquipmentSO clothItemSO;

    public override PanelType Type => PanelType.MetaDebugPanel;

    public override void Init()
    {
        
    }
}