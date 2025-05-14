using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public GameObject tooltipPanel_1, tooltipPanel_2, tooltipPanel_3;
    public RectTransform targetUI_1, targetUI_2, targetUI_3;

    private GameObject activeTooltipPanel = null;

    void Start()
    {
        if (tooltipPanel_1 != null) tooltipPanel_1.SetActive(false);
        if (tooltipPanel_2 != null) tooltipPanel_2.SetActive(false);
        if (tooltipPanel_3 != null) tooltipPanel_3.SetActive(false);
    }

    void Update()
    {
        // 마우스가 어떤 대상 UI 위에 있는지 계속 체크
        if (IsPointerOverTarget(targetUI_1))
        {
            ActivateTooltip(tooltipPanel_1);
        }
        else if (IsPointerOverTarget(targetUI_2))
        {
            ActivateTooltip(tooltipPanel_2);
        }
        else if (IsPointerOverTarget(targetUI_3))
        {
            ActivateTooltip(tooltipPanel_3);
        }
        else
        {
            if (activeTooltipPanel != null)
            {
                activeTooltipPanel.SetActive(false);
                activeTooltipPanel = null;
            }
        }
    }

    private bool IsPointerOverTarget(RectTransform targetUI)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(targetUI, Input.mousePosition, null);
    }

    private void ActivateTooltip(GameObject tooltipPanel)
    {
        if (activeTooltipPanel != tooltipPanel)
        {
            if (activeTooltipPanel != null)
                activeTooltipPanel.SetActive(false);

            tooltipPanel.SetActive(true);
            activeTooltipPanel = tooltipPanel;
        }
    }
}
