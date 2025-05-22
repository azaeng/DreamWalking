using Photon.Pun;
using UnityEngine;

public class PlayerColor : MonoBehaviourPun
{
    void Start()
    {
        // 로컬 플레이어만 빨간색으로 설정
        if (photonView.IsMine)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }
        }
    }
}
