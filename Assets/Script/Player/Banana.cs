using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    // 풀 매니저
    PoolingManager poolManager;

    // 자기 자신의 프리팹
    Banana bananaPrefab;

    // 바나나 속도 & 공격력
    float BananaSpeed = 10f;
    // int BananaPower = 10;

    Transform bananaTarget;

    private void Awake()
    {
        poolManager = FindObjectOfType<PoolingManager>();
        bananaPrefab = GetComponent<Banana>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        BananaLaunch();
    }
    // 바나나는 화면 중앙에 있는 과녘 UI 중앙으로만 발사되도록 수정할 것
    public void BananaLaunch()
    {
        transform.Translate(Vector3.forward * BananaSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("Monster")) || (other.gameObject.CompareTag("BossMonster")))
        {
            poolManager.DestroyBanana(bananaPrefab);
        }
        
    }
}
