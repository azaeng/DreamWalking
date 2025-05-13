using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject ConnectPanel; // 연결 버튼 및 UI가 포함된 패널

    [SerializeField]
    private Text StatusText; // 상태 메시지를 출력할 텍스트 UI

    private bool isConnecting = false; // 현재 서버에 연결 중인지 여부를 저장
    private const string gameVersion = "v1"; // 게임 버전 (서버와 클라이언트의 버전이 일치해야 함)

    void Awake()
    {
        if (ConnectPanel == null)
        {
            Debug.LogError("ConnectPanel 오브젝트를 설정해주세요.");
        }

        if (StatusText == null)
        {
            Debug.LogError("StatusText 오브젝트를 설정해주세요.");
        }

        // 마스터 클라이언트가 씬을 로드하면 자동으로 모든 클라이언트가 해당 씬으로 전환됨
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 연결 버튼을 누르면 호출되는 함수
    public void Connect()
    {
        isConnecting = true;
        ConnectPanel.SetActive(false); // 연결 중에는 UI 비활성화
        ShowStatus("서버에 연결 중...");

        if (PhotonNetwork.IsConnected)
        {
            ShowStatus("랜덤 방에 참가 중...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            ShowStatus("서버에 연결 중...");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    // 종료 버튼을 눌렀을 때 호출되는 함수
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 마스터 서버에 연결 완료 시 호출
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            ShowStatus("서버에 연결됨, 랜덤 방 참가 중...");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    // 랜덤 방 참가 실패 시 호출
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ShowStatus("랜덤 방 참가 실패. 새 방 생성 중...");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    // 방에 참가 성공 시 호출
    public override void OnJoinedRoom()
    {
        ShowStatus("방 참가 완료 - 다른 플레이어를 기다리는 중...");
    }

    // 다른 플레이어가 방에 입장했을 때 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            // 모든 플레이어가 이 씬으로 이동하게 됨
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    // 서버 연결이 끊겼을 때 호출
    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        ConnectPanel.SetActive(true);
        ShowStatus("연결이 끊어졌습니다.");
    }

    // 상태 메시지를 UI에 표시하는 함수
    private void ShowStatus(string text)
    {
        if (StatusText == null) return;

        StatusText.gameObject.SetActive(true);
        StatusText.text = text;
    }
}
