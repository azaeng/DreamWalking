using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviourPunCallbacks
{
    // 현재 방에 있는 플레이어 수를 저장할 변수
    private int playerCount;

    // 닉네임별로 랜덤 숫자를 저장할 딕셔너리
    private Dictionary<string, int> playerRandomNumbers = new Dictionary<string, int>();

    void Start()
    {
        // 방에 있는 모든 플레이어 수를 세고 랜덤 숫자 부여
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log("현재 방에 있는 플레이어 수: " + playerCount);

        AssignRandomNumbersToNicknames();
    }

    // 각 닉네임에 랜덤 숫자 부여
    private void AssignRandomNumbersToNicknames()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int randomNumber = Random.Range(1, 101); // 1~100 사이의 랜덤 숫자
            string nickname = player.NickName;

            if (!playerRandomNumbers.ContainsKey(nickname))
            {
                playerRandomNumbers[nickname] = randomNumber;
                Debug.Log($"닉네임 '{nickname}'에게 랜덤 숫자 {randomNumber}를 부여했습니다.");
            }
        }
    }
}
