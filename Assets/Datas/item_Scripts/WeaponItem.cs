using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기(근접무기) 아이템 스크립터블 오브젝트
[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class WeaponItem : Item
{
    // 무기 종류
    [SerializeField] private EnumTypes.WP_TYPE wpType;
    // 공격 속도
    [SerializeField] protected float attackSpeed;
    // 공격 데미지
    [SerializeField] protected int damage;
    // 무기 장착 부모 태그
    [SerializeField] protected string equipParentTag;
    // 무기 프리팹
    [SerializeField] private GameObject[] wpPrefabs;

    public EnumTypes.WP_TYPE WpType { get => wpType; set => wpType = value; }
    public GameObject[] WpPrefabs { get => wpPrefabs; set => wpPrefabs = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public string EquipParentTag { get => equipParentTag; set => equipParentTag = value; }
}
