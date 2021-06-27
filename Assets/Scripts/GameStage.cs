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
    CinemachineVirtualCamera virtualCamera;

    Queue<EnemyGroup> readyEnemyGroups;
    EnemyGroup currGroup;
    List<Enemy> aliveEnemys;

    [Header("Stage Enemys")]
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
                        //cinemachineBrain.ActiveVirtualCamera.LookAt = dollyCart.transform;
                        isStart = false;
                        GameManager.gameManager.currStage = null;
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

                    Enemy enemy = obj.GetComponent<Enemy>();
                    enemy.enemyId = currGroup.enemyDatas[0].enemyId;

                    enemy.transform.LookAt(dollyCart.transform);
                    enemy.state = currGroup.enemyDatas[0].initState;
                    enemy.destPosition = currGroup.enemyDatas[0].movePoint;
                    enemy.attackSpeed = currGroup.enemyDatas[0].attackSpeed;
                    enemy.onDeathCallback = new Enemy.OnDeathCallback(KillEnemy);
                    aliveEnemys.Add(enemy);

                    enemy.gameObject.SetActive(true);

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
        //enemy.gameObject.SetActive(false);
        //ObjectPool.Instance.ReleaseObject(enemy.enemyId, enemy.gameObject);
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

    public void GameOver()
    {
        isStart = false;
    }
}
