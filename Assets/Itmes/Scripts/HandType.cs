using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandType : MonoBehaviour
{
    // 손 타입
    public enum HAND_TYPE { LEFT, RIGHT };

    [SerializeField] private MeleeWeapon[] meleeWeapon;
}
