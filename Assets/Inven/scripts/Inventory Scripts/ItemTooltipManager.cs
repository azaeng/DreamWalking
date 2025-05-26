using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����Ѵٸ� �ʿ�

public class ItemTooltipManager : MonoBehaviour
{
    public static ItemTooltipManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    [SerializeField] private GameObject tooltipPanel; // ����â �г� GameObject
    [SerializeField] private Image itemPreviewImage; // ������ �̸����� Image
    [SerializeField] private TextMeshProUGUI itemNameText; // ������ �̸�(����)Text

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

        // �ʱ⿡�� ���� �г��� ��Ȱ��ȭ.
        tooltipPanel.SetActive(false);
    }

    // ������ �����ִ� �޼���
    public void ShowTooltip(Item itemToShow)
    {
        if (itemToShow == null)
        {
            HideTooltip();
            return;
        }

        // ������ �̹��� ����
        if (itemToShow.sprite != null)
        {
            itemPreviewImage.sprite = itemToShow.sprite;
            itemPreviewImage.gameObject.SetActive(true); // �̹����� �ִٸ� Ȱ��ȭ
        }
        else
        {
            itemPreviewImage.gameObject.SetActive(false); // �̹����� ���ٸ� ��Ȱ��ȭ
        }

        // ������ �ؽ�Ʈ ���� ����
        // ++ ������ ����, ���� ���� �߰�
        itemNameText.text = itemToShow.name; // Item ScriptableObject�� �̸��� ������

        // ���� �г� Ȱ��ȭ
        tooltipPanel.SetActive(true);
    }

    // ������ ����� �޼���
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    // ������ ��ġ�� ������Ʈ�ϴ� �޼��� (���콺 ��ġ�� ����)
    void Update()
    {
        // ������ Ȱ��ȭ�Ǿ� ���� ���� ���콺 ��ġ�� ���󰡵��� ��
        if (tooltipPanel.activeSelf)
        {
            // ���콺 ��ġ�� �ణ�� �������� �־� ������ ���콺 Ŀ���� ������ �ʵ��� ��
            Vector2 mousePos = Input.mousePosition;
            tooltipPanel.transform.position = mousePos + new Vector2(80, 0);
        }
    }
}