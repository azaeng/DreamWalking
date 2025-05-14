using Photon.Pun;
using UnityEngine;

public class InGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnEnemiesAsMaster();
        }
    }

    // 마스터 클라이언트가 호출: 오브젝트 생성 + 다른 클라이언트에 동기화
    private void SpawnEnemiesAsMaster()
    {
        Vector3[] spawnPositions = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(2, 0, 1),
            new Vector3(-2, 0, -1)
        };

        foreach (var pos in spawnPositions)
        {
            // 마스터 클라이언트는 직접 생성
            Instantiate(enemyPrefab, pos, Quaternion.identity);

            // 다른 클라이언트에게 RPC로 생성 지시
            photonView.RPC("RPC_SpawnEnemy", RpcTarget.OthersBuffered, pos);
        }
    }

    // 클라이언트가 호출되는 함수: 마스터가 보낸 위치에 오브젝트 생성
    [PunRPC]
    void RPC_SpawnEnemy(Vector3 spawnPosition)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
