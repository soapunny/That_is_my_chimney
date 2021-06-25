using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float attackTimer;
    [SerializeField]
    int hp;

    public OnDeathCallback onDeathCallback;

    public GameObject highlightPrefab;
    Transform highlightTransform;
    private Animator ninjaAnimator;
    // Start is called before the first frame update
    void Start()
    {
        GameObject highlight = Instantiate(highlightPrefab);
        highlight.transform.SetParent(FindObjectOfType<Canvas>().transform);
        highlightTransform = highlight.GetComponent<Transform>();
        ninjaAnimator = GetComponent<Animator>();
        if(state == EnemyState.Sit)
        {
            state = EnemyState.Idle;
            ninjaAnimator.SetBool("IsSit", true);
        }
        else
        {
            ninjaAnimator.SetBool("IsSit", false);
        }
        attackTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        highlightTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        float scale = 120f / Camera.main.fieldOfView;
        highlightTransform.localScale = new Vector3(scale, scale, 1f);

        if(state == EnemyState.Idle)
        {
            Attack();
        }
    }

    public void Hit()
    {
        Death();
    }

    void Death()
    {
        if (state == EnemyState.Death) return;

        ninjaAnimator.SetTrigger("Die");
        state = EnemyState.Death;
        onDeathCallback(this);
        Destroy(gameObject, 1.0f);
    }

    void Move()
    {
        // 목표좌표에 도착시 상태 변경
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            ninjaAnimator.SetBool("DoAttack", true);
            state = EnemyState.Attack;
            attackTimer = 0.0f;
        }
    }

    void FinishAttack(int num)
    {
        if(num == 1)
        {
            ninjaAnimator.SetBool("DoAttack", false);
            state = EnemyState.Idle;
        }
    }

    private void OnDestroy()
    {
        Destroy(highlightTransform.gameObject);
    }
}
