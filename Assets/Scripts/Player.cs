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
    public GameObject cart;
    CinemachineDollyCart dollyCart;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        handGun = GetComponent<Gun>();

        dollyCart = cart.gameObject.GetComponent<CinemachineDollyCart>();
        Run();
    }

    // Update is called once per frame
    void Update()
    {

        //if(dollyCart.enabled == true)
        //   transform.position = new Vector3 (cart.transform.position.x , transform.position.y , cart.transform.position.z);

        if (playerInput.fire)    //fire가 true일 경우 발사
        {
            handGun.Fire();
        }
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
