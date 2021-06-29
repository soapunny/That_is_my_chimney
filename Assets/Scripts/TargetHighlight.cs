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

    float elapsedTime;
    public float limitTime;
    public bool isOn;

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

        outerCircle.color = Color.Lerp(fromOuterColor, toOuterColor, elapsedTime / limitTime);
        innerCircle.color = Color.Lerp(fromInnerColor, toInnerColor, elapsedTime / limitTime);
        innerCircle.transform.Rotate(new Vector3(0.0f, 0.0f, 360 * Time.deltaTime));
        if (isOn) elapsedTime += Time.deltaTime;
    }
}
