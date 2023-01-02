using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner_ACTION : MonoBehaviour
{

    // ¸Ê Áö¸é »ý¼º
    [SerializeField] GameObject ActionMapPrefab;

    GameObject MAPCUBE = null;
   
    Monster ActionMob;

    private void Awake()
    {        

    }

    void Start()
    {
        MAPCUBE = Instantiate(ActionMapPrefab, transform.position, Quaternion.identity, transform);       
    }

    void Update()
    {
             
    }

   
}
