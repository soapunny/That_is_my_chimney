using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum MovieSceneState
{
    FirstCam, SecondCam, ThirdCam
}


[RequireComponent(typeof(BoxCollider))]
public class MovieStage : GameStage
{

    protected CinemachineClearShot cineCamera;
    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera2;
    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera3;

    private CinemachineBlendDefinition blendDefinition;
    [SerializeField]
    GameObject Sleigh;
    [SerializeField]
    Transform playerNeck;
    Animator playerAnimator;
    private float moveSpeed = 3f;
    private Vector3 desPoint;
    private Vector3 desFlying;
    private float timer = 0.0f;
    private MovieSceneState movieSceneState;
    // Start is called before the first frame update

    bool isNextScene = false;

    void Start()
    {
        cineCamera = GameObject.Find("CineCamera").GetComponent< CinemachineClearShot>();
        player = GameObject.Find("SantaClaus");
        playerAnimator = player.GetComponent<Animator>();
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        eventTimer = 0.0f;
        isStart = false;
        virtualCamera.LookAt = transform;
        clearTime = 0.0f;
        desPoint = new Vector3(14.18f, 0.778f, -4.55f);
        desFlying = new Vector3(23.83f, 6.762f, 2.81f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timer += Time.deltaTime;
            if (timer <= 3f)
            {
                if(movieSceneState == MovieSceneState.FirstCam)
                    PlayFirstScene();
            }
            else if(timer <= 6f)
            {
                if(movieSceneState == MovieSceneState.SecondCam)
                    PlaySecondScene();
            }
            else if (timer <= 9f)
            {
                if (movieSceneState == MovieSceneState.FirstCam)
                {
                    PlayThirdScene();
                }

            }
            else if (timer <= 12f)
            {
                if (movieSceneState == MovieSceneState.ThirdCam)
                {
                    PlayFourthScene();
                }

            }
            else if (timer <= 15f)
            {
                if (movieSceneState == MovieSceneState.ThirdCam)
                {
                    PlayFifthScene();
                }

            }
            else
            {
                isNextScene = true;
            }

            if (isNextScene)
            {
                //cineCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
                virtualCamera.Priority = 0;
                virtualCamera2.Priority = 0;
                virtualCamera3.Priority = 0;
                player.SetActive(false);
                Sleigh.SetActive(false);
                Debug.Log(virtualCamera.name);
                GameManager.gameManager.currStage = null;
                isStart = false;
            }

            //eventTimer += Time.deltaTime;
            //clearTime += Time.deltaTime;
        }
    }

    private void PlayFirstScene()
    {
        Sleigh.transform.position = Vector3.MoveTowards(Sleigh.transform.position, desPoint, moveSpeed * Time.deltaTime);
        player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(19f, 0f, -3.0f), moveSpeed * Time.deltaTime);
        if(Sleigh.transform.position == desPoint)
            movieSceneState = MovieSceneState.SecondCam;
    }
    private void PlaySecondScene()
    {
        virtualCamera.Priority = 0;
        virtualCamera2.Priority = 20;
        movieSceneState = MovieSceneState.FirstCam;
        player.transform.position = new Vector3(11.02189f, -0.3018253f, -5.456385f);
        playerAnimator.SetBool("IsHappy", true);
    }
    private void PlayThirdScene()
    {
        virtualCamera2.Priority = 0;
        virtualCamera.Priority = 20;
        virtualCamera.LookAt = playerNeck.transform;
        movieSceneState = MovieSceneState.ThirdCam;
    }
    private void PlayFourthScene()
    {
        virtualCamera.Priority = 0;
        virtualCamera3.Priority = 20;
        player.transform.position = new Vector3(13.904f, 0.91f, -4.233f);
        player.transform.eulerAngles = new Vector3(-5.705f, -259.688f, -10.032f);
        playerAnimator.SetBool("IsHappy", false);
        playerAnimator.SetBool("IsRiding", true);
    }
    private void PlayFifthScene()
    {
        Sleigh.transform.position = Vector3.MoveTowards(player.transform.position, desFlying, moveSpeed * Time.deltaTime);
        player.transform.position = Vector3.MoveTowards(Sleigh.transform.position, desFlying, moveSpeed * Time.deltaTime);
        virtualCamera3.LookAt = playerNeck;
        dollyCart.enabled = true;
        //playerAnimator.SetBool("IsHappy", false);
        //playerAnimator.SetBool("IsRiding", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            GameManager.gameManager.currStage = this;
            dollyCart.enabled = false;

            //cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            //cinemachineBrain.ActiveVirtualCamera.Follow = null;
            virtualCamera.Priority = 20;
            cineCamera.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;; 
            Debug.Log(virtualCamera.name);
            isStart = true;
            clearTime = 0;
            score = 0;
            Sleigh.SetActive(true);
            timer = 0.0f;
            movieSceneState = MovieSceneState.FirstCam;
        }
    }
}
