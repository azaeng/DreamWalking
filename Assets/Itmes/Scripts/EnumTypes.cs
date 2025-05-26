using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum 타입 정의
public class EnumTypes : MonoBehaviour
{
    // 아이템 타입 (무기, 소비품)
    public enum ITEM_TYPE { WP, CB }

    // 무기 타입
    public enum WP_TYPE { MELEE, ARMOR }

    // 소비품 타입
    public enum CB_TYPE { S_HP_UP, M_HP_UP, L_HP_UP }
}
