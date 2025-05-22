using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoardFrame : MonoBehaviourPunCallbacks
{
    public int layers; // 겹 수 (인스펙터에서 조정 가능)
    public GameObject board;    // 일반 보드 프리팹
    public GameObject spawn;    // 스폰 지점 프리팹
    public GameObject obstacle; // 장애물 프리팹
    public GameObject obstacle2;
    public GameObject Player; // 인스펙터에서 캐릭터 프리팹 지정

    private float hexRadius = 0.866f; // 육각형 변의 길이 (중심에서 변까지의 거리)
    private Dictionary<Vector3, GameObject> obstaclePositions = new Dictionary<Vector3, GameObject>(); // 장애물 위치 관리
    private List<Vector3> spawnPositions = new List<Vector3>(); // 스폰 위치 저장

    void Start()
    {
        // GenerateBoard(layers);
        // 마스터 클라이언트만 보드 생성
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateBoard(layers);

            // 플레이어 스폰 처리
            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length && i < spawnPositions.Count; i++)
            {
                Vector3 spawnPos = spawnPositions[i] + Vector3.up * 0.5f; // 스폰 오브젝트 위
                PhotonNetwork.Instantiate(Player.name, spawnPos, Quaternion.identity);
            }
        }
    }

    void GenerateBoard(int n)
    {
        // 기존 보드 초기화
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        obstaclePositions.Clear(); // 기존 장애물 데이터 초기화
        spawnPositions.Clear();    // 스폰 위치 초기화

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

                // 스폰 위치 조건
                bool isSpawnPosition = ((floor == 3 && i == hexCount / 2) || (floor == 3+n-1 && i == 0) || (floor == 3+n-1 && i == hexCount-1) || (floor == maxFloor-n-1 && i == 0) || (floor == maxFloor-n-1 && i == hexCount-1) || (floor == maxFloor-2 && i == hexCount/2)); // 원하는 조건으로 수정 가능

                if (isSpawnPosition)
                {
                    // Instantiate(spawn, position, Quaternion.identity, transform);
                    PhotonNetwork.Instantiate("보드(마을)", position, Quaternion.identity);
                    spawnPositions.Add(position); // 스폰 위치 저장
                }
                else
                {
                    Instantiate(board, position, Quaternion.identity, transform);
                }
                
                // 육각형의 각 변에 장애물 배치
                PlaceObstacles(position, floor, n, i, hexCount);
            }
        }
    }

    int GetHexCount(int floor, int n) // 보드 패턴 파악
    {
        if (floor <= n) return floor;                 // 증가 구간
        if (floor <= 3 * n + 1 && n % 2 == 0) return (floor % 2 == 0) ? n : n + 1; // 패턴 반복(짝수)
        if (floor <= 3 * n + 1) return (floor % 2 == 0) ? n + 1 : n; // 패턴 반복(홀수)
        if (floor <= 4 * n) return 4 * n + 2 - floor; // 감소 구간
        return 1; // 마지막 층 (4N+1층)
    }
    void PlaceObstacles(Vector3 hexPosition, int floor, int n, int index, int hexCount) // 장애물 생성
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
        int maxFloor = 4 * n + 1; // 마지막 층
        
        for (int i = 0; i < 6; i++)
        {
            Vector3 obstaclePosition = hexPosition + obstacleOffsets[i];

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
                GameObject selectedObstacle = this.obstacle;

                // 1층 (좌측 아래, 아래, 우측 아래)
                if (floor == 1 && (i == 4 || i == 3 || i == 2)) selectedObstacle = obstacle2;
                // 2 ~ n층 왼쪽(좌측 아래, 아래), 오른쪽(아래, 우측 아래)
                else if (floor <= n && (index == 0 && (i == 4 || i == 3) || index == hexCount - 1 && (i == 3 || i == 2))) selectedObstacle = obstacle2;
                // n+1층 왼쪽(좌측 위, 좌측 아래, 아래), 오른쪽(아래, 우측 아래, 우측 위)
                else if (floor == n + 1 && (index == 0 && (i == 5 || i == 4 || i == 3) || index == hexCount - 1 && (i == 3 || i == 2 || i == 1))) selectedObstacle = obstacle2;
                // n+3층 ~ O-n-2층 왼쪽(좌측 아래, 아래), 오른쪽(아래, 우측 아래)
                else if (floor >= n + 3 && floor <= maxFloor - n - 2 && (((n % 2 == 1 && floor % 2 == 0) || (n % 2 == 0 && floor % 2 == 1)) && (index == 0 && (i == 5 || i == 4) || index == hexCount - 1 && (i == 1 || i == 2)))) selectedObstacle = obstacle2;
                // O-n층 왼쪽(위, 좌측 위, 좌측 아래), 오른쪽(위, 우측 위, 우측 아래)
                else if (floor == maxFloor - n && (index == 0 && (i == 0 || i == 5 || i == 4) || index == hexCount - 1 && (i == 0 || i == 1 || i == 2))) selectedObstacle = obstacle2;
                // O-n+1층 ~ O-1층 왼쪽(좌측 위, 위), 오른쪽(위, 우측 위)
                else if (floor >= maxFloor - n + 1 && floor <= maxFloor - 1 && (index == 0 && (i == 5 || i == 0) || index == hexCount - 1 && (i == 0 || i == 1))) selectedObstacle = obstacle2;
                // O층 (좌측 위, 위, 우측 위)
                else if (floor == maxFloor) selectedObstacle = (i == 5 || i == 0 || i == 1) ? obstacle2 : this.obstacle;

                // 장애물 생성
                string prefabName = selectedObstacle.name;
                GameObject obstacle = PhotonNetwork.Instantiate(prefabName, obstaclePosition, Quaternion.Euler(0, obstacleRotations[i], 0));
                // GameObject obstacle = Instantiate(selectedObstacle, obstaclePosition, Quaternion.Euler(0, obstacleRotations[i], 0), transform);
                obstaclePositions[obstaclePosition] = obstacle; // 위치 저장

                // obstaclePrefab인 경우만 삭제 후보에 추가
                if (selectedObstacle == this.obstacle)
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
            PhotonNetwork.Destroy(obj); // PhotonNetwork로 제거
            // obstaclePositions.Remove(obj.transform.position);
        }
    }
}
