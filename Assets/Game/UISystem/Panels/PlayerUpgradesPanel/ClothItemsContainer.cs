using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClothItemsContainer : MonoBehaviour
{
    [SerializeField] private Vector2 portretOrientationResolution;
    [SerializeField] private Vector2 albomOrientationResolution;

    private ClothItemView[] items;
    private GridLayoutGroup gridLayoutGroup;

    public event Action<ClothItemView> ClothItemClicked;

    public void Init(EquipmentManager equipmentManager)
    {
        List<EquipmentData> datas = equipmentManager.GetEquipmentDatas();

        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        items = GetComponentsInChildren<ClothItemView>();

        gridLayoutGroup.cellSize = ScreenExtension.IsPortretOrientation ? portretOrientationResolution : albomOrientationResolution;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].gameObject.SetActive(i < datas.Count);

            if (items[i].gameObject.activeSelf)
            {
                items[i].Init(datas[i]);
                items[i].ItemClicked += OnItemClicked;
            }
        }
    }

    public ClothItemView GetClothItem(EquipmentType type)
    {
        return items.First(i => i.EquipmentData.Type == type);
    }

    public InsertItemView GetEmptyInsertSlot(EquipmentType type)
    {
        ClothItemView clothItemView = items.First(i => i.EquipmentData.Type == type);
        return clothItemView.GetEmptyInsertSlot();
    }

    public void ClearInsertSlot(InsertData data)
    {
        ClothItemView clothItemView = items.First(i => i.EquipmentData.Type == data.Type);
        clothItemView.UpdateDatas();
    }

    private void OnItemClicked(ClothItemView item)
    {
        ClothItemClicked?.Invoke(item);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].ItemClicked -= OnItemClicked;
    }

    #region Debug
#if UNITY_EDITOR
    private bool isPortretOrientation;

    private void Update()
    {
        if (isPortretOrientation != ScreenExtension.IsPortretOrientation)
        {
            isPortretOrientation = ScreenExtension.IsPortretOrientation;
            gridLayoutGroup.cellSize = ScreenExtension.IsPortretOrientation ? portretOrientationResolution : albomOrientationResolution;
        }
    }
#endif
    #endregion
}