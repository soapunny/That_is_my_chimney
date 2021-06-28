using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public struct EnemyData
{
    public float spawnTime;
    public Obejct_Key enemyId;
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
    [Header("Stage Info")]
    public string name;
    public int score;
    public float clearTime;
    [SerializeField]
    protected CinemachineVirtualCamera virtualCamera;

    protected Queue<EnemyGroup> readyEnemyGroups;
    protected EnemyGroup currGroup;
    protected List<Enemy> aliveEnemys;

    [Header("Stage Enemys")]
    [SerializeField]
    protected List<EnemyGroup> enemyGroups;
    protected CinemachineDollyCart dollyCart;
    protected CinemachineBrain cinemachineBrain;

    [SerializeField]
    protected CinemachineDollyCart bossDolly;

    protected float eventTimer;
    protected private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        eventTimer = 0.0f;
        isStart = false;
        aliveEnemys = new List<Enemy>();
        virtualCamera.LookAt = transform;
        clearTime = 0.0f;
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
                if (aliveEnemys.Count == 0)
                {
                    if (!NextEnemyGroup())
                    {
                        // empty wait enemys
                        dollyCart.enabled = true;
                        virtualCamera.Priority = 0;
                        Debug.Log(virtualCamera.name);
                        //virtualCamera.gameObject.SetActive(false);
                        //cinemachineBrain.ActiveVirtualCamera.LookAt = dollyCart.transform;
                        GameManager.gameManager.currStage = null;
                        isStart = false;
                    }
                }
            }
            else
            {
                while (currGroup.enemyDatas.Count != 0 && eventTimer > currGroup.enemyDatas[0].spawnTime)
                {
                    GameObject obj = ObjectPool.Instance.GetObject(currGroup.enemyDatas[0].enemyId);
                    //if (!(obj is Enemy))
                    //{
                    //    Debug.LogError("[CastException] obj가 Enemy의 부모가 아닙니다");
                    //    break;
                    //}
                    obj.transform.position = currGroup.enemyDatas[0].spawnPoint;

                    Enemy enemy;
                    if (currGroup.enemyDatas[0].enemyId == Obejct_Key.BossEnemy)
                    {
                        enemy = obj.GetComponent<Boss>();
                        Boss boss = (enemy as Boss);
                        boss.onBossDeathCallback = new Boss.OnBossDeathCallback(KillBoss);
                        boss.BossState = BossState.RockMode;
                        boss.enemyId = currGroup.enemyDatas[0].enemyId;

                        boss.transform.LookAt(dollyCart.transform);
                        boss.destPosition = currGroup.enemyDatas[0].movePoint;
                        boss.attackSpeed = currGroup.enemyDatas[0].attackSpeed;
                        aliveEnemys.Add(boss);

                        boss.gameObject.SetActive(true);
                        boss.enabled = true;
                    }
                    else
                    {
                        enemy = obj.GetComponent<Enemy>();
                        enemy.onDeathCallback = new Enemy.OnDeathCallback(KillEnemy);
                        enemy.State = currGroup.enemyDatas[0].initState;
                        enemy.enemyId = currGroup.enemyDatas[0].enemyId;

                        enemy.transform.LookAt(dollyCart.transform);
                        enemy.destPosition = currGroup.enemyDatas[0].movePoint;
                        enemy.attackSpeed = currGroup.enemyDatas[0].attackSpeed;
                        aliveEnemys.Add(enemy);

                        enemy.gameObject.SetActive(true);
                        enemy.enabled = true;
                    }

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
            GameManager.gameManager.currStage = this;
            dollyCart.enabled = false;
            if(GameManager.gameManager.currStage.name == "BossStage")
            {
                dollyCart = bossDolly;
            }
            //cinemachineBrain.ActiveVirtualCamera.LookAt = transform;
            //cinemachineBrain.ActiveVirtualCamera.Follow = null;
            virtualCamera.Priority = 20;
            Debug.Log(virtualCamera.name);
            readyEnemyGroups = new Queue<EnemyGroup>(enemyGroups);
            isStart = NextEnemyGroup();
            clearTime = 0;
            score = 0;
        }
    }

    public void KillEnemy(Enemy enemy)
    {
        score += 5;
        //enemy.gameObject.SetActive(false);
        //ObjectPool.Instance.ReleaseObject(enemy.enemyId, enemy.gameObject);
        aliveEnemys.Remove(enemy);
    }

    public void KillBoss(Boss boss)
    {
        score += 20;
        //enemy.gameObject.SetActive(false);
        //ObjectPool.Instance.ReleaseObject(enemy.enemyId, enemy.gameObject);
        aliveEnemys.Remove(boss);
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

    public void GameOver()
    {
        isStart = false;
    }
}
