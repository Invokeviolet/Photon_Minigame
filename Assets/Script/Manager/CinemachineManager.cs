using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera TitleCamera;
    [SerializeField] CinemachineVirtualCamera IngameCamera;
    [SerializeField] CinemachineVirtualCamera ActionCamera;
    [SerializeField] CinemachineVirtualCamera StoreCamera;


    private void Awake()
    {        
       
    }
    private void Start()
    {
        OnTitleCamera();
        UIManager.INSTANCE.ingame += OnIngameCamera;
        UIManager.INSTANCE.action += OnActionCamera;
        UIManager.INSTANCE.store += OnStoreCamera;
    }

    private void Update()
    {

    }

    // 처음 카메라 위치는 타이틀 맵 위치에서 시작
    public void OnTitleCamera()
    {
        TitleCamera.MoveToTopOfPrioritySubqueue();
    }

    // 로그인 버튼을 누르면 인게임 씬 위치로 이동

    public void OnIngameCamera() 
    {
        IngameCamera.MoveToTopOfPrioritySubqueue();
    }
    public void MoveIngameScene() 
    { 
        // 인게임씬에서 플레이어는 항상 카메라의 중심위치에 위치하도록 한다.
        
    }

    // 플레이어가 전투씬으로 이동하면 전투씬 카메라를 켜고
    public void OnActionCamera() 
    {
        ActionCamera = FindObjectOfType<Player>().GetComponentInChildren<CinemachineVirtualCamera>();
        ActionCamera.MoveToTopOfPrioritySubqueue();
    }

    public void OnStoreCamera()
    {
        StoreCamera.MoveToTopOfPrioritySubqueue();
    }
}
