using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용한다면 필요

public class ItemTooltipManager : MonoBehaviour
{
    public static ItemTooltipManager Instance { get; private set; } // 싱글톤 인스턴스

    [SerializeField] private GameObject tooltipPanel; // 설명창 패널 GameObject
    [SerializeField] private Image itemPreviewImage; // 아이템 미리보기 Image
    [SerializeField] private TextMeshProUGUI itemNameText; // 아이템 이름(설명)Text

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // 초기에는 툴팁 패널을 비활성화.
        tooltipPanel.SetActive(false);
    }

    // 툴팁을 보여주는 메서드
    public void ShowTooltip(Item itemToShow)
    {
        if (itemToShow == null)
        {
            HideTooltip();
            return;
        }

        // 아이템 이미지 설정
        if (itemToShow.sprite != null)
        {
            itemPreviewImage.sprite = itemToShow.sprite;
            itemPreviewImage.gameObject.SetActive(true); // 이미지가 있다면 활성화
        }
        else
        {
            itemPreviewImage.gameObject.SetActive(false); // 이미지가 없다면 비활성화
        }

        // 아이템 텍스트 정보 설정
        // ++ 아이템 설명, 스탯 등을 추가
        itemNameText.text = itemToShow.name; // Item ScriptableObject의 이름을 가져옴

        // 툴팁 패널 활성화
        tooltipPanel.SetActive(true);
    }

    // 툴팁을 숨기는 메서드
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    // 툴팁의 위치를 업데이트하는 메서드 (마우스 위치에 따라)
    void Update()
    {
        // 툴팁이 활성화되어 있을 때만 마우스 위치를 따라가도록 함
        if (tooltipPanel.activeSelf)
        {
            // 마우스 위치에 약간의 오프셋을 주어 툴팁이 마우스 커서를 가리지 않도록 함
            Vector2 mousePos = Input.mousePosition;
            tooltipPanel.transform.position = mousePos + new Vector2(80, 0);
        }
    }
}