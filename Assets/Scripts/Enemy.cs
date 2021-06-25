using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Move,
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
    [SerializeField]
    int hp;

    public OnDeathCallback onDeathCallback;

    public GameObject highlightPrefab;
    Transform highlightTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameObject highlight = Instantiate(highlightPrefab);
        highlight.transform.SetParent(FindObjectOfType<Canvas>().transform);
        highlightTransform = highlight.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        highlightTransform.position = Camera.main.WorldToScreenPoint(transform.position);
        float scale = 120f / Camera.main.fieldOfView;
        highlightTransform.localScale = new Vector3(scale, scale, 1f);
    }

    public void Hit()
    {
        Death();
    }

    void Death()
    {
        state = EnemyState.Death;
        onDeathCallback(this);
        Destroy(gameObject, 1.0f);
    }

    private void OnDestroy()
    {
        Destroy(highlightTransform.gameObject);
    }
}
