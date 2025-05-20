using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Threading.Tasks;

public class PhotonData : MonoBehaviourPunCallbacks
{
    public const string NICKNAME_KEY = "nickname";

    async void Start()
    {
        // 닉네임 가져오기
        string nickname = await FirebaseAuthManager.Instance.GetNickname();

        if (!string.IsNullOrEmpty(nickname))
        {
            // Photon Player의 Custom Properties에 닉네임 설정
            Hashtable playerProperties = new Hashtable
            {
                { NICKNAME_KEY, nickname }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            Debug.Log("Photon에 닉네임 설정됨: " + nickname);
        }
        else
        {
            Debug.LogWarning("닉네임을 가져올 수 없습니다.");
        }
    }
}
