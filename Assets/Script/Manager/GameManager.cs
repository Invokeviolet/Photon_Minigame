using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


// 전체 데이터 관리
// 출력을 UI매니저가 하게끔

// 몬스터 정보
public enum MOBINFO
{
    EASY,
    NORMAL,
    HARD,
    BOSS
}

// 맵 정보
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
    // 싱글톤
    #region 싱글톤

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

    // 플레이어가 인게임이니?
    public bool myPlayerInGame { get; set; }
    // 다른 윈도우 창이 켜져있나?
    public bool IsWindowOpen { get; set; }
    // 플레이어가 전투중이니?
    public bool myPlayerAction { get; set; }
    // 플레이어가 죽었니?
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

    // 플레이어 생성
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


    public void TITLESCENE() // 타이틀 - 로그인 버튼
    {
        // 카메라 위치
        UIManager.INSTANCE.TITLESCENE();
        myPlayerInGame = false;
        
    }

    public void INGAMESCENE() // 인게임
    {
        UIManager.INSTANCE.ingame();
        myPlayerInGame = true;
        myPlayerAction = false;
        Debug.Log("인게임?");
        Player.INSTANCE.transform.position = Player.INSTANCE.PlayerPos.transform.position;
    }

    public void STORESCENE() // 상점 - OK 버튼
    {
        UIManager.INSTANCE.store();
        UIManager.INSTANCE.OFFQUESTION();
        myPlayerInGame = false;
        myPlayerAction = false;
    }


    public void ACTIONFORESTSCENE() // 전투
    {
        UIManager.INSTANCE.action();
        myPlayerInGame = false;
        myPlayerAction = true;        
    }

    public void GAMERESULTSCENE() // 전투 끝
    {
        Debug.Log("전두 끝!");
    }

    public void GAMECLEARSCENE() // 게임 클리어
    {
        UIManager.INSTANCE.GAMECLEARSCENE();
    }

    public void GAMEOVERSCENE() // 게임 오버
    {
        UIManager.INSTANCE.GAMEOVERSCENE();
    }

    /* public void LOADINGSECNE()
     {
         // 카메라 위치
         SceneManager.LoadScene("10_LOADING_Scene");
         GameManager.INSTANCE.myPlayerInGame = false;
     }*/

    static public void Quit() // 게임 종료
    {
        Application.Quit();
    }


}

