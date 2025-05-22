using Photon.Pun;
using UnityEngine;

public class Board : MonoBehaviourPun
{
    public GameObject[] objectPrefabs; // 3가지 프리팹을 여기에 드래그하여 지정

    void Start()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0)
        {
            Debug.LogWarning("변환할 프리팹이 없습니다.");
            return;
        }

        // 현재 위치 및 회전 정보 저장
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // 무작위 프리팹 선택
        int index = Random.Range(0, objectPrefabs.Length);
        string prefabName = objectPrefabs[index].name;
        // GameObject selectedPrefab = objectPrefabs[index];

        // 새로운 오브젝트 생성
        // Instantiate(selectedPrefab, currentPosition, currentRotation);

        // PhotonNetwork로 오브젝트 생성 (마스터 클라이언트일 때만)
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(prefabName, currentPosition, currentRotation);
        }

        // 자신 제거
        Destroy(gameObject);
    }

    [PunRPC]
    void SpawnObject(int index)
    {
        if (objectPrefabs == null || index < 0 || index >= objectPrefabs.Length)
        {
            Debug.LogWarning("유효하지 않은 인덱스입니다.");
            return;
        }

        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        string prefabName = objectPrefabs[index].name;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(prefabName, currentPosition, currentRotation);
        }

        Destroy(gameObject);
    }
}
