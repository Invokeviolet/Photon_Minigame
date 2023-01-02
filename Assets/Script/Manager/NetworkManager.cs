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

    // ȭ�� ũ�� ����
    private void Awake()
    {        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Setting();
    }

    // ���� �ؽ�Ʈ ������Ʈ
    private void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    // Setting
    #region Setting
    private void Setting()
    {
        // ������ �⺻ ����
        Screen.SetResolution(1920, 1080, false);

        // ��� ��Ʈ��ũ ���⿡ ���� ��Ʈ��ũ ������Ʈ�� �⺻ ���� �ӵ�
        PhotonNetwork.SendRate = 60;

        // PhotonView ���� OnPhotonSerialize�� �ʴ� ��ȸ ȣ������
        PhotonNetwork.SerializationRate = 30;

        // ������ �������� ����� ��ȯ�� �� �ٸ� Ŭ���̾�Ʈ�� ����� �ڵ����� ����ȭ�մϴ�.
        PhotonNetwork.AutomaticallySyncScene = true;

        // Don't get a photonmessage when access the room.
        PhotonNetwork.IsMessageQueueRunning = false;
    }
    #endregion

    // Network
    #region Network

    // ���� ���� - ��ư ������ ��
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // ���� ���� ����
    public override void OnConnectedToMaster()
    {
        Debug.Log("Server Join Success");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    // ���� ���� ����
    public void Disconnected() => PhotonNetwork.Disconnect();
    // ���� ���� ���� ����? �޾ƿ�
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("Server Disconnected");

    #endregion

    // �κ� ����
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby"); // �κ� ���� �Ϸ�
        PhotonNetwork.LoadLevel("01_LOBBYSCENE");

    }

    // Room
    #region Room

    // �� ����
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text, new RoomOptions { MaxPlayers = 2 });

    // �� ����
    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomInput.text);

    // public override void OnJoinedRoom() => Debug.Log("Joined Room"); // �� ���� �Ϸ�

    // �� ���� �Ǵ� ����
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom("TEST", new RoomOptions { MaxPlayers = 2 }, null);

    // �� ���� ����
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    // �� ������
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("Create Room Failed"); // �� ����� ����
    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("Random Room Failed"); // �� ���� ���� ����
    public override void OnCreatedRoom() => Debug.Log("Create Room Success"); // �� ����� �Ϸ�  
    

    // ��� �ߴٰ� ��� �÷��̾ ������ �ο�üũ�ؼ� �������� �Ѿ�� �Լ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // ��ũ��Ʈ �̸��� �����ϰų� �տ� �������ֱ�
    {        
        Debug.Log("Wait..."); // ��� ����

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel("02_GAMESCENE");
        }
    }

    public override void OnJoinedRoom()
    {
        // �÷��̾ 2�� �̻��� ���� ���� ����
        Debug.Log("OnJoinRoom");

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " Player Count");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Debug.Log("Joined Room"); // �� ���� �Ϸ�
            Debug.Log("2�� �̻� ����?" + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("02_GAMESCENE");
        }
    }
    #endregion

    // Room Info
    #region Room Info

    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("�� ���� : " + PhotonNetwork.CountOfRooms);
            Debug.Log("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            Debug.Log("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion


}
