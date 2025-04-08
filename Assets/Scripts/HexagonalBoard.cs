using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonalBoard : MonoBehaviour
{
    public int layers; // 겹 수 (인스펙터에서 조정 가능)
    public GameObject[] hexPrefabs; // 육각형 오브젝트 프리팹 배열
    public GameObject obstaclePrefab; // 장애물 오브젝트 프리팹
    public GameObject obstaclePrefab2;
    private float hexRadius = 0.866f; // 육각형 변의 길이 (중심에서 변까지의 거리)
    private Dictionary<Vector3, GameObject> obstaclePositions = new Dictionary<Vector3, GameObject>(); // 장애물 위치 관리

    void Start()
    {
        GenerateBoard(layers);
    }

    void GenerateBoard(int n)
    {
        // 기존 보드 초기화
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        obstaclePositions.Clear(); // 기존 장애물 데이터 초기화

        int maxFloor = 4 * n + 1; // 마지막 층

        for (int floor = 1; floor <= maxFloor; floor++)
        {
            int hexCount = GetHexCount(floor, n);
            float z = floor * 0.866f; // 층 높이 계산 (6각형 높이 간격)
            float xOffset = (hexCount - 1) * -1.5f; // 중앙 정렬

            for (int i = 0; i < hexCount; i++)
            {
                float x = xOffset + (i * 3f);
                Vector3 position = new Vector3(x, 0, z);
                GameObject hexPrefab = hexPrefabs[Random.Range(0, hexPrefabs.Length)]; // 랜덤 프리팹 선택
                Instantiate(hexPrefab, position, Quaternion.identity, transform);

                // 육각형의 각 변에 장애물 배치
                PlaceObstacles(position, floor, n, i, hexCount);
            }
        }
    }

    int GetHexCount(int floor, int n)
    {
        if (floor <= n) return floor;                 // 증가 구간
        if (floor <= 3 * n + 1 && n % 2 == 0) return (floor % 2 == 0) ? n : n + 1; // 패턴 반복(짝수)
        if (floor <= 3 * n + 1) return (floor % 2 == 0) ? n + 1 : n; // 패턴 반복(홀수)
        if (floor <= 4 * n) return 4 * n + 2 - floor; // 감소 구간
        return 1; // 마지막 층 (4N+1층)
    }
    void PlaceObstacles(Vector3 hexPosition, int floor, int n, int index, int hexCount)
    {
        // 육각형의 6개 변 좌표 계산
        Vector3[] obstacleOffsets = new Vector3[6]
        {
            new Vector3(0, 0, hexRadius),         // 0, 위
            new Vector3(0.75f, 0, hexRadius/2),   // 1, 우측 위
            new Vector3(0.75f, 0, -hexRadius/2),  // 2, 우측 아래
            new Vector3(0, 0, -hexRadius),        // 3, 아래
            new Vector3(-0.75f, 0, -hexRadius/2), // 4, 좌측 아래
            new Vector3(-0.75f, 0, hexRadius/2)   // 5, 좌측 위
        };

        float[] obstacleRotations = new float[6] { 0, 60, 120, 180, 240, 300 }; // 변마다 회전

        List<GameObject> createdObstacles = new List<GameObject>(); // 생성된 장애물 리스트
        List<GameObject> obstaclesToRemove = new List<GameObject>(); // 제거할 장애물 리스트
        int removeCount = 0;

        for (int i = 0; i < 6; i++)
        {
            Vector3 obstaclePosition = hexPosition + obstacleOffsets[i];
            int maxFloor = 4 * n + 1; // 마지막 층

            // 장애물이 이미 존재하는지 확인 (0.1 거리 내에)
            bool shouldCreateObstacle = true;
            foreach (var existingObstacle in obstaclePositions)
            {
                // 위치와 거리 계산
                if (Vector3.Distance(existingObstacle.Key, obstaclePosition) < 0.1f)
                {
                    shouldCreateObstacle = false;
                    break; // 이미 근처에 장애물이 있으므로 중복 생성 방지
                }
            }

            if (shouldCreateObstacle)
            {
                GameObject selectedObstacle = obstaclePrefab;

                // 1층 (좌측 아래, 아래, 우측 아래)
                if (floor == 1 && (i == 4 || i == 3 || i == 2)) selectedObstacle = obstaclePrefab2;
                // 2 ~ n층 왼쪽(좌측 아래, 아래), 오른쪽(아래, 우측 아래)
                else if (floor <= n && (index == 0 && (i == 4 || i == 3) || index == hexCount - 1 && (i == 3 || i == 2))) selectedObstacle = obstaclePrefab2;
                // n+1층 왼쪽(좌측 위, 좌측 아래, 아래), 오른쪽(아래, 우측 아래, 우측 위)
                else if (floor == n + 1 && (index == 0 && (i == 5 || i == 4 || i == 3) || index == hexCount - 1 && (i == 3 || i == 2 || i == 1))) selectedObstacle = obstaclePrefab2;
                // n+3층 ~ O-n-2층 왼쪽(좌측 아래, 아래), 오른쪽(아래, 우측 아래)
                else if (floor >= n + 3 && floor <= maxFloor - n - 2 && (((n % 2 == 1 && floor % 2 == 0) || (n % 2 == 0 && floor % 2 == 1)) && (index == 0 && (i == 5 || i == 4) || index == hexCount - 1 && (i == 1 || i == 2)))) selectedObstacle = obstaclePrefab2;
                // O-n층 왼쪽(위, 좌측 위, 좌측 아래), 오른쪽(위, 우측 위, 우측 아래)
                else if (floor == maxFloor - n && (index == 0 && (i == 0 || i == 5 || i == 4) || index == hexCount - 1 && (i == 0 || i == 1 || i == 2))) selectedObstacle = obstaclePrefab2;
                // O-n+1층 ~ O-1층 왼쪽(좌측 위, 위), 오른쪽(위, 우측 위)
                else if (floor >= maxFloor - n + 1 && floor <= maxFloor - 1 && (index == 0 && (i == 5 || i == 0) || index == hexCount - 1 && (i == 0 || i == 1))) selectedObstacle = obstaclePrefab2;
                // O층 (좌측 위, 위, 우측 위)
                else if (floor == maxFloor) selectedObstacle = (i == 5 || i == 0 || i == 1) ? obstaclePrefab2 : obstaclePrefab;
                
                // 장애물 생성
                GameObject obstacle = Instantiate(selectedObstacle, obstaclePosition, Quaternion.Euler(0, obstacleRotations[i], 0), transform);
                obstaclePositions[obstaclePosition] = obstacle; // 위치 저장

                // obstaclePrefab인 경우만 삭제 후보에 추가
                if (selectedObstacle == obstaclePrefab)
                {
                    createdObstacles.Add(obstacle);
                }
            }
        }
        
        // 생성된 장애물 중에서 랜덤하게 최대 2개 제거
        while (removeCount < 2 && createdObstacles.Count > 0)
        {
            int randomIndex = Random.Range(0, createdObstacles.Count);
            GameObject obstacleToRemove = createdObstacles[randomIndex];
            obstaclesToRemove.Add(obstacleToRemove);
            createdObstacles.RemoveAt(randomIndex);
            removeCount++;
        }

        // 장애물 제거
        foreach (GameObject obj in obstaclesToRemove)
        {
            Destroy(obj);
            obstaclePositions.Remove(obj.transform.position);
        }
    }
}
