using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class Obstacle : MonoBehaviourPun
{
    public static List<GameObject> storedObstacles = new List<GameObject>(); // 최대 3개까지 저장
    public float interactRange = 2.5f; // 플레이어 근처 거리 제한

    private GameObject Player;       // 내 플레이어 오브젝트
    private GameObject myTurnObj;    // 내 턴 UI 오브젝트 (활성/비활성 모두 포함)

    private static bool isRemoved = false; // 한 턴에 한 번만 제거 가능

    void Start()
    {
        StartCoroutine(InitPlayerAndTurnObj());
    }

    private System.Collections.IEnumerator InitPlayerAndTurnObj()
    {
        // 내 플레이어 오브젝트가 생성될 때까지 대기
        while (PlayerSpawner.MyPlayerObject == null) { yield return null; }

        Player = PlayerSpawner.MyPlayerObject;

        // 비활성화된 오브젝트 포함하여 MyTurn 오브젝트 찾기
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "MyTurn")
            {
                myTurnObj = obj;
                break;
            }
        }

        if (myTurnObj == null)
        {
            Debug.LogWarning("MyTurn 오브젝트를 찾을 수 없습니다.");
        }
    }

    void OnMouseDown()
    {
        if (Player == null || myTurnObj == null)
        {
            Debug.LogWarning("Player 또는 MyTurn이 null입니다.");
            return;
        }

        // 내 턴이 아닐 경우
        if (!myTurnObj.activeInHierarchy)
        {
            Debug.LogWarning("현재 내 턴이 아닙니다.");
            return;
        }

        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance > interactRange)
        {
            Debug.LogWarning("플레이어가 너무 멀리 있음");
            return;
        }

        if (!gameObject.name.Contains("벽")) // 벽이 아니면 무시
        {
            Debug.LogWarning("클릭한 오브젝트는 벽이 아님");
            return;
        }

        if (isRemoved) // 이미 제거했으면 리턴
        {
            Debug.LogWarning("이번 턴에는 이미 제거했습니다.");
            return;
        }

        if (storedObstacles.Count >= 3)
        {
            Debug.LogWarning("이미 최대 3개의 장애물을 제거했습니다.");
            return;
        }

        // 장애물 저장 및 제거
        storedObstacles.Add(gameObject);
        photonView.RPC("RequestDestroy", RpcTarget.MasterClient, photonView.ViewID);

        isRemoved = true; // 한 턴에 한 번만 제거되도록 설정
    }

    [PunRPC]
    void RequestDestroy(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(targetView.gameObject);
        }
    }
    
    // 외부에서 이 메서드를 호출해 턴이 시작될 때 플래그를 초기화
    public static void ResetObstacleRemovalFlag()
    {
        isRemoved = false;
    }
}
