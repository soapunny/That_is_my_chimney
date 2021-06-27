using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    private float introTimer = 0f;
    private float startTime = 5f;
    private float velocity = 3000f;
    private bool isJumping = false;

    public Obejct_Key objectKey;
    private Vector3 initPos;
    NavMeshAgent navAgent;
    Rigidbody rigidbody;
    GameObject rock;
    private GameObject mainCamera;
    //public delegate void OnDeathCallback(Enemy enemy);

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //rigidBody = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        attackTimer = 1.0f;
        navAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        navAgent.enabled = false;
        initPos = transform.position;
        rock = GameObject.Find("Rock");
    }

    void OnEnable()
    {
        if (state == EnemyState.Move || state == EnemyState.MoveSit)
        {
            Debug.LogWarning("이동 ==> " + destPosition);
            nav.SetDestination(destPosition);
        }
        highlight.gameObject.SetActive(true);
        highlight.limitTime = attackSpeed;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Death) return;
        transform.LookAt(mainCamera.transform.position);

        introTimer += Time.deltaTime;
        if (introTimer >= 1f && !isJumping)
        {
            rock.SetActive(false);
            rigidbody.AddForce(new Vector3(100f, velocity, 0f));
            isJumping = true;
        }


        if (introTimer >= startTime)
        {
            if (!navAgent.enabled)
                navAgent.enabled = true;
            Vector3 pos = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    navAgent.SetDestination(hitInfo.point);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //navAgent.for
            }
        }

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
    void Attack()
    {
        EffectManager.Instance.HeartBeat(0.5f);
        EffectManager.Instance.CreateEffect(EffectType.ShatteredWindow, 1.0f);
    }

    public void Hit()
    {
        //if (state == EnemyState.Death) return;

        highlight.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        state = EnemyState.Death;
        //onDeathCallback(this);
        //Destroy(gameObject, 1.0f);
    }
}
