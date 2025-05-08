using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject[] objectPrefabs; // 3가지 프리팹을 여기에 드래그하여 지정

    void Start()
    {
        if (objectPrefabs.Length == 0)
        {
            Debug.LogWarning("변환할 프리팹이 없습니다.");
            return;
        }

        // 현재 위치 및 회전 정보 저장
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        // 무작위 프리팹 선택
        int index = Random.Range(0, objectPrefabs.Length);
        GameObject selectedPrefab = objectPrefabs[index];

        // 새로운 오브젝트 생성
        Instantiate(selectedPrefab, currentPosition, currentRotation);

        // 자신 제거
        Destroy(gameObject);
    }
}
