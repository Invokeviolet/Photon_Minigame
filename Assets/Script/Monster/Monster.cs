using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public int CurMobCount { get; set; }
    public int MaxMobCount = 10;
    public bool IsDead { get; set; }


    MOBINFO mobInfo;

    // 체력 받아오기
    public int HP { get; set; }

    // 몹 정보 
    public MOBINFO MOBINFO { get { return mobInfo; } set { mobInfo = value; } }

    private void Start()
    {
        CurMobCount = MaxMobCount;
    }
    private void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Banana"))
        {
            HP -= Player.INSTANCE.PlayerAttackPower;
            if (HP <= 0)
            {
                DeadMonster();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기
            int rndDamage = Random.Range(0, 10);
            GameManager.INSTANCE.GetPlayer().PlayerHP -= rndDamage;
        }
    }
    public void DeadMonster()
    {
        IsDead = true;
        Destroy(this.gameObject);
        UIManager.INSTANCE.Check4WhoIsWin(false);

        CurMobCount--;
        if (CurMobCount == 0)
        {
            UIManager.INSTANCE.Check4WhoIsWin(true);
            UIManager.INSTANCE.RESULTSCENE(); // 다 잡았다! 결과창 띄워줌
        }
        // 플레이어 경험치 증가
        Player.INSTANCE.ExpUpdate(10);
    }

    // 몬스터는 생성되면 플레이어를 찾아 이동한다.
    // 타겟, 이동 스피드, 이동 방향    
    // NONE, IDLE, MOVE, TRACKING, ATTACK, DIE
}

