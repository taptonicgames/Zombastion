using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    [SerializeField] private BattleUpgradeConfigsPack battleUpgradeConfigsPack;
    [SerializeField] private Button testButton;
    [SerializeField] private CardsPanel cardsPanel;

    private BattleUpgradeHandler battleUpgradeHandler;
    private BattleUpgradeStorage battleUpgradeStorage;

    private void Awake()
    {
        testButton.onClick.AddListener(OnTestButtonClicked);
    }

    private void Start()
    {
        battleUpgradeStorage = new BattleUpgradeStorage(battleUpgradeConfigsPack);
        battleUpgradeHandler = new BattleUpgradeHandler(battleUpgradeStorage);
        cardsPanel.Init(battleUpgradeConfigsPack, battleUpgradeHandler, battleUpgradeStorage);
        cardsPanel.Hide();
    }

    private void OnTestButtonClicked()
    {
        cardsPanel.Show();
    }

    private void OnDestroy()
    {
        testButton.onClick.RemoveListener(OnTestButtonClicked);
    }
}
