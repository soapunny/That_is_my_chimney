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

    // Start is called before the first frame update
    void Start()
    {
        stoneMeshRenderer = GetComponent<MeshRenderer>();
        stoneCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stoneDurability <= 0)
        {
            Destroyed();
        }
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
}
