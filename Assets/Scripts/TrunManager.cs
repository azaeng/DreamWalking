using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public Text logText; // UI에 로그 출력용 텍스트

    private int playerCount;
    private Dictionary<Player, int> playerRandomNumbers = new Dictionary<Player, int>();

    void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        AppendLog($"현재 방에 있는 플레이어 수: {playerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            AppendLog("이 클라이언트는 마스터입니다. 랜덤 숫자를 부여합니다.");
            AssignRandomNumbersToPlayers();
            DetermineTurnOrder(); // 턴 순서 결정
        }
        else
        {
            AppendLog("이 클라이언트는 마스터가 아닙니다. 랜덤 숫자를 부여하지 않습니다.");
        }
    }

    private void AssignRandomNumbersToPlayers()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int randomNumber = Random.Range(1, 10); // 1~9 사이
            playerRandomNumbers[player] = randomNumber;
            AppendLog($"플레이어 {player.NickName}에게 랜덤 숫자 {randomNumber}를 부여했습니다.");
        }
    }

    private void DetermineTurnOrder()
    {
        AppendLog("\n[턴 순서]");

        // 랜덤 숫자를 기준으로 내림차순 정렬
        var sortedPlayers = playerRandomNumbers
            .OrderByDescending(entry => entry.Value)
            .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var player = sortedPlayers[i].Key;
            var number = sortedPlayers[i].Value;
            AppendLog($"{i + 1}번 턴: {player.NickName} (랜덤 숫자: {number})");
        }
    }

    private void AppendLog(string message)
    {
        if (logText != null)
        {
            logText.text += message + "\n";
        }
        else
        {
            Debug.LogWarning("LogText UI가 연결되지 않았습니다.");
        }
    }
}
