using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MyTurn : MonoBehaviourPun
{
    /*
    public Button diceButton; // 주사위 버튼 (에디터에서 연결)
    private TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        if (diceButton != null)
        {
            diceButton.onClick.AddListener(OnDiceButtonClicked);
            diceButton.gameObject.SetActive(false); // 처음엔 비활성화
        }
    }
    
    void Update()
    {
        if (!PhotonNetwork.InRoom || turnManager == null) return;

        // 자신의 턴인지 확인
        Player currentPlayer = turnManager.GetCurrentTurnPlayer();
        bool isMyTurn = PhotonNetwork.LocalPlayer == currentPlayer;

        // 주사위 버튼 활성화 여부 설정
        if (diceButton != null)
        {
            diceButton.gameObject.SetActive(isMyTurn);
        }
    }

    private void OnDiceButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient && turnManager != null)
        {
            turnManager.EndPlayerTurn(); // 마스터가 턴 넘김
        }
    }*/
}
