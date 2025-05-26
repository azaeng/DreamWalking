using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 정보를 담는 구조체
[Serializable]
public struct ItemInfo
{
    // 아이템 타입 (예: 무기, 방어구 등)
    public EnumTypes.ITEM_TYPE ItemType;

    // 아이템 고유 ID
    public int ItemId;
}