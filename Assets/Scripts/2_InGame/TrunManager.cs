using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public Button endTurnButton; // 턴 종료 버튼
    public Text turnInfoText;    // 현재 턴 표시용 UI

    private List<Player> turnOrder = new List<Player>();
    private int currentTurnIndex = 0;

    void Start()
    {
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

        if (PhotonNetwork.IsMasterClient)
        {
            GenerateTurnOrder();
            photonView.RPC("SetTurnOrder", RpcTarget.AllBuffered, GetTurnOrderActorNumbers());
        }
    }

    void OnEndTurnButtonClicked()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == turnOrder[currentTurnIndex].ActorNumber)
        {
            photonView.RPC("NextTurn", RpcTarget.AllBuffered);
        }
    }

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

    int[] GetTurnOrderActorNumbers()
    {
        int[] order = new int[turnOrder.Count];
        for (int i = 0; i < turnOrder.Count; i++)
        {
            order[i] = turnOrder[i].ActorNumber;
        }
        return order;
    }

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
        UpdateTurnUI();
    }

    [PunRPC]
    void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        UpdateTurnUI();
    }

    void UpdateTurnUI()
    {
        if (turnInfoText != null)
        {
            Player currentPlayer = turnOrder[currentTurnIndex];
            string playerName = GetPlayerNickname(currentPlayer);

            bool isMyTurn = PhotonNetwork.LocalPlayer.ActorNumber == currentPlayer.ActorNumber;

            turnInfoText.text = isMyTurn ? $"당신의 턴입니다 ({playerName})" : $"{playerName}의 턴입니다";

            endTurnButton.interactable = isMyTurn;
        }
    }
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