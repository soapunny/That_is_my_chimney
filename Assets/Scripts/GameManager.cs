using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject dollyCartObject;
    public static GameManager gameManager;
    public Image bulletUi;
    public int maximumBullet;
    public Image hpUi;
    public int maximumHp;

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

    public void ChangeHpUi(int currHp)
    {
        float fillAmount = (float)currHp / (float)maximumHp;
        hpUi.fillAmount = fillAmount;
    }
    public void CrossHairUpdate()
    {
        crossHair.transform.position = Input.mousePosition;
    }

    public void ChangeBulletUi(int currBullet)
    {
        float fillAmount = (float)currBullet / (float)maximumBullet;
        bulletUi.fillAmount = fillAmount;
    }
}
