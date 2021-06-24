using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Sit,
    Attack,
    Death
}

public interface IHitable
{
    abstract void Hit();
}

public class Enemy : MonoBehaviour, IHitable
{
    public delegate void OnDeathCallback(Enemy enemy);

    public EnemyState state;
    public float attackSpeed;
    [SerializeField]
    int hp;

    public OnDeathCallback onDeathCallback;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Hit()
    {
        Death();
    }

    void Death()
    {
        state = EnemyState.Death;
        onDeathCallback(this);
        Destroy(gameObject, 1.0f);
    }
}
