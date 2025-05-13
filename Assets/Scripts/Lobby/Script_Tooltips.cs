using UnityEngine;
using UnityEngine.EventSystems;

public class SpecificTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel_1, tooltipPanel_2, tooltipPanel_3; // 툴팁으로 사용할 Panel
    public RectTransform targetUI_1, targetUI_2, targetUI_3; // 특정 UI 요소

    private GameObject activeTooltipPanel = null; // 현재 활성화된 Panel

    void Start()
    {
        // 모든 툴팁 Panel 초기 상태 비활성화
        if (tooltipPanel_1 != null) tooltipPanel_1.SetActive(false);
        if (tooltipPanel_2 != null) tooltipPanel_2.SetActive(false);
        if (tooltipPanel_3 != null) tooltipPanel_3.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsPointerOverTarget(eventData, targetUI_1))
        {
            ActivateTooltip(tooltipPanel_1); // 첫 번째 UI에 마우스가 닿았을 때 첫 번째 Panel 활성화
        }
        else if (IsPointerOverTarget(eventData, targetUI_2))
        {
            ActivateTooltip(tooltipPanel_2); // 두 번째 UI에 마우스가 닿았을 때 두 번째 Panel 활성화
        }
        else if (IsPointerOverTarget(eventData, targetUI_3))
        {
            ActivateTooltip(tooltipPanel_3); // 세 번째 UI에 마우스가 닿았을 때 세 번째 Panel 활성화
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activeTooltipPanel != null)
        {
            activeTooltipPanel.SetActive(false); // 현재 활성화된 Panel 비활성화
            activeTooltipPanel = null; // 활성화 상태 초기화
        }
    }

    private bool IsPointerOverTarget(PointerEventData eventData, RectTransform targetUI)
    {
        // 특정 UI 요소에 마우스가 닿았는지 확인
        return RectTransformUtility.RectangleContainsScreenPoint(targetUI, Input.mousePosition, eventData.pressEventCamera);
    }

    private void ActivateTooltip(GameObject tooltipPanel)
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(true); // 툴팁 활성화
            activeTooltipPanel = tooltipPanel; // 현재 활성화된 Panel 저장
        }
    }
}
