using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform; // �� �߻� ��ġ

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    public AudioClip shotClip; //�� �߻� ����
    public AudioClip reloadClip; // ������ ����

    public int gunDamage = 1;   // �� ������
    public int magCapacity = 6; // źâ �뷮
    public int magAmmo;         // ���� źâ

    public List<LayerMask> hitMasks;

    int hitMask;
    // Start is called before the first frame update
    void Start()
    {
        hitMask = 0;
        foreach (var mask in hitMasks)
        {
            hitMask += mask.value;
        }
        gunAudioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
    }

    public void Fire()  // PlayerShooter���� Gun�� �߻縦 ���� �Լ�
    {
        if (magAmmo >= 1)
        {
            Shot();
        }
        else if(magAmmo <= 0)
        {
            GameManager.gameManager.ShowReload();
        }
    }

    private void Shot() // ������ �߻�ó���� �ϴ� �Լ�
    {
        // Raycastó���� �� ü�� ����ó��

        magAmmo--;
        GameManager.gameManager.ChangeBulletUi(magAmmo);
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Camera.main.transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)) - Camera.main.transform.position).normalized, out hitInfo, Mathf.Infinity, hitMask))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            IHitable hitable = hitInfo.collider.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit();
            }
            EffectManager.Instance.CreateEffect(EffectType.HitEffect, 1.5f, hitInfo.point);
        }
    }

    public void Reload() // ������ �Լ�
    {
        ReloadAmmo();
    }

    private void ReloadAmmo() // ���� ������ �Լ�
    {
        GameManager.gameManager.EraseReload();
        magAmmo = magCapacity;
        GameManager.gameManager.ChangeBulletUi(magAmmo);
    }
}
