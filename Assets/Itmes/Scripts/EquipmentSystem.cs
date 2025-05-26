using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    // 현재 장착 무기 아이템
    [SerializeField] private WeaponItem meleeWeaponItem;
    // 현재 장착 방어구 아이템
    [SerializeField] private WeaponItem amorWeaponItem;
    // 근접 무기 장착 위치
    [SerializeField] private Transform leftMeleeWeaponPosition;
    [SerializeField] private Transform rightMeleeWeaponPosition;
    // 장착된 근접 무기 게임오브젝트 참조
    [SerializeField] private GameObject leftMeleeWeapon;
    [SerializeField] private GameObject rightMeleeWeapon;
    // 방어구 장착 위치
    [SerializeField] private Transform armorWeaponPosition;
    // 장착된 방어구 게임오브젝트 참조
    [SerializeField] private GameObject armorWeapon;

    // 시작 시 장착할 무기 아이템 정보
    [SerializeField] private ItemInfo startWeaponItemInfo;

    public ItemInfo StartWeaponItemInfo { get => startWeaponItemInfo; set => startWeaponItemInfo = value; }

    // 인벤토리 시스템 참조
    // [SerializeField] private InventorySystem inventorySystem;

    // 현재 장착 무기의 공격력을 반환
    public int GetEquipWPDamage()
    {
        if (meleeWeaponItem != null)
        {
            return meleeWeaponItem.Damage;
        }

        return 0;
    }

    // 무기/방어구 아이템 장착 처리
    public void EquipWeaponItem(WeaponItem weaponItem)
    {
        // 근접무기 타입일 경우
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // 기존 장착된 무기가 있다면 먼저 제거 처리
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

                // 기존 무기의 장착 상태를 해제
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"[{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} 무기가 장착 해제됨");
            }

            // 새로운 무기를 현재 무기로 설정하고 장착 상태로 변경
            meleeWeaponItem = weaponItem;
            meleeWeaponItem.IsEquip = true;

            // 해당 무기의 프리팹을 손 위치에 인스턴스화하여 장착
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
            // 기존 장착된 방어구가 있다면 제거 처리
            if (amorWeaponItem != null)
            {
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 방어구가 해제됨");
            }

            // 새로운 방어구를 현재 방어구로 설정
            amorWeaponItem = weaponItem;
            // 새로운 방어구의 장착 상태를 true로 설정
            amorWeaponItem.IsEquip = true;
            // 방어구 프리팹을 생성해 플레이어의 위치에 장착
            armorWeapon = Instantiate(amorWeaponItem.WpPrefabs[0], armorWeaponPosition);


            Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 방어구가 {amorWeaponItem.EquipParentTag} 위치에 장착됨");
        }
    }

    // 무기 장착 해제 (인벤토리로 되돌리기)
    public void UnEquipWeaponItem(WeaponItem weaponItem)
    {
        // 근접 무기일 경우
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // 캐릭터가 근접 무기를 장착 중이라면
            if (meleeWeaponItem != null)
            {
                // 장착된 무기 제거 (양손)
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

                // 무기 장착 상태 해제
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"무기 [{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} 장착이 해제됨");
                meleeWeaponItem = null; // 현재 장착된 무기를 장착 해제한다
            }
        }

        else
        {
            // 캐릭터가 방어구를 장착 중이라면
            if (armorWeapon != null)
            {
                // 장착된 방어구 제거
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                // 방어구 장착 상태 해제
                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 방어구가 해제됨");
                amorWeaponItem = null; // 현재 장착 중인 장비를 해제한다
            }
        }
    }
}
