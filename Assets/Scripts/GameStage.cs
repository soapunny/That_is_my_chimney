using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public struct EnemyData
{
    [SerializeField]
    float spawnTime;
    [SerializeField]
    string enemyId;
    [SerializeField]
    GameObject enemyObj;
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
    List<EnemyData> enemyDatas;
}

[RequireComponent(typeof(BoxCollider))]
public class GameStage : MonoBehaviour
{
    [Header("스테이지 클리어정보")]
    [SerializeField]
    string name;
    [SerializeField]
    int score;
    [SerializeField]
    float clearTime;

    float eventTimer;
    Queue<EnemyGroup> readyEnemyGroups;
    EnemyGroup currGroup;
    List<Enemy> enemys;

    [Header("스테이지 적 스폰정보")]
    [SerializeField]
    List<EnemyGroup> enemyGroups;
    CinemachineDollyCart dollyCart;
    CinemachineBrain cinemachineBrain;

    delegate void killScore(Enemy enemy);

    public bool IsPlay { get; set;}

    // Start is called before the first frame update
    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlay)
        {
            eventTimer += Time.deltaTime;

            if (eventTimer > 3.0f)
            {
                dollyCart.enabled = true;
                IsPlay = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            //
            cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            //cinemachineBrain.ActiveVirtualCamera.Follow = null;
            dollyCart.enabled = false;

            readyEnemyGroups = new Queue<EnemyGroup>(enemyGroups);
            eventTimer = 0f;
            score = 0;
            clearTime = 0;

            IsPlay = true;
        }
    }
}
