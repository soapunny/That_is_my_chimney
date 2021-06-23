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
    // Start is called before the first frame update
    void Start()
    {
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
}
