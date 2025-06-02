using UnityEngine;
using Photon.Pun;

public class BoardClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("보드 클릭됨!");
        if (PhotonNetwork.LocalPlayer.TagObject is GameObject playerObj)
        {
            PhotonView view = playerObj.GetComponent<PhotonView>();

            if (view != null && view.IsMine)
            {
                Debug.Log("내 플레이어 찾음, 이동 시도");
                var move = playerObj.GetComponent<PlayerMovement>();
                if (move != null)
                {
                    move.MoveToPosition(transform.position);
                }
                else
                {
                    Debug.LogWarning("PlayerMovement 컴포넌트 없음");
                }
            }
            else
            {
                Debug.Log("PhotonView가 없거나 IsMine이 아님");
            }
        }
        else
        {
            Debug.Log("내 플레이어 오브젝트를 찾지 못함 (TagObject 비어있음)");
        }
    }
}
