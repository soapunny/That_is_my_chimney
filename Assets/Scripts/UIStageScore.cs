using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStageScore : MonoBehaviour
{
    enum State
    {
        Start,
        UpdateStart,
        UpdateEnd,
        End
    }

    public GameStage stage;
    public TextMeshProUGUI stageLabel;
    public TextMeshProUGUI bestRank;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI score;
    public TextMeshProUGUI mul;
    public TextMeshProUGUI rankBonus;
    public TextMeshProUGUI clearRank;
    public TextMeshProUGUI clearTime;
    public GameObject newRecord;

    State state;
    public int totalScore;

    // Start is called before the first frame update
    void Start()
    {
        stageLabel.text = stage.name;
        float time = PlayerPrefs.GetFloat(stage.name + "_BestTime");
        float lastSpawn = PlayerPrefs.GetFloat(stage.name + "_LastSpawn");
        Debug.Log(time + " / " + lastSpawn);
        if (time < lastSpawn + 0.5f)
        {
            bestRank.text = "SS";
        }
        else if (time < lastSpawn + 1.0f)
        {
            bestRank.text = "S";
        }
        else if (time < lastSpawn + 2.0f)
        {
            bestRank.text = "A";
        }
        else if (time < lastSpawn + 3.0f)
        {
            bestRank.text = "B";
        }
        else
        {
            bestRank.text = "F";
        }
        bestTime.text = time.ToString("F2");
        state = State.Start;
        StartCoroutine(StageScoreView());

        totalScore = 0;
    }

    IEnumerator StageScoreView()
    {
        state = State.Start;

        // 스코어 업데이트
        score.gameObject.SetActive(true);
        StartCoroutine(ScoreUpdate());
        yield return null;
        while (state == State.UpdateStart)
        {
            yield return null;
        }

        // clearTime Update
        clearTime.gameObject.SetActive(true);
        StartCoroutine(ClearTimeUpdate());
        yield return null;
        while (state == State.UpdateStart)
        {
            yield return null;
        }

        state = State.End;
    }

    IEnumerator ScoreUpdate()
    {
        state = State.UpdateStart;
        float elapsedTime = 0;
        int gameScore;
        while (elapsedTime < 1.0f)
        {
            gameScore = (int)Mathf.Lerp(0, stage.score, elapsedTime);
            score.text = gameScore.ToString("N0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        score.text = stage.score.ToString("N0");
        state = State.UpdateEnd;
    }

    IEnumerator ClearTimeUpdate()
    {
        state = State.UpdateStart;
        float elapsedTime = 0;
        float time = 0;
        while (elapsedTime < 1.0f)
        {
            time = Mathf.Lerp(0, stage.clearTime, elapsedTime);
            clearTime.text = time.ToString("F2");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        clearTime.text = stage.clearTime.ToString("F2");
        yield return new WaitForSeconds(0.2f);

        // rank
        float lastSpawn = stage.lastSpawnTime;
        int rank = 0;
        if (time < lastSpawn + 0.5f)
        {
            clearRank.text = "SS";
            rank = 500;
        }
        else if (time < lastSpawn + 1.0f)
        {
            clearRank.text = "S";
            rank = 400;
        }
        else if (time < lastSpawn + 2.0f)
        {
            clearRank.text = "A";
            rank = 300;
        }
        else if (time < lastSpawn + 3.0f)
        {
            clearRank.text = "B";
            rank = 200;
        }
        else
        {
            clearRank.text = "F";
            rank = 100;
        }
        clearRank.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        // rankBonus
        mul.gameObject.SetActive(true);
        rankBonus.text = rank.ToString("N0");
        rankBonus.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        // BestTime Update
        totalScore = stage.score * rank;
        if (totalScore > PlayerPrefs.GetFloat(stage.name + "_TotalScore"))
        {
            PlayerPrefs.SetFloat(stage.name + "_TotalScore", totalScore);
            PlayerPrefs.SetFloat(stage.name + "_BestTime", stage.clearTime);
            PlayerPrefs.SetFloat(stage.name + "_LastSpawn", stage.lastSpawnTime);

            bestTime.text = clearTime.text;
            bestRank.text = clearRank.text;
            newRecord.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }

        state = State.UpdateEnd;
    }

    public bool IsRunning() { return state != State.End; }
}
