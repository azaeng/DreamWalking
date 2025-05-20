using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NickNameManager : MonoBehaviour
{
    public Text nicknameText;

    void Start()
    {
        string nickname = GetMyPhotonNickname();

        if (!string.IsNullOrEmpty(nickname))
        {
            nicknameText.text = nickname;
        }
        else
        {
            nicknameText.text = "닉네임을 불러올 수 없습니다.";
        }
    }

    private string GetMyPhotonNickname()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("nickname"))
        {
            return PhotonNetwork.LocalPlayer.CustomProperties["nickname"].ToString();
        }

        return null;
    }
}
