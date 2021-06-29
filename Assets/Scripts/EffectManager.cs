using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public enum EffectType
{
    ShatteredWindow,
    NinjaDisappear,
    HitEffect,
    BreakWindow
}

public enum PostEffectType
{
    HeartBeat,
    Death
}

public class GameEffect
{
    public PostEffectType type;
    public float lifeTime;
    public Vignette vignette;
    public Bloom bloom;
    public DepthOfField dof;
    public float elapsedTime;
    public bool isRemain;

    public void Update()
    {
        switch (type)
        {
            case PostEffectType.HeartBeat:
                vignette.intensity.value = 0.55f + Mathf.Sin(elapsedTime * 10) * 0.1f;
                break;
            case PostEffectType.Death:
                if (elapsedTime < 1)
                {
                    vignette.intensity.value += Time.deltaTime * 0.45f;
                    vignette.roundness.value += Time.deltaTime;
                    bloom.intensity.value += Time.deltaTime * 15;
                    if (bloom.intensity.value > 20) bloom.intensity.value = 20;
                    dof.focalLength.value += 300 * Time.deltaTime;
                }
                break;
        }
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
    public PostProcessVolume volume;

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
        profile = volume.profile;
    }

    private void Update()
    {
        if (gameEffect != null)
        {
            gameEffect.Update();
            if (gameEffect.elapsedTime > gameEffect.lifeTime)
            {
                if (gameEffect.type == PostEffectType.HeartBeat)
                {
                    gameEffect = null;
                }
            }
        }
    }

    public void HeartBeat(float lifeTime)
    {
        if (gameEffect == null)
        {
            GameEffect effect = new GameEffect();
            effect.type = PostEffectType.HeartBeat;
            effect.lifeTime = lifeTime;
            profile.TryGetSettings(out effect.vignette);
            gameEffect = effect;
        }
        else
        {
            if (gameEffect.type == PostEffectType.HeartBeat)
            {
                gameEffect.elapsedTime = 0;
            }
        }
    }

    public void Death()
    {
        if (gameEffect == null)
        {
            GameEffect effect = new GameEffect();
            effect.type = PostEffectType.Death;
            profile.TryGetSettings(out effect.vignette);
            profile.TryGetSettings(out effect.bloom);
            profile.TryGetSettings(out effect.dof);
            gameEffect = effect;
        }
        else
        {
            if (gameEffect.type == PostEffectType.HeartBeat)
            {
                profile.TryGetSettings(out gameEffect.bloom);
                profile.TryGetSettings(out gameEffect.dof);
                gameEffect.elapsedTime = 0;
            }
        }
    }

    public void CreateEffect(EffectType type, float lifeTime, Vector3 position = new Vector3())
    {
        GameObject obj;
        switch (type)
        {
            case EffectType.ShatteredWindow:
                obj = Instantiate(effectPrefabs[0], UICamera.Instance.transform);
                obj.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.1f), Random.Range(0.5f, 1.0f));
                Destroy(obj, lifeTime);
                break;
            case EffectType.NinjaDisappear:
                obj = Instantiate(effectPrefabs[1], position, Quaternion.identity);
                Destroy(obj, lifeTime);
                break;
            case EffectType.HitEffect:
                obj = Instantiate(effectPrefabs[2], position, Quaternion.identity);
                Destroy(obj, lifeTime);
                break;
            case EffectType.BreakWindow:
                obj = Instantiate(effectPrefabs[3], UICamera.Instance.transform);
                obj.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.1f, 0.1f), Random.Range(0.5f, 1.0f));
                Destroy(obj, lifeTime);
                break;

        }
    }
}
