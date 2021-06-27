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
    public delegate void OnDeathCallback(Boss boss);

    private float introTimer = 0f;
    private float startTime = 5f;
    private float velocity = 3000f;

    private BossState bossState;
    public Obejct_Key objectKey;
    private Vector3 initPos;
    Rigidbody rigidbody;
    GameObject dollyCart;
    private GameObject mainCamera;
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
        dollyCart = GameObject.Find("BossDollyCart");
        dollyCart.SetActive(false);
    }

    void OnEnable()
    {
        if (bossState == BossState.Idle)
        {
            Debug.LogWarning("이동 ==> " + destPosition);
            nav.SetDestination(destPosition);
        }
        bossState = BossState.RockMode;
        highlight.gameObject.SetActive(true);
        highlight.limitTime = attackSpeed;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossState == BossState.Death) return;

        if(bossState != BossState.RockMode)
            transform.LookAt(mainCamera.transform.position);
        else
            introTimer += Time.deltaTime;
        if (introTimer >= 1f && bossState == BossState.RockMode)
        {
            rigidbody.AddForce(new Vector3(100f, velocity, 0f));
            animator.SetInteger("JumpState", 1);
            bossState = BossState.JumpingUp;
        }
        else if(bossState == BossState.JumpingUp && rigidbody.velocity.y <= 0f)
        {
            bossState = BossState.Falling;
            animator.SetInteger("JumpState", 2);
        }


        if (introTimer >= startTime)
        {
            if (!nav.enabled)
                nav.enabled = true;
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

        if (attackTimer <= 0f)
        {
            if (bossState != BossState.Idle)
            {
                bossState = BossState.Idle;
            }
            animator.SetTrigger("Attack");
            attackTimer = 5f;
        }

        if (bossState == BossState.Idle)
        {
            attackTimer -= Time.deltaTime * attackSpeed;
        }
    }
    void Attack()
    {
        target.GetDamage();
        EffectManager.Instance.HeartBeat(0.5f);
        EffectManager.Instance.CreateEffect(EffectType.ShatteredWindow, 1.0f);
    }

    override
    public void Hit()
    {
        if (bossState == BossState.Death) return;

        //highlight.gameObject.SetActive(false);
        animator.SetTrigger("Hit");
        //bossState = BossState.Death;
        //onDeathCallback(this);
        //Destroy(gameObject, 1.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(bossState == BossState.Falling)
        {
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
}