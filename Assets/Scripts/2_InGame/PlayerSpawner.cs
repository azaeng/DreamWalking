using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public string playerPrefabName = "플레이어"; // Resources/Player.prefab

    void Start()
    {
        StartCoroutine(SpawnPlayerWithDelay());
    }
    
    IEnumerator SpawnPlayerWithDelay()
    {
        // 보드가 생성될 시간을 기다림 (0.5초~1초 정도)
        yield return new WaitForSeconds(0.5f);

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // "보드(마을)"이 이름에 포함된 오브젝트를 모두 찾아 위치 저장
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            List<Vector3> spawnCandidates = new List<Vector3>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("보드(마을)(Clone)"))
                {
                    Debug.LogWarning("뭐지?");
                    spawnCandidates.Add(obj.transform.position);
                }
            }

            // 랜덤 위치 선택
            Vector3 spawn = Vector3.zero;
            if (spawnCandidates.Count > 0)
            {
                Debug.LogWarning("뭐지???");
                int index = Random.Range(0, spawnCandidates.Count);
                spawn = spawnCandidates[index] + Vector3.up * 0.5f;
            }

            // 플레이어 생성
            GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawn, Quaternion.identity);

            // 자신의 오브젝트만 빨간색으로 표시
            if (player.GetComponent<PhotonView>().IsMine)
            {
                Renderer renderer = player.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }
        }
    }
}
