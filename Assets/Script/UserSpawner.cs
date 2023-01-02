using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.INSTANCE.PlayerSpawn();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
