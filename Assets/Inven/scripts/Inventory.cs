using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;

    [SerializeField] Transform draggablesTransform; // 드래그 중인 아이템의 부모 Transform
    [SerializeField] InventoryItem itemPrefab; // 인벤토리 아이템 프리팹
    [SerializeField] Item[] items; // 모든 아이템 ScriptableObject 배열
    [SerializeField] Button giveItemBtn; // 아이템 생성 디버그 버튼


    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    // 인벤토리 아이템을 생성하고 빈 슬롯에 배치
    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item;
        if (_item == null)
        {
            int random = Random.Range(0, items.Length);
            _item = items[random];
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem == null)
            {
                // 아이템 프리팹을 인스턴스화하고 초기화
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }


    void Update()
    {
        if (carriedItem == null)
        {
            ItemTooltipManager.Instance?.HideTooltip();
            return;
        }

        carriedItem.transform.position = Input.mousePosition;
        ItemTooltipManager.Instance?.ShowTooltip(carriedItem.myItem);
    }

    // 아이템을 "들고 있는 상태"로 만듦
    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        if (item.activeSlot.myTag != SlotTag.None)
        {
            EquipEquipment(item.activeSlot.myTag, null);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
        ItemTooltipManager.Instance?.ShowTooltip(carriedItem.myItem);
    }

    // 장비 장착 또는 해제
    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        switch (tag)
        {
            case SlotTag.Head:
                if (item == null)
                {
                    Debug.Log("Unequipped helmet on " + tag);
                }
                else
                {
                    Debug.Log("Equipped " + item.myItem.name + " on " + tag);
                }
                break;
            case SlotTag.Chest:
                break;
            case SlotTag.Legs:
                break;
            case SlotTag.Feet:
                break;
        }
    }
}