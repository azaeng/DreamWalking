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

    [SerializeField] Transform draggablesTransform; // �巡�� ���� �������� �θ� Transform
    [SerializeField] InventoryItem itemPrefab; // �κ��丮 ������ ������
    [SerializeField] Item[] items; // ��� ������ ScriptableObject �迭
    [SerializeField] Button giveItemBtn; // ������ ���� ����� ��ư


    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    // �κ��丮 �������� �����ϰ� �� ���Կ� ��ġ
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
                // ������ �������� �ν��Ͻ�ȭ�ϰ� �ʱ�ȭ
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

    // �������� "��� �ִ� ����"�� ����
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

    // ��� ���� �Ǵ� ����
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