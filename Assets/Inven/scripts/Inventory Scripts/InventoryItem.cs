using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    // 인벤토리 아이템 이미지를 표시하는 Image 컴포넌트
    Image itemIcon;

    // CanvasGroup은 UI 상호작용을 제어하기 위한 컴포넌트 (Raycast 등)
    public CanvasGroup canvasGroup { get; private set; }

    // 이 인벤토리 아이템이 참조하는 실제 아이템 데이터
    public Item myItem { get; set; }

    // 현재 이 아이템이 위치한 슬롯
    public InventorySlot activeSlot { get; set; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    // InventoryItem을 초기화하는 메서드
    // 어떤 Item 데이터와 어떤 InventorySlot에 속하는지를 설정함
    public void Initialize(Item item, InventorySlot parent)
    {
        // 아이템이 들어갈 슬롯 설정
        activeSlot = parent;

        // 슬롯의 myItem에 현재 이 인벤토리 아이템 등록
        activeSlot.myItem = this;

        // 실제 아이템 데이터 저장
        myItem = item;

        // 아이콘 이미지 설정
        itemIcon.sprite = item.sprite;
    }

    // 마우스 클릭 이벤트 발생 시 호출되는 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 왼쪽 클릭 시 현재 아이템을 Inventory에서 마우스로 들게 설정
            Inventory.Singleton.SetCarriedItem(this);
        }
    }
}
