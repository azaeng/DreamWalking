using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon; // <- 이거 꼭 필요함

public class NickNameManager : MonoBehaviourPunCallbacks
{
    public Text logText;

    private void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            AssignNickNames();
        }
    }

    void AssignNickNames()
    {
        int counter = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (counter > 10) break;
            string tempName = $"player{counter}";

            if (PhotonNetwork.IsMasterClient)
            {
                if (!player.CustomProperties.ContainsKey("NickName"))
                {
                    Hashtable nickProp = new Hashtable { { "NickName", tempName } };
                    player.SetCustomProperties(nickProp);
                }
            }

            counter++;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("NickName"))
        {
            string nick = changedProps["NickName"].ToString();

            if (targetPlayer == PhotonNetwork.LocalPlayer)
            {
                AppendLog($"[나] 닉네임 설정됨: {nick}");
            }
            else
            {
                AppendLog($"{targetPlayer.ActorNumber} 닉네임 설정됨: {nick}");
            }
        }
    }


     // 로그 출력
    private void AppendLog(string message)
    {
        if (logText != null)
            logText.text += message + "\n";
        else
            Debug.LogWarning("LogText UI가 연결되지 않았습니다.");
    }
}
