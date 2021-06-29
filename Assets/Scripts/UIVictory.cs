using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIVictory : MonoBehaviour
{
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI clearTime;
    public TextMeshProUGUI totalRank;
    public GameObject titleBtn;

    [SerializeField]
    GameObject scoreUiPrefab;
    UIStageScore currScore;
    Coroutine totalScoreUpdate;

    float score;
    int maxScore;
    float time;
    float maxTime;
    bool isTotalUpdate;

    void Start()
    {
        score = 0;
        maxScore = 0;
        time = 0;
        maxTime = 0;
        isTotalUpdate = false;

        StartCoroutine(VictoryView());
    }

    IEnumerator VictoryView()
    {
        GameStage[] stages = GameManager.gameManager.gameStages;
        int height = -220;
        float totalRankScore = 0;
        foreach (var stage in stages)
        {
            currScore = Instantiate(scoreUiPrefab, transform).GetComponent<UIStageScore>();
            currScore.GetComponent<RectTransform>().anchoredPosition = new Vector2(-740f, height);
            currScore.stage = stage;
            height -= 120;
            yield return null;
            while (currScore.IsRunning())
            {
                yield return null;
            }
            if (totalScoreUpdate != null) StopCoroutine(totalScoreUpdate);
            totalScoreUpdate = StartCoroutine(TotalScoreUpdate(currScore.totalScore, stage.clearTime));
            totalRankScore += (int)stage.clearRank;
            yield return new WaitForSeconds(0.3f);
        }

        while (isTotalUpdate) yield return null;

        switch ((int)(totalRankScore / stages.Length))
		{
            case 5:
                totalRank.text = "SS";
                break;
            case 4:
                totalRank.text = "S";
                break;
            case 3:
                totalRank.text = "A";
                break;
            case 2:
                totalRank.text = "B";
                break;
            default:
                totalRank.text = "F";
                break;
		}
        titleBtn.SetActive(true);
        totalRank.gameObject.SetActive(true);
    }

    IEnumerator TotalScoreUpdate(int addScore, float addTime)
    {
        isTotalUpdate = true;
        maxScore += addScore;
        maxTime += addTime;
        Debug.LogWarning(addScore);
        while (score < maxScore || time < maxTime)
        {
            score += 5000 * Time.deltaTime;
            time += 5 * Time.deltaTime;

            if (score > maxScore) score = maxScore;
            if (time > maxTime) time = maxTime;

            totalScore.text = score.ToString("N0");
            clearTime.text = time.ToString("F2");
            yield return null;
        }

        totalScore.text = maxScore.ToString("N0");
        clearTime.text = maxTime.ToString("F2");
        yield return null;

        isTotalUpdate = false;
    }
}
