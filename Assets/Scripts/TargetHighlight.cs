using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
    public SpriteRenderer outerCircle;
    public SpriteRenderer innerCircle;

    [Header("Outer Image Color")]
    public Color fromOuterColor;
    public Color toOuterColor;

    [Header("Inner Image Color")]
    public Color fromInnerColor;
    public Color toInnerColor;

    [Header("Deactive Color")]
    public Color DeactiveOuterColor;
    public Color DeactiveInnerColor;

    public float elapsedTime;
    public float limitTime;
    public bool isTimer;
    bool isOn;

    public bool TargetOn
    { 
        get 
        { 
            return isOn;
        }

		set
		{
            isOn = value;
            if (value)
			{
                elapsedTime = 0f;
			}
		}
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        elapsedTime = 0f;
        outerCircle.color = fromOuterColor;
        innerCircle.color = fromInnerColor;
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(UICamera.Instance.transform);

        if (isOn)
        {
            outerCircle.color = Color.Lerp(fromOuterColor, toOuterColor, elapsedTime / limitTime);
            innerCircle.color = Color.Lerp(fromInnerColor, toInnerColor, elapsedTime / limitTime);
        }
        else
		{
            outerCircle.color = DeactiveOuterColor;
            innerCircle.color = DeactiveInnerColor;
        }
        innerCircle.transform.Rotate(new Vector3(0.0f, 0.0f, 360 * Time.deltaTime));
        if (isTimer && isOn) elapsedTime += Time.deltaTime;
    }
}
