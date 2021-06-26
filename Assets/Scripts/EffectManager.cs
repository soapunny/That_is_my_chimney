using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public enum EffectType
{
    ShatteredWindow
}

public class GameEffect
{
    public EffectType type;
    public float lifeTime;
    public Vignette vignette;
    public float elapsedTime;

    public void Update()
    {
        vignette.intensity.value = 0.55f + Mathf.Sin(elapsedTime * 10) * 0.1f;
        elapsedTime += Time.deltaTime;
    }
}

public class EffectManager : MonoBehaviour
{

    public struct EffectInfo
    {
        EffectType type;
    }

    public static EffectManager Instance { get; private set; }

    public GameObject[] effectPrefabs;
    public PostProcessProfile profile;

    GameEffect gameEffect;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (gameEffect != null)
        {
            gameEffect.Update();
            if (gameEffect.elapsedTime > gameEffect.lifeTime) gameEffect = null;
        }
    }

    public void HeartBeat(float lifeTime)
    {
        if (gameEffect == null)
        {
            GameEffect effect = new GameEffect();
            effect.lifeTime = lifeTime;
            profile.TryGetSettings(out effect.vignette);
            gameEffect = effect;
        }
        else
        {
            gameEffect.elapsedTime = 0;
        }
    }

    public void CreateEffect(EffectType type, float lifeTime)
    {
        switch (type)
        {
            case EffectType.ShatteredWindow:
                GameObject obj = Instantiate(effectPrefabs[0], UICamera.Instance.transform);
                obj.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.1f), Random.Range(0.5f, 1.0f));
                Destroy(obj, lifeTime);
                break;
        }
    }
}
