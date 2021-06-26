using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject dollyCartObject;
    public static GameManager gameManager;
    public Image bulletUi;
    public int maximumBullet;
    public Image hpUi;
    public int maximumHp;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public GameObject gameOver;

    int score;
    float elapsedTime;
    public GameStage currStage;
    public GameStage[] gameStages;

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
        // Score, Clear Time Text Update
        score = 0;
        elapsedTime = 0;
        foreach (var stage in gameStages)
        {
            score += stage.score;
            elapsedTime += stage.clearTime;
        }
        scoreText.text = "score : " + score.ToString("N0");
        timeText.text = "time  : " + elapsedTime.ToString("F2");

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

    public void GameOver()
    {
        gameOver.SetActive(true);
        if (currStage) currStage.GameOver();
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainGame_PHS");
    }
}
