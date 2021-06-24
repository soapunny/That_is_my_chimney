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
    [Header("스테이지 클리어정보")]
    [SerializeField]
    string name;
    [SerializeField]
    int score;
    [SerializeField]
    float clearTime;

    Queue<EnemyGroup> readyEnemyGroups;
    EnemyGroup currGroup;
    List<Enemy> aliveEnemys;

    [Header("스테이지 적 스폰정보")]
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
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (aliveEnemys.Count == 0)
            {
                cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            }
            else
            {
                cinemachineBrain.ActiveVirtualCamera.LookAt = aliveEnemys[0].transform;
            }

            if (currGroup.enemyDatas.Count == 0)
            {
                // 현재 그룹의 남은 대기열이 다 스폰되었다.
                if (aliveEnemys.Count == 0)
                {
                    // 남아있는 적이 없다
                    // 다음 그룹 실행
                    if (!NextEnemyGroup())
                    {
                        // 다음 그룹이 없다
                        // 스테이지 종료
                        dollyCart.enabled = true;
                        cinemachineBrain.ActiveVirtualCamera.LookAt = dollyCart.transform;
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
            cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            //cinemachineBrain.ActiveVirtualCamera.Follow = null;
            dollyCart.enabled = false;

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
