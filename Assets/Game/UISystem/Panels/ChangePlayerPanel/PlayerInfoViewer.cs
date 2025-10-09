using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoViewer : MonoBehaviour
{
    [SerializeField] private Transform unlockStateObject;
    [SerializeField] private Transform lockStateObject;

    [Space(10)]
    [SerializeField] private TMP_Text tittleText;
    [SerializeField] private TMP_Text subTittleText;
    [SerializeField] private TMP_Text description;

    [Space(10)]
    [SerializeField] private TMP_Text firstBustDescription;
    [SerializeField] private TMP_Text firstBustValue;
    [SerializeField] private TMP_Text secondBustDescription;
    [SerializeField] private TMP_Text secondBustValue;
    [SerializeField] private Image skillIcon;

    internal void UpdateInfo(PlayerCosmeticSO.PlayerCosmeticData data)
    {
        tittleText.SetText(data.Tittle);
        subTittleText.SetText(data.SubTittle);
        description.SetText(data.Descriprion);

        firstBustDescription.SetText(data.FirstBustDescription);
        firstBustValue.SetText($"+{data.FirstBustValue}%");

        secondBustDescription.SetText(data.SecondBustDescription);
        secondBustValue.SetText($"+{data.SecondBustValue}%");

        skillIcon.sprite = data.SkillIcon;

        //TODO: implement save lcok/unlock state
        unlockStateObject.gameObject.SetActive(data.Descriprion != "");
        lockStateObject.gameObject.SetActive(data.Descriprion == "");
    }
}
