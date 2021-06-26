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

    public GameObject highlightPrefab;
    public TargetHighlight targetHighlight;
    Transform highlightTransform;
    float attackTimer;
    Animator animator;
    Rigidbody rigidbody;
    NavMeshAgent nav;

    private ObjectPool targetPool;

    // Start is called before the first frame update
    void Start()
    {
        targetPool = GameObject.Find("GameManager").GetComponent<ObjectPool>();
        GameObject obj = targetPool.GetObject(Obejct_Key.Target);
        //if(!(obj is TargetHighlight))
        //{
        //    Debug.LogError("[Case Exception] obj는 TargetHighlight의 부모가 아닙니다.");
        //    return;
        //}

        targetHighlight = Instantiate( obj.GetComponent<TargetHighlight>(), transform.position, Quaternion.identity);
        GameObject highlight = targetHighlight.gameObject;
        highlightTransform = highlight.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        if (state == EnemyState.Move || state == EnemyState.MoveSit)
		{
            nav.SetDestination(destPosition);
        }
        attackTimer = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Death) return;

        highlightTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        float scale = 120f / Camera.main.fieldOfView;
        highlightTransform.localScale = new Vector3(scale, scale, 1f);
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
            nav.ResetPath();
            state = EnemyState.Idle;
            rigidbody.MoveRotation(Quaternion.FromToRotation((Camera.main.transform.position - transform.position).normalized, transform.forward));
        }
    }

    public void Hit()
    {
        Death();
    }

    void Death()
    {
        if (state == EnemyState.Death)
            return;

        targetHighlight.enabled = false;
        highlightTransform.gameObject.SetActive(false);
        targetPool.ReleaseObject(Obejct_Key.Target, targetHighlight.gameObject);
        animator.SetTrigger("Death");
        state = EnemyState.Death;
        onDeathCallback(this);
        //Destroy(gameObject, 1.0f);
    }
}
