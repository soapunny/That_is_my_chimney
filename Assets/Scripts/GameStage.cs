using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public struct EnemyData
{
    [SerializeField]
    EnemyState initState;
    [SerializeField]
    Vector3 spawnPoint;
    [SerializeField]
    Vector3 movePoint;
    [SerializeField]
    float attackSpeed;
}

[Serializable]
public struct EnemyGroup
{
    [SerializeField]
    string enemyId;
    [SerializeField]
    List<EnemyData> enemyDatas;
}

[RequireComponent(typeof(BoxCollider))]
public class GameStage : MonoBehaviour
{

    [SerializeField]
    List<EnemyGroup> enemyGroups;
    CinemachineDollyCart dollyCart;
    private float cameraTimer;
    private bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        cameraTimer = 0.0f;
        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraTimer();
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            //
            dollyCart.enabled = false;
            isStart = true;
        }
    }

    private void CameraTimer()
    {
        if(isStart)
        {
            cameraTimer += Time.deltaTime;
            if(cameraTimer >= 3.0f)
            {
                cameraTimer = 0.0f;
                isStart = false;
                dollyCart.enabled = true;
            }
        }
    }
}
