using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoneStatus
{
    Stone,
    Destroy
}

public class Stone : MonoBehaviour, IHitable
{
    private GameObject stoneObject;
    public GameObject stoneSocket;
    public MeshRenderer stoneMeshRenderer;
    public MeshCollider stoneCollider;
    public int stoneDurability;
    public StoneStatus stoneStatus;
    public float moveSpeed;
    public TargetHighlight highlight;
    public PlayerController target;
    private bool isMove;
    private float destroyTimer;
    //private float moveTimer;
    [SerializeField]
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        stoneMeshRenderer = GetComponent<MeshRenderer>();
        stoneCollider = GetComponent<MeshCollider>();
        isMove = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        highlight.TargetOn = false;
        //moveTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(stoneDurability <= 0)
        {
            Destroyed();
        }
        if (isMove && stoneStatus == StoneStatus.Stone)
        {
            Move();
            //moveTimer += Time.deltaTime;
            //if(moveTimer >= 3.0f)
            //{
            //    Destroyed();
            //}
            PlayerCollision();
        }
        if(stoneStatus == StoneStatus.Destroy)
        {
            //destroyTimer += Time.deltaTime;
            //if(destroyTimer >= 2.0f)
            //{
            //    destroyTimer = 0.0f;
            //    gameObject.SetActive(false);
            //}
        }
    }
    public void Hit()   // 총에 맞았을 때 상태 구현
    {
        if(stoneDurability >= 1)
        stoneDurability--;
    }

    public void Destroyed()
    {
        if (stoneStatus == StoneStatus.Destroy) return;
        //moveTimer = 0.0f;
        destroyTimer = 0.0f;
        stoneStatus = StoneStatus.Destroy;
        stoneMeshRenderer.enabled = false;
        stoneCollider.enabled = false;
        particle.Play();
        highlight.gameObject.SetActive(false);
    }

    private void PlayerCollision()
    {
        
        if (Vector3.Distance((gameObject.transform.position), (Camera.main.transform.position)) <= 5)
        {
            if (stoneStatus == StoneStatus.Destroy) return;
            target.GetDamage();
            stoneDurability = 0;
            isMove = false;
            EffectManager.Instance.HeartBeat(1.0f);
            EffectManager.Instance.CreateEffect(EffectType.BreakWindow, 1.0f);
            Destroyed();

        }
    }

    public void IsThrow(Vector3 armPosition)
    {
        stoneStatus = StoneStatus.Stone;
        stoneDurability = 3;
        gameObject.transform.position = armPosition;
        stoneMeshRenderer.enabled = true;
        isMove = true;
        highlight.gameObject.SetActive(true);
        highlight.limitTime = Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
        highlight.TargetOn = true;
    }
    private void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.transform.position, moveSpeed * Time.deltaTime);
        if(!stoneCollider.enabled)
        {
            stoneCollider.enabled = true;
        }
        highlight.elapsedTime = highlight.limitTime - Vector3.Distance(gameObject.transform.position, Camera.main.transform.position);
    }
}
