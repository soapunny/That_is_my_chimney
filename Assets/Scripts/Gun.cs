using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform; // 총 발사 위치

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; //총 발사 사운드
    public AudioClip reloadClip; // 재장전 사운드

    public int gunDamage = 1;   // 총 데미지
    public int magCapacity = 6; // 탄창 용량
    public int magAmmo;         // 현재 탄창

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

    public void Fire()  // PlayerShooter에서 Gun에 발사를 위한 함수
    {
        if (magAmmo >= 1)
        {
            Shot();
        }
    }

    private void Shot() // 실제로 발사처리를 하는 함수
    {
        // Raycast처리로 적 체력 감소처리

        //magAmmo--;
        Debug.Log(11);
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, hitMask))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            IHitable hitable = hitInfo.collider.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit();
            }
        }
    }

    public void Reload() // 재장전 함수
    {

    }

    private void ReloadAmmo() // 실제 재장전 함수
    {
        magAmmo = magCapacity;
    }
}
