using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }

    public SlotTag myTag;

    // 마우스 클릭 이벤트가 발생함.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 현재 마우스에 들고 있는 아이템이 없다면 함수 종료
            if (Inventory.carriedItem == null) return;

            // 이 슬롯에 맞는 아이템이 아니라면 함수 종료
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return;

            // 현재 슬롯에 아이템을 등록
            SetItem(Inventory.carriedItem);
        }
    }

    // 인벤토리 아이템을 이 슬롯에 등록하는 메서드
    public void SetItem(InventoryItem item)
    {
        // 현재 들고 있는 아이템(carriedItem)을 null로 초기화하여 마우스에서 제거
        Inventory.carriedItem = null;

        // 이전에 있던 슬롯의 아이템 참조를 null로
        item.activeSlot.myItem = null;

        // 현재 슬롯의 myItem에 새 아이템 등록
        myItem = item;

        // 아이템의 현재 슬롯 정보를 이 슬롯(this)으로 갱신
        myItem.activeSlot = this;

        // 아이템의 부모를 현재 슬롯(transform)으로 설정하여 UI상 위치 이동
        myItem.transform.SetParent(transform);

        // 아이템의 CanvasGroup에서 raycast 차단을 다시 활성화
        // 그래야 슬롯에 들어간 아이템이 클릭 이벤트를 다시 받을 수 있음
        myItem.canvasGroup.blocksRaycasts = true;

        if (myTag != SlotTag.None)
        {
            // Inventory 인스턴스를 통해 장비 착용 처리 메서드 호출
            Inventory.Singleton.EquipEquipment(myTag, myItem);
        }
    }
}
