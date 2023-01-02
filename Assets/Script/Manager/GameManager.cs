using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


// ��ü ������ ����
// ����� UI�Ŵ����� �ϰԲ�

// ���� ����
public enum MOBINFO
{
    EASY,
    NORMAL,
    HARD,
    BOSS
}

// �� ����
public enum MAPINFO
{
    FOREST,
    SWAMP,
    GROUND,
    SHOP,
    START,
    END
}


public class GameManager : MonoBehaviourPunCallbacks
{
    // �̱���
    #region �̱���

    private static GameManager Instance = null;
    public static GameManager INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<GameManager>();
            }
            return Instance;
        }
    }
    #endregion

    // �÷��̾ �ΰ����̴�?
    public bool myPlayerInGame { get; set; }
    // �ٸ� ������ â�� �����ֳ�?
    public bool IsWindowOpen { get; set; }
    // �÷��̾ �������̴�?
    public bool myPlayerAction { get; set; }
    // �÷��̾ �׾���?
    // public bool myPlayerDead { get; set; }

    Player player;
    Monster monster;
    public Player GetPlayer() => player;
    public Monster GetMonster() => monster;
    
    public int POTION;
    public int GOLD;

    public bool isConnect = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);       
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<Monster>();
       
        TITLESCENE();
    }

    private void Update()
    {
       
    }

    // �÷��̾� ����
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField] private Player playerPrefab;


    public void PlayerSpawn()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            Debug.Log("Master Client Player Instantiate");
        }
        else
        {
            PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        }        
    }


    public void TITLESCENE() // Ÿ��Ʋ - �α��� ��ư
    {
        // ī�޶� ��ġ
        UIManager.INSTANCE.TITLESCENE();
        myPlayerInGame = false;
        
    }

    public void INGAMESCENE() // �ΰ���
    {
        UIManager.INSTANCE.ingame();
        myPlayerInGame = true;
        myPlayerAction = false;
        Debug.Log("�ΰ���?");
        Player.INSTANCE.transform.position = Player.INSTANCE.PlayerPos.transform.position;
    }

    public void STORESCENE() // ���� - OK ��ư
    {
        UIManager.INSTANCE.store();
        UIManager.INSTANCE.OFFQUESTION();
        myPlayerInGame = false;
        myPlayerAction = false;
    }


    public void ACTIONFORESTSCENE() // ����
    {
        UIManager.INSTANCE.action();
        myPlayerInGame = false;
        myPlayerAction = true;        
    }

    public void GAMERESULTSCENE() // ���� ��
    {
        Debug.Log("���� ��!");
    }

    public void GAMECLEARSCENE() // ���� Ŭ����
    {
        UIManager.INSTANCE.GAMECLEARSCENE();
    }

    public void GAMEOVERSCENE() // ���� ����
    {
        UIManager.INSTANCE.GAMEOVERSCENE();
    }

    /* public void LOADINGSECNE()
     {
         // ī�޶� ��ġ
         SceneManager.LoadScene("10_LOADING_Scene");
         GameManager.INSTANCE.myPlayerInGame = false;
     }*/

    static public void Quit() // ���� ����
    {
        Application.Quit();
    }


}

