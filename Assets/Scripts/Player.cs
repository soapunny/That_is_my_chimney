using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera movingCamara;
    [SerializeField]
    Animator animator;
    private PlayerInput playerInput;
    private bool isRunning = false;
    public Gun handGun;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        handGun = GetComponent<Gun>();
        Run();
    }

    // Update is called once per frame
    void Update()
    {
        if(movingCamara)
        {
            Vector3 leadPos = new Vector3(movingCamara.GetComponent<Transform>().position.x-1, movingCamara.GetComponent<Transform>().position.y, movingCamara.GetComponent<Transform>().position.z-1);
            GetComponent<Transform>().position = leadPos;
        }
        //if(playerInput.fire)    //fire가 true일 경우 발사
        //{
        //    handGun.Fire();
        //}
    }
    public void Run()
    {
        if (!isRunning)
        {
            isRunning = true;
            animator.SetBool("isRunning", isRunning);
        }
    }
    public void StopRunning()
    {
        if (isRunning)
        {
            isRunning = false;
            animator.SetBool("isRunning", isRunning);
        }
    }
}
