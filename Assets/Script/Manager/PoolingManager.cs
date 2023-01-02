using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    Queue<Banana> pool = new Queue<Banana>();

    [SerializeField] GameObject BananaSpawner;
    [SerializeField] Banana BananaPrefab;

    public Banana CreateBanana(Vector3 pos, Quaternion rot)
    {
        Banana instbanana = null;

        if (pool.Count == 0)
        {
            instbanana = Instantiate(BananaPrefab, pos, rot);
            BananaPrefab.gameObject.SetActive(true);

            return instbanana;
        }
        instbanana = pool.Dequeue();
        instbanana.transform.position = pos;
        instbanana.transform.rotation = rot;
        instbanana.gameObject.SetActive(true);

        return instbanana;
    }

    public void DestroyBanana(Banana banana) 
    {
        banana.gameObject.SetActive(false);
        pool.Enqueue(banana);
    }
}
