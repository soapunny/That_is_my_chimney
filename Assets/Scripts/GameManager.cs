using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject dollyCartObject;
    public static GameManager gameManager;


    [SerializeField]
    List<GameObject> bulletImage;
    [SerializeField]
    List<GameObject> hpImage;
    [SerializeField]
    GameObject crossHair;
    // Start is called before the first frame update

    private void Awake()
    {
        Cursor.visible = false;
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        dollyCartObject = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        CrossHairUpdate();
    }

    public void EraseBulletImage(int arrayNum)
    {
        bulletImage[arrayNum].SetActive(false);
    }
    public void ReloadBulletImage(int arrayNum)
    {
        bulletImage[arrayNum].SetActive(true);
    }

    public void MinusHpImage(int currHp)
    {
        hpImage[currHp].SetActive(false);
    }

    public void PlusHpImage(int currHp)
    {
        hpImage[currHp].SetActive(true);
    }

    public void CrossHairUpdate()
    {
        crossHair.transform.position = Input.mousePosition;
    }
}
