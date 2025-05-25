using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����(��������) ������ ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class WeaponItem : Item
{
    // ���� ����
    [SerializeField] private EnumTypes.WP_TYPE wpType;
    // ���� �ӵ�
    [SerializeField] protected float attackSpeed;
    // ���� ������
    [SerializeField] protected int damage;
    // ���� ���� �θ� �±�
    [SerializeField] protected string equipParentTag;
    // ���� ������
    [SerializeField] private GameObject[] wpPrefabs;

    public EnumTypes.WP_TYPE WpType { get => wpType; set => wpType = value; }
    public GameObject[] WpPrefabs { get => wpPrefabs; set => wpPrefabs = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Damage { get => damage; set => damage = value; }
    public string EquipParentTag { get => equipParentTag; set => equipParentTag = value; }
}
