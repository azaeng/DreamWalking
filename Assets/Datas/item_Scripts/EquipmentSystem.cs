using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    // 근접 장착 아이템
    [SerializeField] private WeaponItem meleeWeaponItem;
    // 갑옷 장착 아이템
    [SerializeField] private WeaponItem amorWeaponItem;
    // 근접 무기 장착 위치
    [SerializeField] private Transform leftMeleeWeaponPosition;
    [SerializeField] private Transform rightMeleeWeaponPosition;
    // 근접 무기 게임 오브젝트 참조
    [SerializeField] private GameObject leftMeleeWeapon;
    [SerializeField] private GameObject rightMeleeWeapon;
    // 갑옷 장착 위치
    [SerializeField] private Transform armorWeaponPosition;
    // 갑옷 장비 게임 오브젝트 참조
    [SerializeField] private GameObject armorWeapon;

    // 시작 시 장착하고 있을 아이템 정보
    [SerializeField] private ItemInfo startWeaponItemInfo;

    public ItemInfo StartWeaponItemInfo { get => startWeaponItemInfo; set => startWeaponItemInfo = value; }

    // 인벤토리 시스템 컴포넌트
    // [SerializeField] private InventorySystem inventorySystem;

    // 장착된 무기의 데미지를 반환
    public int GetEquipWPDamage()
    {
        if (meleeWeaponItem != null)
        {
            return meleeWeaponItem.Damage;
        }

        return 0;
    }

    // 무기/갑옷 아이템 장착 처리
    public void EquipWeaponItem(WeaponItem weaponItem)
    {
        // 장착하려는 아이템이 무기 장비 아이템일 경우
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // 이전에 장착한 무기장비를 장착 해제 처리함
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

                // 장착 해제한 아이템의 장착 유무를 해제 상태로 설정
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"[{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} 무기가 장착이 해제됨");
            }

            // 새로운 무기 장비 아이템의 장착 유무를 장착상태로 설정
            meleeWeaponItem = weaponItem;
            meleeWeaponItem.IsEquip = true;

            // 해당 아이템의 실질적인 무기 게임 오브젝트를 지정한 무기 장착 위치에 생성함
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
            // 이전에 장착한 투구를 장착 해제 처리함
            if (amorWeaponItem != null)
            {
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 갑옷 장착이 해제됨");
            }

            // 새로 장착할 투구 아이템 설정
            amorWeaponItem = weaponItem;
            // 새로 장착할 헬멧 아이템을 장착 상태로 변경
            amorWeaponItem.IsEquip = true;
            // 투구 게임 오브젝트 생성 후 플레이어의 머리에 장착
            armorWeapon = Instantiate(amorWeaponItem.WpPrefabs[0], armorWeaponPosition);

            Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 갑옷을 {amorWeaponItem.EquipParentTag} 위치에 장착 됨");
        }
    }

    // 장비 장착을 해제함 (해제할 장착 아이템)
    public void UnEquipWeaponItem(WeaponItem weaponItem)
    {
        // 장착하려는 아이템이 무기 장착 아이템일 경우
        if (weaponItem.WpType == EnumTypes.WP_TYPE.MELEE)
        {
            // 캐릭터가 장착한 무기 아이템이 존재한다면
            if (meleeWeaponItem != null)
            {
                // 장착된 무기 장비를 파괴함 (장착 해제)
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

                // 장착된 아이템을 장착 해제 상태로 변경함
                meleeWeaponItem.IsEquip = false;
                Debug.Log($"장착된 [{meleeWeaponItem.ItemId}] {meleeWeaponItem.ItemName} 무기의 장착이 해제됨");
                meleeWeaponItem = null; // 장착한 무기 아이템이 없음으로 설정함
            }
        }

        else
        {
            // 캐릭터가 장착한 갑옷(헬멧) 아이템이 존재한다면
            if (armorWeapon != null)
            {
                // 장착된 헬멧을 파괴함 (장착 해제)
                if (armorWeapon != null)
                {
                    Destroy(armorWeapon);
                    armorWeapon = null;
                }

                // 현재 아이템을 장착 해제 상태로 변경함
                amorWeaponItem.IsEquip = false;
                Debug.Log($"[{amorWeaponItem.ItemId}] {amorWeaponItem.ItemName} 갑옷 장착이 해제됨");
                amorWeaponItem = null; // 장착된 갑옷 아이템이 없음으로 설정함
            }
        }
    }
}
