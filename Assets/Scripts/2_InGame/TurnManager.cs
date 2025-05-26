using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public GameObject MyTurn;    // 현재 턴인 플레이어에게만 활성화 오브젝트
    public Text turnInfoText;    // 현재 턴 정보 표시용 UI 텍스트

    private List<Player> turnOrder = new List<Player>(); // 턴 순서를 저장하는 리스트
    private int currentTurnIndex = 0; // 현재 턴인 플레이어의 인덱스
     private bool hasEndedTurn = false;                  // 이미 턴 넘김 여부

    void Start()
    {
        // 마스터 클라이언트가 턴 순서 생성 및 모든 클라이언트에 공유
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateTurnOrder(); // 랜덤 턴 순서 생성
            photonView.RPC("SetTurnOrder", RpcTarget.AllBuffered, GetTurnOrderActorNumbers()); // 턴 순서 전파
        }
    }
    void Update()
    {
        // 내 턴이고 MyTrun이 꺼졌으면 턴 종료
        if (IsMyTurn() && MyTurn != null && !MyTurn.activeSelf && !hasEndedTurn)
        {
            hasEndedTurn = true;
            photonView.RPC("NextTurn", RpcTarget.AllBuffered);
        }
    }
    // 플레이어 리스트에서 무작위로 턴 순서를 생성
    void GenerateTurnOrder()
    {
        List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
        while (players.Count > 0)
        {
            int randomIndex = Random.Range(0, players.Count);
            turnOrder.Add(players[randomIndex]);
            players.RemoveAt(randomIndex);
        }
    }

    // 턴 순서를 ActorNumber 배열로 변환
    int[] GetTurnOrderActorNumbers()
    {
        int[] order = new int[turnOrder.Count];
        for (int i = 0; i < turnOrder.Count; i++)
        {
            order[i] = turnOrder[i].ActorNumber;
        }
        return order;
    }

    // RPC: 클라이언트에 턴 순서 설정
    [PunRPC]
    void SetTurnOrder(int[] actorNumbers)
    {
        turnOrder.Clear();
        foreach (int actorNumber in actorNumbers)
        {
            Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
            if (player != null)
                turnOrder.Add(player);
        }
        UpdateTurnUI(); // UI 갱신
    }

    // RPC: 다음 턴으로 넘어감
    [PunRPC]
    void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        hasEndedTurn = false; // 다음 턴을 위해 초기화
        UpdateTurnUI(); // UI 갱신
    }

    // 현재 턴 UI 갱신 (누구의 턴인지 표시하고, 턴 종료 버튼 활성/비활성)
    void UpdateTurnUI()
    {
        if (turnInfoText != null)
        {
            Player currentPlayer = turnOrder[currentTurnIndex];
            string playerName = GetPlayerNickname(currentPlayer);

            bool isMyTurn = PhotonNetwork.LocalPlayer.ActorNumber == currentPlayer.ActorNumber;
            turnInfoText.text = isMyTurn ? $"당신의 턴입니다 ({playerName})" : $"{playerName}의 턴입니다";

            if (MyTurn != null)
            {
                MyTurn.SetActive(isMyTurn);
            }
        }
    }

    // 현재 플레이어가 내 턴인지 확인
    bool IsMyTurn()
    {
        if (turnOrder.Count == 0) return false;
        return PhotonNetwork.LocalPlayer.ActorNumber == turnOrder[currentTurnIndex].ActorNumber;
    }

    // 플레이어의 닉네임 가져오기 (없으면 기본값 설정)
    string GetPlayerNickname(Player player)
    {
        if (player.CustomProperties.ContainsKey("nickname"))
        {
            return player.CustomProperties["nickname"].ToString();
        }
        else if (!string.IsNullOrEmpty(player.NickName))
        {
            return player.NickName;
        }
        else
        {
            return $"플레이어 {player.ActorNumber}";
        }
    }
}
