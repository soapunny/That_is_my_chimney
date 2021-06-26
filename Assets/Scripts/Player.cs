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
    LineRenderer line;
    private int playerHp;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        handGun = GetComponent<Gun>();

        dollyCart = cart.gameObject.GetComponent<CinemachineDollyCart>();
        //Run();

        line = Camera.main.GetComponent<LineRenderer>();
        line.positionCount = 2;

        //SetPlayerHp && UI
        playerHp = 3;
        for(int i = 0; i < playerHp; i++)
        {
            GameManager.gameManager.PlusHpImage(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if(dollyCart.enabled == true)
        //   transform.position = new Vector3 (cart.transform.position.x , transform.position.y , cart.transform.position.z);

        if (playerInput.fire)    //fire가 true일 경우 발사
        {
            handGun.Fire();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            line.SetPosition(0, ray.origin);
            line.SetPosition(1, ray.origin + ray.direction * 10);
        }
        if(playerInput.reload)
        {
            handGun.Reload();
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

    public void GetDamage()
    {
        GameManager.gameManager.MinusHpImage(playerHp - 1);
        playerHp--;
    }

    public void GetHeal()
    {
        GameManager.gameManager.MinusHpImage(playerHp);
        playerHp++;
    }
}
