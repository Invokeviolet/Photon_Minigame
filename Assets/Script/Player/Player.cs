using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;



public class Player : MonoBehaviourPun
{
    // 싱글톤
    #region 싱글톤

    private static Player Instance = null;
    public static Player INSTANCE
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Player>();
                if (Instance == null)
                {
                    // 플레이어 생성
                    Debug.Log("싱글턴! 플레이어! 생성!");
                    Instance = Instantiate(Resources.Load<Player>("Player"));
                    Instance.name = Instance.name.Replace("(Clone)", "");
                    //Instance = new GameObject("PlayerCam").AddComponent<PlayerCamera>();                   
                }
            }
            // DontDestroyOnLoad(Instance.gameObject);
            return Instance;
        }
    }
    #endregion


    MonsterMarble monsterMarble;

    // 플레이어 위치 받아오기
    
    public Transform PlayerPos;
    public Transform ActionPlayerPos;

    Cube cube;
    // 플레이어 죽음 체크
    public bool IsDead { get; set; }
    // 플레이어 점프 힘
    float JumpForce;

    // 플레이어 리지드바디
    private Rigidbody PlayerRB;

    // 플레이어 정보 
    //
    // Player Info Property
    #region Player Info Property

    // 속도 
    public int PlayerSpeed { get; set; }
    // 레벨

    public int PlayerLevel { get; set; }

    // 체력
    public int PlayerHP { get; set; }

    // 경험치
    public int PlayerEXP { get; set; }

    // 공격력
    public int PlayerAttackPower { get; set; }

    // 공격범위
    public int PlayerAttackRange { get; set; }

    // 물약
    public int PlayerPotion { get; set; }

    // 골드
    private int playerGold;
    public int PlayerGold
    {
        get
        {
            if (playerGold <= 0)
            {
                playerGold = 0;
                Shop4BuyAvailable = false;
            }
            return playerGold;
        }
        set
        {
            playerGold = value;
        }

    }

    private int PlayerMaxHP = 100;
    private int PlayerMaxEXP = 1000;

    public float CURSTAMINAR { get; set; }
    public float MAXSTAMINAR { get; set; }

    public bool Shop4BuyAvailable { get; set; }

    #endregion


    private void Awake()
    {
        monsterMarble = FindObjectOfType<MonsterMarble>();
       
        PlayerRB = this.gameObject.GetComponent<Rigidbody>();
        Shop4BuyAvailable = true;

        PlayerSpeed = 8;

        PlayerLevel = 1;

        PlayerMaxHP = 100;
        PlayerHP = 1 / PlayerMaxHP;
        PlayerAttackPower = 1;
        PlayerAttackRange = 1;
        PlayerEXP = 0;
        PlayerMaxEXP = 1000;
        PlayerPotion = 1;
        playerGold = 100;
        //CURSTAMINAR = 0;
        //MAXSTAMINAR = 0.5f;
    }


    // 플레이어 레벨 UI
    string MyLV = "";
    // 플레이어 공격력 UI
    string MyAttack = "";


    private void Start()
    {
        // 플레이어 시작 위치
        PlayerPos.transform.position = new Vector3(0, 0.5f, 0);
        this.transform.position = PlayerPos.transform.position;

        PlayerRB = GetComponent<Rigidbody>();
        if (PlayerRB == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
    }


    // Update
    #region Update
    private void Update()
    {
        MyLV = "LV ." + PlayerLevel.ToString();
        MyAttack = PlayerAttackPower.ToString() + " / " + PlayerAttackRange.ToString();

        // 인게임 씬이고, 어떤 알림창도 뜨지 않았을 때
        if ((GameManager.INSTANCE.myPlayerInGame == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            PlayerMoveToIngame();
        }
        // 전투 씬이고, 어떤 알림창도 뜨지 않았을 때
        if ((GameManager.INSTANCE.myPlayerAction == true) && (GameManager.INSTANCE.IsWindowOpen == false))
        {
            // 인게임씬 플레이어가 이동되는 문제
            PlayerMoveToAction();
        }
        PlayerCharge(); // 물약 먹었을 때
        UpdatePlayerInfo(); // UI 업데이트
    }
    #endregion 




    private void UpdatePlayerInfo() // 나중에 안쓰면 삭제
    {
        // 플레이어 정보 업데이트 시점 - 전투씬 전,후
        UIManager.INSTANCE.GETPLAYERINFO(MyLV, MyAttack);
    }



    //
    // 플레이어 경험치 및 레벨업
    #region 플레이어 경험치 및 레벨업
    public void ExpUpdate(int exp) // 몬스터 잡는 곳에서 호출
    {
        UIManager.INSTANCE.Player_Exp.value += exp;
        if (PlayerEXP >= PlayerMaxEXP)
        {
            // 경험치가 최대일 때 레벨업시 능력 증가
            PlayerLevel += 1;
            PlayerEXP = 0;
            PlayerMaxHP = PlayerMaxHP * 2;
            PlayerHP = PlayerMaxHP;
            PlayerAttackPower = PlayerAttackPower * 2;
        }
    }
    #endregion
    //


    //
    // 플레이어 인게임 이동
    #region 플레이어 인게임 이동
    void PlayerMoveToIngame()
    {
        // WASD 키로 이동
        if (Input.GetKeyDown((KeyCode.T))) // 앞
        {
            transform.position += Vector3.forward;
            if (transform.position.z >= 9)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 9);
            }
        }
        else if (Input.GetKeyDown((KeyCode.F))) // 왼 
        {
            transform.position += Vector3.left;
            if (transform.position.x <= 0)
            {
                transform.position = new Vector3(0, 0.5f, transform.position.z);
            }
        }
        else if (Input.GetKeyDown((KeyCode.G))) // 뒤
        {
            transform.position += Vector3.back;
            if (transform.position.z <= 0)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, 0);
            }
        }
        else if (Input.GetKeyDown((KeyCode.H))) // 오른
        {
            transform.position += Vector3.right;
            if (transform.position.x >= 9)
            {
                transform.position = new Vector3(9, 0.5f, transform.position.z);
            }
        }

    }
    #endregion

    
    //
    //
    // 플레이어 전투씬 이동
    #region 플레이어 전투씬 이동

    protected float hAxis = 0f;
    protected float vAxis = 0f;
    protected bool isMove = false;
    protected Vector3 moveDir;

    public float speed = 2.5f;
    private float xRotate = 0.0f;
    private float yRotate = 0.0f;

    void PlayerMoveToAction()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // 좌우이동 * 속도
        float yRotateSize = Input.GetAxis("Mouse X") * speed;
        // y축 회전값 + 새로운 회전각도
        yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래이동 * 속도
        float xRotateSize = -Input.GetAxis("Mouse Y") * speed;

        // 카메라 회전량 = 카메라에 반영
        transform.eulerAngles = new Vector3(0, yRotate, 0);

        // 위아래 회전량 + 새로운 회전각도, 제한 범위
        // Clamp 값의 범위를 제한 -> min, max
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -40, 60);

        // 카메라
        Camera.main.transform.eulerAngles = new Vector3(xRotate, 0, 0);        

        // isMove는 hAxis가 0의 근사값일 때 false가 됨.
        isMove = !Mathf.Approximately(hAxis, 0f) || !Mathf.Approximately(vAxis, 0f);

        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (isMove)
        {            
            transform.Translate(moveDir * Time.deltaTime * PlayerSpeed, Space.Self);
        }

        // 점프
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 달리기
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
        
    }
    void Jump()
    {
        JumpForce = 1.5f;
        PlayerRB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
    void Run()
    {
        PlayerSpeed = PlayerSpeed * 2;
    }

    #endregion
    //



    // 플레이어 체력
    #region
    void PlayerCharge()
    {
        int curPotion = PlayerPotion;
        if (Input.GetKeyDown((KeyCode.I)))
        {
            PlayerPotion -= 1;
            PlayerHP += 10;

            if (PlayerPotion <= 0)
            {
                PlayerPotion = 0;
            }
            if (PlayerHP >= PlayerMaxHP) // HP가 이미 꽉차있을 때 물약 사용 막기
            {
                PlayerHP = PlayerMaxHP;
                PlayerPotion = curPotion;
            }
        }
    }
    #endregion

    //
    // 플레이어 위치 정보 출력
    #region 플레이어 위치 정보 출력

    private void OnCollisionEnter(Collision collision)
    {
        // 맵에 있는 맵큐브랑 충돌하면 맵 정보 출력
        if (collision.gameObject.TryGetComponent<Cube>(out cube) == true) // (collision.collider.CompareTag("Cube"))
        {
            Debug.Log(cube.MAPINFO);
            UIManager.INSTANCE.GETMAPINFO(cube.MAPINFO); // UI에 맵 정보 출력
        }
        if (collision.collider.CompareTag("Store"))
        {
            // 상점으로 이동할건지 물어보는 창 띄우기
            UIManager.INSTANCE.ONQUESTION();
        }
        if (collision.collider.CompareTag("NPC"))
        {
            Debug.Log("안녕 NPC");
        }
        if (collision.collider.CompareTag("End"))
        {
            Debug.Log("도착!");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<MonsterMarble>(out monsterMarble) == true) //(collision.collider.CompareTag("Monster"))
        {
            PlayerPos.transform.position = transform.position;
            ActionPlayerPos.transform.position = new Vector3(2000, 0.5f, 0);
            transform.position = ActionPlayerPos.transform.position;

            // 충돌했을 때 플레이어 액션 위치로 이동
            Debug.Log("플레이어 공격 준비 위치로 이동 완료");
            UIManager.INSTANCE.GETMOBINFO(monsterMarble); // UI에 몬스터 정보 출력

            // 카메라 위치 이동
            GameManager.INSTANCE.ACTIONFORESTSCENE();
        }
    }

    #endregion


    //
    // 플레이어 타격 및 죽음
    #region 플레이어 타격 및 죽음
    public void HitPlayer(int damage)
    {
        PlayerHP -= damage;
        if (PlayerHP <= 0)
        {
            IMDEAD();
        }
    }
    public void IMDEAD()
    {
        IsDead = true;
        UIManager.INSTANCE.Check4WhoIsWin(true);
        // 1 초 뒤 플레이어 졌다는 창 띄우기
        // Invoke("UIManager.INSTANCE.RESULTSCENE()", 1f);

    }
    #endregion
}
