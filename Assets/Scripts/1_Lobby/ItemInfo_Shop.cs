using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemInfo_Shop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string descriptionText;
    [SerializeField] private TextMeshProUGUI descriptionDisplay;

    public void OnPointerEnter(PointerEventData eventData) // 상점의 아이템에 마우스 커서를 올리면 UnderBar의 itemInfo에 아이템정보 출력
    {
        if (descriptionDisplay != null)
            descriptionDisplay.text = descriptionText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionDisplay != null)
            descriptionDisplay.text = ""; // 마우스 커서를 치우면 공백 출력
    }
}
