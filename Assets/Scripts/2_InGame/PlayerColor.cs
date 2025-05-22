using Photon.Pun;
using UnityEngine;

public class PlayerColor : MonoBehaviourPun
{
    void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();

        // 자신의 플레이어 오브젝트만 빨간색으로 설정
        if (renderer != null)
        {
            if (photonView.IsMine)
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.white;
        }
    }
}
