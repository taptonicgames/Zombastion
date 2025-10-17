using TMPro;
using UnityEngine;

public class TowerStatItemView : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text valueText;

    public void Init(string description, string valueText)
    {
        this.description.SetText(description);
        this.valueText.SetText(valueText);
    }
}
