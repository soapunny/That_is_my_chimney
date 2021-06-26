using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum Obejct_Key // your custom enumeration
{
    NormalEnemy,
    BossEnemy,
    Target
};

[Serializable]
public struct ObjectPoolData
{
    public Obejct_Key objectKey;
    public GameObject ObjectPrefab;
    public int objectCnt;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<Obejct_Key, Queue<GameObject>> mObjectPool;

    [Header("Object Info")]
    [SerializeField]
    List<ObjectPoolData> lObjectPoolDatas = new List<ObjectPoolData>();

    private GameObject gameManager;
    public GameObject GameManager { get => gameManager; set => gameManager = value; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        GameManager = GameObject.Find("GameManager");
        mObjectPool = new Dictionary<Obejct_Key, Queue<GameObject>>();
        for(int j=0;j<lObjectPoolDatas.Count;j++)
        { 
            Queue<GameObject> qObjects = new Queue<GameObject>();

            for (int i= 0;i< lObjectPoolDatas[j].objectCnt; i++)
            {
                qObjects.Enqueue(
                    InitObj(lObjectPoolDatas[j].objectKey, lObjectPoolDatas[j].ObjectPrefab)
                );
            }
            mObjectPool.Add(lObjectPoolDatas[j].objectKey, qObjects);
        }
    }

    private GameObject InitObj(Obejct_Key object_key, GameObject ObjectPrefab)
    {
        GameObject gameobj = Instantiate(ObjectPrefab);
        gameobj.SetActive(false);

        switch (object_key)
        {
            case Obejct_Key.NormalEnemy:
                gameobj.transform.SetParent(GameManager.transform, true);
                break;
            case Obejct_Key.BossEnemy:
                break;
            case Obejct_Key.Target:
                gameobj.transform.SetParent(FindObjectOfType<Canvas>().transform);
                break;
        }
        //gameobj.enabled = false;

        return gameobj;
    }

    public GameObject GetObject(Obejct_Key objectKey)
    {
        if (! mObjectPool.ContainsKey(objectKey))
        {
            Debug.LogError("잘못된 EnemyId 입니다.");
            return null;
        }

        Queue<GameObject> qObjects = mObjectPool[objectKey];
        if (qObjects.Count == 0)
        {
            Debug.LogError("오브젝트풀이 전부 사용 중 입니다.");
            return null;
        }

        GameObject obj = qObjects.Dequeue();
        return obj;
    }

    public void ReleaseObject(Obejct_Key objectKey, GameObject obj)
    {
        if (!mObjectPool.ContainsKey(objectKey))
        {
            Debug.LogError("잘못된 EnemyId 입니다.");
            return;
        }

        mObjectPool[objectKey].Enqueue(obj);
    }
}
