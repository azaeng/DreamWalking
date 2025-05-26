using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static GameObject MyPlayerObject; // 내 플레이어를 저장하는 static 변수
    public string playerPrefabName = "플레이어"; // Resources/플레이어.prefab
    private List<Vector3> spawnCandidates = new List<Vector3>();

    void Start()
    {
        StartCoroutine(SpawnPlayerWithDelay());
    }
    
    IEnumerator SpawnPlayerWithDelay()
    {
        // 보드가 생성될 시간을 기다림 (0.5초~1초 정도)
        yield return new WaitForSeconds(1.0f);

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // 보드(마을) 오브젝트들을 모두 찾아 위치 저장
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("보드(마을)"))
                {
                    spawnCandidates.Add(obj.transform.position);
                }
            }

            // 인원 수보다 적은 스폰 지점일 경우 방지
            if (PhotonNetwork.CurrentRoom.PlayerCount > spawnCandidates.Count)
            {
                Debug.LogError("스폰 지점 수보다 플레이어 수가 많습니다.");
                yield break;
            }

            // 각 플레이어는 자신의 ActorNumber에 따라 고유한 위치에서 스폰 (ActorNumber는 1부터 시작)
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (index >= 0 && index < spawnCandidates.Count)
            {
                Vector3 spawnPos = spawnCandidates[index] + Vector3.up * 0.5f;
                GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);

                // 자신의 플레이어 오브젝트만 빨간색으로 표시
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    MyPlayerObject = player; // 저장
                    Renderer renderer = player.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = Color.red;
                    }
                }
            }
        }
    }
}
