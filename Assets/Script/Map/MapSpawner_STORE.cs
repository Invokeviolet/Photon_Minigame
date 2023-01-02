using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner_STORE : MonoBehaviour
{

    [SerializeField] GameObject Merchant;    
    [SerializeField] GameObject StoreMapPrefab;

    GameObject StoreMap;

    private void Start()
    {
        StoreMap = Instantiate(StoreMapPrefab, transform.position, Quaternion.identity, transform);
    }

}
