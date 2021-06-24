using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Serializable]
public struct EnemyData
{
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

    [SerializeField]
    List<EnemyGroup> enemyGroups;
    CinemachineDollyCart dollyCart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            //
            dollyCart.enabled = false;
        }
    }
}
