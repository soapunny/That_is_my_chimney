using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider))]
public class MovieStage : GameStage
{

    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera2;
    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera3;

    [SerializeField]
    GameObject Sleigh;
    private float moveSpeed = 3f;
    private Vector3 desPoint;
    // Start is called before the first frame update

    bool isNextScene = false;

    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        eventTimer = 0.0f;
        isStart = false;
        virtualCamera.LookAt = transform;
        clearTime = 0.0f;
        desPoint = new Vector3(14.18f, 0.778f, -4.55f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {

            Sleigh.transform.position = Vector3.MoveTowards(Sleigh.transform.position, desPoint, moveSpeed * Time.deltaTime);

            if (isNextScene)
            {
                dollyCart.enabled = true;
                virtualCamera.Priority = 0;
                Debug.Log(virtualCamera.name);
            }

            //eventTimer += Time.deltaTime;
            //clearTime += Time.deltaTime;
        }
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
            Debug.Log(virtualCamera.name);
            isStart = true;
            clearTime = 0;
            score = 0;
            Sleigh.SetActive(true);
        }
    }
}
