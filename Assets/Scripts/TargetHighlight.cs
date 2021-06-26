using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHighlight : MonoBehaviour
{
    public Image outerCircle;
    public Image innerCircle;

    [Header("Outer Image Color")]
    public Color fromOuterColor;
    public Color toOuterColor;

    [Header("Inner Image Color")]
    public Color fromInnerColor;
    public Color toInnerColor;

    float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        outerCircle.color = fromOuterColor;
        innerCircle.color = fromInnerColor;
    }

    // Update is called once per frame
    void Update()
    {
        outerCircle.color = Color.Lerp(fromOuterColor, toOuterColor, elapsedTime);
        innerCircle.color = Color.Lerp(fromInnerColor, toInnerColor, elapsedTime);
        innerCircle.transform.Rotate(new Vector3(0.0f, 0.0f, 360 * Time.deltaTime));
        elapsedTime += Time.deltaTime;
    }
}