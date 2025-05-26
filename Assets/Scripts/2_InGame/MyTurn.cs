using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MyTurn : MonoBehaviourPun
{
    public Button Btn_Rolling;  // 주사위 버튼
    public Button Btn_End;      // 턴 종료 버튼

    public Image Dice_1;        // 주사위 1 이미지
    public Image Dice_2;        // 주사위 2 이미지
    public Sprite[] diceSprites;    // 주사위 눈 1~6 이미지 배열
    private int dice1Value;
    private int dice2Value;

    void Start()
    {
        // 턴 종료 버튼 클릭 시 이벤트 연결
        Btn_Rolling.onClick.AddListener(Rolling);   // 주사위 굴리기
        Btn_End.onClick.AddListener(EndTurn);       // 턴 종료
    }

    // 오브젝트가 활성화될 때 자동 호출
    void OnEnable()
    {
        if (Btn_Rolling != null)
        {
            Btn_Rolling.gameObject.SetActive(true);
            // Btn_Rolling.interactable = true;
        }
        if (Btn_End != null) {Btn_End.gameObject.SetActive(false);}
    }

    void Rolling()
    {
        // 랜덤 숫자 생성 (1~6)
        dice1Value = Random.Range(0, 6);
        dice2Value = Random.Range(0, 6);

        // 이미지 설정 (Sprite 배열에서 선택)
        if (diceSprites != null && diceSprites.Length == 6)
        {
            Dice_1.sprite = diceSprites[dice1Value];
            Dice_2.sprite = diceSprites[dice2Value];
        }

        Btn_Rolling.gameObject.SetActive(false);  // 주사위 버튼 비활성화
        Btn_End.gameObject.SetActive(true);       // 턴 종료 버튼 활성화
        // Btn_Rolling.interactable = false;
    }

    void EndTurn()
    {
        Btn_End.gameObject.SetActive(false);      // 종료 버튼 비활성화
        this.gameObject.SetActive(false);         // 오브젝트 비활성화 (턴 종료)
    }
}
