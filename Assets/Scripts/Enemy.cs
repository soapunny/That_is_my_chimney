using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Sit,
    Attack
}

public class Enemy : MonoBehaviour
{

    [SerializeField]
    EnemyState state;
    [SerializeField]
    float attackSpeed;
    [SerializeField]
    int hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
