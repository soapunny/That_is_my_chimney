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
    public TextMeshProUGUI reloadText;

    public GameObject gameOver;
    public GameObject victory;
    public GameObject reLoad;

    int score;
    float elapsedTime;
    public GameStage currStage;
    public GameStage[] gameStages;
    public GameObject bossHealthUI;

    [SerializeField]
    GameObject crossHair;
    // Start is called before the first frame update

    public AudioSource bgmPlayer;
    public AudioSource gunSound;
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
        bgmPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            victory.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Victory()
    {
        victory.SetActive(true);
    }

    public void Title()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EraseReload()
    {
        reLoad.SetActive(false);
    }

    public void ShowReload()
    {
        reLoad.SetActive(true);
    }

    public void ViewBossHealth(Boss boss)
	{
        bossHealthUI.GetComponent<BossHealthBar>().target = boss;
        bossHealthUI.SetActive(true);
    }

    public void PlayShotSound()
    {
        gunSound.Play();
    }
}
