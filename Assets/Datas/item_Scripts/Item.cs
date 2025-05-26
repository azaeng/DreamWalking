using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ũ���ͺ� ������Ʈ
public class Item : ScriptableObject
{
    // ������ Ÿ��
    [SerializeField] private EnumTypes.ITEM_TYPE itemType;
    // ������ ���̵�
    [SerializeField] protected int itemId;
    // ������ �̸�
    [SerializeField] protected string itemName;
    // ������ ����
    [SerializeField] protected string itemDescription;
    // ������ ������ �̹���
    [SerializeField] protected Sprite itemIconIamge;
    // ������ ����
    [SerializeField] protected int itemPrice;
    // ������ ����
    [SerializeField] private int itemCount;
    // ������ ����
    [SerializeField] private bool isEquip;

    public EnumTypes.ITEM_TYPE ItemType { get => itemType; set => itemType = value; }
    public int ItemCount { get => itemCount; set => itemCount = value; }
    public bool IsEquip { get => isEquip; set => isEquip = value; }
    public int ItemId { get => itemId; set => itemId = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public Sprite ItemIconIamge { get => itemIconIamge; set => itemIconIamge = value; }
    public int ItemPrice { get => itemPrice; set => itemPrice = value; }

    // ������ ����
    public Item Clone()
    {
        Item newItem = Instantiate(this);
        return newItem;
    }
}
