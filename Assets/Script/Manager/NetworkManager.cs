using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;


public class NetworkManager : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI StatusText;
    public InputField RoomInput, NickNameInput;

    // 화면 크기 설정
    private void Awake()
    {        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Setting();
    }

    // 상태 텍스트 업데이트
    private void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    // Setting
    #region Setting
    private void Setting()
    {
        // 포톤의 기본 셋팅
        Screen.SetResolution(1920, 1080, false);

        // 모든 네트워크 보기에 대한 네트워크 업데이트의 기본 전송 속도
        PhotonNetwork.SendRate = 60;

        // PhotonView 들이 OnPhotonSerialize를 초당 몇회 호출할지
        PhotonNetwork.SerializationRate = 30;

        // 마스터 서버에서 장면을 전환할 때 다른 클라이언트의 장면을 자동으로 동기화합니다.
        PhotonNetwork.AutomaticallySyncScene = true;

        // Don't get a photonmessage when access the room.
        PhotonNetwork.IsMessageQueueRunning = false;
    }
    #endregion

    // Network
    #region Network

    // 서버 연결 - 버튼 연결할 것
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // 서버 연결 성공
    public override void OnConnectedToMaster()
    {
        Debug.Log("Server Join Success");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    // 서버 연결 끊김
    public void Disconnected() => PhotonNetwork.Disconnect();
    // 서버 연결 끊긴 이유? 받아옴
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("Server Disconnected");

    #endregion

    // 로비 참가
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby"); // 로비 접속 완료
        PhotonNetwork.LoadLevel("01_LOBBYSCENE");

    }

    // Room
    #region Room

    // 방 생성
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 2 });

    // 방 참가
    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomInput.text);

    // public override void OnJoinedRoom() => Debug.Log("Joined Room"); // 방 참가 완료

    // 방 생성 또는 참가
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom("TEST", new RoomOptions { MaxPlayers = 2 }, null);

    // 방 랜덤 참여
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    // 방 떠나기
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("Create Room Failed"); // 방 만들기 실패
    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("Random Room Failed"); // 방 랜덤 참가 실패
    public override void OnCreatedRoom() => Debug.Log("Create Room Success"); // 방 만들기 완료  
    

    // 대기 했다가 상대 플레이어가 들어오면 인원체크해서 게임으로 넘어가는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // 스크립트 이름을 수정하거나 앞에 선언해주기
    {        
        Debug.Log("Wait..."); // 대기 상태

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel("02_GAMESCENE");
        }
    }

    public override void OnJoinedRoom()
    {
        // 플레이어가 2명 이상일 때만 게임 실행
        Debug.Log("OnJoinRoom");

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " Player Count");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Debug.Log("Joined Room"); // 방 참가 완료
            Debug.Log("2명 이상 모였음?" + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("02_GAMESCENE");
        }
    }
    #endregion

    // Room Info
    #region Room Info

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 갯수 : " + PhotonNetwork.CountOfRooms);
            Debug.Log("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("로비에 있는지? : " + PhotonNetwork.InLobby);
            Debug.Log("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion


}
