using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 스크립터블 오브젝트
public class Item : ScriptableObject
{
    // 아이템 타입
    [SerializeField] private EnumTypes.ITEM_TYPE itemType;
    // 아이템 아이디
    [SerializeField] protected int itemId;
    // 아이템 이름
    [SerializeField] protected string itemName;
    // 아이템 설명
    [SerializeField] protected string itemDescription;
    // 아이템 아이콘 이미지
    [SerializeField] protected Sprite itemIconIamge;
    // 아이템 가격
    [SerializeField] protected int itemPrice;
    // 아이템 수량
    [SerializeField] private int itemCount;
    // 아이템 장착 여부
    [SerializeField] private bool isEquip;

    public EnumTypes.ITEM_TYPE ItemType { get => itemType; set => itemType = value; }
    public int ItemCount { get => itemCount; set => itemCount = value; }
    public bool IsEquip { get => isEquip; set => isEquip = value; }
    public int ItemId { get => itemId; set => itemId = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public string ItemDescription { get => itemDescription; set => itemDescription = value; }
    public Sprite ItemIconIamge { get => itemIconIamge; set => itemIconIamge = value; }
    public int ItemPrice { get => itemPrice; set => itemPrice = value; }

    // 아이템 복사
    public Item Clone()
    {
        Item newItem = Instantiate(this);
        return newItem;
    }
}
