using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 정보 구조체
[Serializable]
public struct ItemInfo
{
    // 아이템 타입
    public EnumTypes.ITEM_TYPE ItemType;

    // 아이템 아이디
    public int ItemId;
}
