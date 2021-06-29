using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Boss target;
    public Slider slider;
    public RectTransform backFill;

    Coroutine healthUpdate;

	private void OnEnable()
	{
        slider.value = 0;
        StartCoroutine(HealthReady());
	}

	private void OnDisable()
	{
        if (healthUpdate != null) StopCoroutine(healthUpdate);
	}

    IEnumerator HealthReady()
	{
        float ratio = 0;
        while (slider.value < 1)
        {
            slider.value = Mathf.Lerp(0, 1, ratio);
            ratio += Time.deltaTime;
            yield return null;
        }
        backFill.anchorMax = new Vector2(1, 1);
        healthUpdate = StartCoroutine(HealthUpdate());
    }

    IEnumerator HealthUpdate()
	{
        while (true)
        {
            slider.value = (float)target.hp / target.maxHp;
            if (slider.value < backFill.anchorMax.x)
			{
                Vector2 anchorMax = backFill.anchorMax;
                anchorMax.x = Mathf.Lerp(anchorMax.x, slider.value, Time.deltaTime * 2.0f);
                backFill.anchorMax = anchorMax;
			}
            yield return null;
        }
    }
}
