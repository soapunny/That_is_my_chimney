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

    public EnemyState state;
    public float attackSpeed;
    public Vector3 destPosition;

    [SerializeField]
    int hp;

    public OnDeathCallback onDeathCallback;

    public GameObject highlightPrefab;
    Transform highlightTransform;
    float attackTimer;
    Animator animator;
    //Rigidbody rigidBody;
    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        GameObject highlight = Instantiate(highlightPrefab);
        highlight.transform.SetParent(FindObjectOfType<Canvas>().transform);
        highlightTransform = highlight.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        //rigidBody = GetComponent<Rigidbody>();
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
            //nav.ResetPath();
            state = EnemyState.Idle;
            Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
            nav.SetDestination(transform.position + dir * 0.001f);
            //animator.SetBool("FinishMove", true);
            //rigidBody.MoveRotation(Quaternion.FromToRotation((Camera.main.transform.position - transform.position).normalized, transform.forward));
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

        highlightTransform.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        state = EnemyState.Death;
        onDeathCallback(this);
        Destroy(gameObject, 1.0f);
    }
}
