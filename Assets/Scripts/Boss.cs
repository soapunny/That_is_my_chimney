using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum BossState
{
    RockMode, JumpingUp, Falling, Roaring, Idle, Attack, GetHit, Death
}

public class Boss : Enemy
{
    public delegate void OnBossDeathCallback(Boss boss);
    public OnBossDeathCallback onBossDeathCallback;

    private float introTimer = 0f;
    private float startTime = 5f;
    private float velocity = 3000f;

    private BossState bossState;
    private Vector3 initPos;
    Rigidbody rigidbody;
    GameObject dollyCart;
    private GameObject mainCamera;
    private GameObject stone;
    public GameObject stonePrefab;
    public GameObject handPosition;
    public BossState BossState { get => bossState; set => bossState = value; }

    //public delegate void OnDeathCallback(Enemy enemy);

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        attackTimer = 1.0f;
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        nav.enabled = false;
        initPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        collider = GetComponent<CapsuleCollider>();
        dollyCart = GameObject.Find("BossDollyCart");
        dollyCart.SetActive(false);
    }

    void OnEnable()
    {
        if (bossState == BossState.Idle)
        {
            Debug.LogWarning("이동 ==> " + destPosition);
            //nav.SetDestination(destPosition);
        }
        bossState = BossState.RockMode;
        highlight.gameObject.SetActive(true);
        highlight.limitTime = attackSpeed;
        collider.enabled = true;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossState == BossState.Death) return;

        //if(bossState != BossState.RockMode)
            transform.LookAt(mainCamera.transform.position);
        //else
            introTimer += Time.deltaTime;
        if (introTimer >= 3f && bossState == BossState.RockMode)
        {
            rigidbody.AddForce(new Vector3(-1000f, velocity, -1000f));
            animator.SetInteger("JumpState", 1);
            bossState = BossState.JumpingUp;
        }
        else if(bossState == BossState.JumpingUp && rigidbody.velocity.y <= 0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 1, Vector3.down, out hit, 6.4f, LayerMask.GetMask("Environment")))
             {
                bossState = BossState.Falling;
                animator.SetInteger("JumpState", 2);
             }
        }


        if (introTimer >= startTime)
        {
            Vector3 pos = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    nav.SetDestination(hitInfo.point);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //nav.for
            }
        }

        if (bossState == BossState.Idle)
        {
            attackTimer -= Time.deltaTime * attackSpeed;
            if (attackTimer <= 0f)
            {
                animator.SetTrigger("Attack");
                bossState = BossState.Attack;
            }
        }
    }

    public void Attack()
    {
        stone.GetComponent<Stone>().IsThrow(handPosition.transform.position);
        mainCamera.transform.LookAt(stone.transform);
        attackTimer = 5f;
        //target.GetDamage();
        //EffectManager.Instance.HeartBeat(0.5f);
        //EffectManager.Instance.CreateEffect(EffectType.ShatteredWindow, 1.0f);
    }

    override
    public void Hit()
    {
        if (bossState != BossState.Idle) return;

        //highlight.gameObject.SetActive(false);
        animator.SetTrigger("Hit");
        bossState = BossState.GetHit;
        if (--hp <= 0)
        {
            Death();
        }
        //bossState = BossState.Idle;
        //Destroy(gameObject, 1.0f);
    }

    public void InitState()
    {
        bossState = BossState.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(bossState == BossState.Falling)
        {
            rigidbody.isKinematic = true;
            if (!nav.enabled)
                nav.enabled = true;
            stone = Instantiate(stonePrefab) as GameObject;
            stone.transform.SetParent(transform, false);
            bossState = BossState.Idle;
            animator.SetInteger("JumpState", 0);
            dollyCart.SetActive(true);
        }
    }

    void Release()
    {
        gameObject.SetActive(false);
        EffectManager.Instance.CreateEffect(EffectType.NinjaDisappear, 1.5f, transform.position);
        ObjectPool.Instance.ReleaseObject(enemyId, gameObject);
    }
    void Death()
    {
        if (bossState == BossState.Death) return;

        animator.SetTrigger("Die");
        nav.ResetPath();
        collider.enabled = false;
        highlight.gameObject.SetActive(false);
        bossState = BossState.Death;
        onBossDeathCallback(this);
        //Destroy(gameObject, 1.0f);
    }
}
