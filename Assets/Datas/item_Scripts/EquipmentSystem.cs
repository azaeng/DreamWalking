using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    // ���� ���� ������
    [SerializeField] private WeaponItem meleeWeaponItem;
    // ���� ���� ������
    [SerializeField] private WeaponItem amorWeaponItem;
    // ���� ���� ���� ��ġ
    [SerializeField] private Transform leftMeleeWeaponPosition;
    [SerializeField] private Transform rightMeleeWeaponPosition;
    // ���� ���� ���� ������Ʈ ����
    [SerializeField] private GameObject leftMeleeWeapon;
    [SerializeField] private GameObject rightMeleeWeapon;
    // ���� ���� ��ġ
    [SerializeField] private Transform armorWeaponPosition;
    // ���� ��� ���� ������Ʈ ����
    [SerializeField] private GameObject armorWeapon;

    // ���� �� �����ϰ� ���� ������ ����
    [SerializeField] private ItemInfo startWeaponItemInfo;

    public ItemInfo StartWeaponItemInfo { get => startWeaponItemInfo; set => startWeaponItemInfo = value; }

    // �κ��丮 �ý��� ������Ʈ
    // [SerializeField] private InventorySystem inventorySystem;

    // ������ ������ �������� ��ȯ
    public int GetEquipWPDamage()
    {
        if (meleeWeaponItem != null)
        {
            return meleeWeaponItem.Damage;
        }

        return 0;
    }

    // ����/���� ������ ���� ó��
    public void EquipWeaponItem(WeaponItem weaponItem)
    {
        // �����Ϸ��� �������� ���� ��� �������� ���
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // ������ ������ ������� ���� ���� ó����
            if (meleeWeaponItem != null)
            {
                if (leftMeleeWeapon != null)
                {
                    Destroy(leftMeleeWeapon);
                    leftMeleeWeapon = null;
                }

                if (rightMeleeWeapon != null)
                {
                    Destroy(rightMeleeWeapon);
                    rightMeleeWeapon = null;
                }

                // ���� ������ �������� ���� ������ ���� ���·� ����
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"[{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} ���Ⱑ ������ ������");
            }

            // ���ο� ���� ��� �������� ���� ������ �������·� ����
            meleeWeaponItem = weaponItem;
            meleeWeaponItem.IsEquip = true;

            // �ش� �������� �������� ���� ���� ������Ʈ�� ������ ���� ���� ��ġ�� ������
            if (meleeWeaponItem.WpPrefabs.Length > 1)
            {
                leftMeleeWeapon = Instantiate(meleeWeaponItem.WpPrefabs[0], leftMeleeWeaponPosition);
                rightMeleeWeapon = Instantiate(meleeWeaponItem.WpPrefabs[1], rightMeleeWeaponPosition);
            }

            else
            {
                rightMeleeWeapon = Instantiate(meleeWeaponItem.WpPrefabs[0], rightMeleeWeaponPosition);
            }
        }
        else
        {
            // ������ ������ ������ ���� ���� ó����
            if (amorWeaponItem != null)
            {
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} ���� ������ ������");
            }

            // ���� ������ ���� ������ ����
            amorWeaponItem = weaponItem;
            // ���� ������ ��� �������� ���� ���·� ����
            amorWeaponItem.IsEquip = true;
            // ���� ���� ������Ʈ ���� �� �÷��̾��� �Ӹ��� ����
            armorWeapon = Instantiate(amorWeaponItem.WpPrefabs[0], armorWeaponPosition);

            Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} ������ {amorWeaponItem.EquipParentTag} ��ġ�� ���� ��");
        }
    }

    // ��� ������ ������ (������ ���� ������)
    public void UnEquipWeaponItem(WeaponItem weaponItem)
    {
        // �����Ϸ��� �������� ���� ���� �������� ���
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // ĳ���Ͱ� ������ ���� �������� �����Ѵٸ�
            if (meleeWeaponItem != null)
            {
                // ������ ���� ��� �ı��� (���� ����)
                if (leftMeleeWeapon != null)
                {
                    Destroy(leftMeleeWeapon);
                    leftMeleeWeapon = null;
                }

                if (rightMeleeWeapon != null)
                {
                    Destroy(rightMeleeWeapon);
                    rightMeleeWeapon = null;
                }

                // ������ �������� ���� ���� ���·� ������
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"������ [{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} ������ ������ ������");
                meleeWeaponItem = null; // ������ ���� �������� �������� ������
            }
        }

        else
        {
            // ĳ���Ͱ� ������ ����(���) �������� �����Ѵٸ�
            if (armorWeapon != null)
            {
                // ������ ����� �ı��� (���� ����)
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                // ���� �������� ���� ���� ���·� ������
                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} ���� ������ ������");
                amorWeaponItem = null; // ������ ���� �������� �������� ������
            }
        }
    }
}
