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

    public void Fire()  // PlayerShooter���� Gun�� �߻縦 ���� �Լ�
    {
        if (magAmmo >= 1)
        {
            Shot();
        }
    }

    private void Shot() // ������ �߻�ó���� �ϴ� �Լ�
    {
        // Raycastó���� �� ü�� ����ó��
        magAmmo--;
    }

    public void Reload() // ������ �Լ�
    {

    }

    private void ReloadAmmo() // ���� ������ �Լ�
    {
        magAmmo = magCapacity;
    }
}
