using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Move,
    MoveSit,
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

    public Obejct_Key enemyId;
    public EnemyState state;
    public float attackSpeed;
    public Vector3 destPosition;

    [SerializeField]
    int hp;

    public OnDeathCallback onDeathCallback;

    public TargetHighlight highlight;
    public Player target;

    protected float attackTimer;
    protected Animator animator;
    //Rigidbody rigidBody;
    protected NavMeshAgent nav;

    protected Collider collider;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //rigidBody = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        attackTimer = 1.0f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        collider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        if (state == EnemyState.Move || state == EnemyState.MoveSit)
		{
            Debug.LogWarning("ÀÌµ¿ ==> " + destPosition);
            nav.SetDestination(destPosition);
        }
        highlight.gameObject.SetActive(true);
        highlight.limitTime = attackSpeed;
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Death) return;

        if (attackTimer < 0f)
		{
            if (state == EnemyState.Sit || state == EnemyState.MoveSit)
			{
                state = EnemyState.Idle;
			}
            animator.SetTrigger("Attack");
            attackTimer = 1f;
        }

        animator.SetFloat("MoveSpeed", nav.velocity.magnitude);
        if (state == EnemyState.Sit || state == EnemyState.MoveSit) animator.SetFloat("Sit", 1.0f);
        else animator.SetFloat("Sit", 0.0f);

        if (state == EnemyState.Idle || state == EnemyState.Sit)
		{
            attackTimer -= Time.deltaTime * attackSpeed;
		}

        if (state != EnemyState.Idle && nav.remainingDistance < 0.01f)
		{
            //nav.ResetPath();
            state = EnemyState.Idle;
            Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
            nav.SetDestination(transform.position + dir * 0.1f);
            //animator.SetBool("FinishMove", true);
            //rigidBody.MoveRotation(Quaternion.FromToRotation((Camera.main.transform.position - transform.position).normalized, transform.forward));
        }
    }

    public void Hit()
    {
        Death();
    }

    void Attack()
    {
        target.GetDamage();
        EffectManager.Instance.HeartBeat(0.5f);
        EffectManager.Instance.CreateEffect(EffectType.ShatteredWindow, 1.0f);
    }

    void Death()
    {
        if (state == EnemyState.Death) return;

        animator.SetTrigger("Death");
        nav.ResetPath();
        collider.enabled = false;
        highlight.gameObject.SetActive(false);
        state = EnemyState.Death;
        onDeathCallback(this);
        //Destroy(gameObject, 1.0f);
    }

    void Release()
    {
        gameObject.SetActive(false);
        EffectManager.Instance.CreateEffect(EffectType.NinjaDisappear, 1.5f, transform.position);
        ObjectPool.Instance.ReleaseObject(enemyId, gameObject);
    }
}
