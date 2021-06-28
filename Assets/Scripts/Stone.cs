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
    public Player target;
    private bool isMove;
    private float destroyTimer;
    [SerializeField]
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        stoneMeshRenderer = GetComponent<MeshRenderer>();
        stoneCollider = GetComponent<MeshCollider>();
        isMove = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        }
        PlayerCollision();
        if(stoneStatus == StoneStatus.Destroy)
        {
            destroyTimer += Time.deltaTime;
            if(destroyTimer >= 2.0f)
            {
                destroyTimer = 0.0f;
                gameObject.SetActive(false);
            }
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
        stoneStatus = StoneStatus.Destroy;
        stoneMeshRenderer.enabled = false;
        stoneCollider.enabled = false;
        particle.Play();
    }

    private void PlayerCollision()
    {
        if (gameObject.transform.position == Camera.main.transform.position)
        {
            if (stoneStatus == StoneStatus.Destroy) return;
            target.GetDamage();
            stoneDurability = 0;
            stoneStatus = StoneStatus.Destroy;
            isMove = false;
        }
    }

    public void IsThrow(Vector3 armPosition)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = armPosition;
        stoneMeshRenderer.enabled = true;
        isMove = true;
    }
    private void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.transform.position, moveSpeed * Time.deltaTime);
        if(!stoneCollider.enabled)
        {
            stoneCollider.enabled = true;
        }
    }
}
