using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public struct EnemyData
{
    public float spawnTime;
    public string enemyId;
    public GameObject enemyObj;
    public EnemyState initState;
    public Vector3 spawnPoint;
    public Vector3 movePoint;
    public float attackSpeed;
}

[Serializable]
public struct EnemyGroup
{
    [SerializeField]
    public List<EnemyData> enemyDatas;
}

[RequireComponent(typeof(BoxCollider))]
public class GameStage : MonoBehaviour
{
    [Header("스테이지 정보")]
    [SerializeField]
    string name;
    [SerializeField]
    int score;
    [SerializeField]
    float clearTime;
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;

    Queue<EnemyGroup> readyEnemyGroups;
    EnemyGroup currGroup;
    List<Enemy> aliveEnemys;

    [Header("생존한 적 정보")]
    [SerializeField]
    List<EnemyGroup> enemyGroups;
    CinemachineDollyCart dollyCart;
    CinemachineBrain cinemachineBrain;

    float eventTimer;
    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        eventTimer = 0.0f;
        isStart = false;
        aliveEnemys = new List<Enemy>();
        virtualCamera.LookAt = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (aliveEnemys.Count == 0)
            {
                virtualCamera.LookAt = transform;
            }
            else
            {
                virtualCamera.LookAt = aliveEnemys[0].transform;
            }

            if (currGroup.enemyDatas.Count == 0)
            {
                // 생존한 적의 수가 0일경우 진행
                if (aliveEnemys.Count == 0)
                {
                    if (!NextEnemyGroup())
                    {
                        dollyCart.enabled = true;
                        virtualCamera.Priority = 0;
                        //cinemachineBrain.ActiveVirtualCamera.LookAt = dollyCart.transform;
                        isStart = false;
                    }
                }
            }
            else
            {
                while (currGroup.enemyDatas.Count != 0 && eventTimer > currGroup.enemyDatas[0].spawnTime)
                {
                    Enemy enemy = Instantiate(currGroup.enemyDatas[0].enemyObj, currGroup.enemyDatas[0].spawnPoint, Quaternion.identity).GetComponent<Enemy>();
                    enemy.transform.LookAt(dollyCart.transform);
                    enemy.state = currGroup.enemyDatas[0].initState;
                    enemy.attackSpeed = currGroup.enemyDatas[0].attackSpeed;
                    enemy.onDeathCallback = new Enemy.OnDeathCallback(KillEnemy);
                    aliveEnemys.Add(enemy);

                    currGroup.enemyDatas.RemoveAt(0);
                }
            }

            eventTimer += Time.deltaTime;
            clearTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            //cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            //cinemachineBrain.ActiveVirtualCamera.Follow = null;
            dollyCart.enabled = false;
            virtualCamera.Priority = 20;
            readyEnemyGroups = new Queue<EnemyGroup>(enemyGroups);
            isStart = NextEnemyGroup();
            clearTime = 0;
            score = 0;
        }
    }

    public void KillEnemy(Enemy enemy)
    {
        score += 5;
        aliveEnemys.Remove(enemy);
    }

    bool NextEnemyGroup()
    {
        aliveEnemys = new List<Enemy>();
        eventTimer = 0f;

        if (readyEnemyGroups.Count == 0)
        {
            currGroup = new EnemyGroup();
            return false;
        }

        currGroup = readyEnemyGroups.Dequeue();
        return true;
    }
}
