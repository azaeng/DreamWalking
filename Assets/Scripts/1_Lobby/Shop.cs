using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Search;

public class ShopManager : MonoBehaviour
{
    // UI 연결
    public TextMeshProUGUI currencyText;
    public Button buyButton;
    public TextMeshProUGUI resultText;

    // 임시 플레이어, 아이템 가격 데이터
    [SerializeField] int playerCurrency = 1000;
    [SerializeField] int itemPrice = 600;

    void Start()
    {
        UpdateCurrencyUI();
        resultText.text = "";
        buyButton.onClick.AddListener(BuyItem);
    }

    void UpdateCurrencyUI()
    {
        currencyText.text = $"Gold: {playerCurrency}";
    }

    void BuyItem()
    {
        if (playerCurrency >= itemPrice)
        {
            playerCurrency -= itemPrice;
            resultText.text = "Success!";
        }
        else
        {
            resultText.text = "Oops! Not enough gold..";
        }

        UpdateCurrencyUI();
    }
}
