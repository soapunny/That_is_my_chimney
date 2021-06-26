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
    public List<GameObject> particle;
    public MeshRenderer stoneMeshRenderer;
    public MeshCollider stoneCollider;
    public int stoneDurability;
    public StoneStatus stoneStatus;
    public float moveSpeed;
    public Player target;
    private bool isMove;
    // Start is called before the first frame update
    void Start()
    {
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
        if (isMove)
        {
            Move();
        }
        PlayerCollision();
    }
    public void Hit()   // �ѿ� �¾��� �� ���� ����
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
        int Num = Random.Range(0, 3);
        particle[Num].SetActive(true);
    }

    //ī�޶� �浹 ���� �ʿ�
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
    public void IsThrow()
    {
        isMove = true;
    }
    private void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Camera.main.transform.position, moveSpeed * Time.deltaTime);
    }
}
