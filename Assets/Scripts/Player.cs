using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;


public class Player : MonoBehaviour
{
    enum PlayerState
    {
        Alive,
        Death
    }

    PlayerState state;
    [SerializeField]
    CinemachineVirtualCamera movingCamara;
    [SerializeField]
    Animator animator;
    private PlayerInput playerInput;
    private bool isRunning = false;
    public Gun handGun;
    public GameObject cart;
    CinemachineDollyCart dollyCart;

    private int playerHp;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        handGun = GetComponent<Gun>();

        dollyCart = cart.gameObject.GetComponent<CinemachineDollyCart>();
        //Run();

        //SetPlayerHp && UI
        playerHp = 3;
        GameManager.gameManager.ChangeHpUi(playerHp);
        state = PlayerState.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == PlayerState.Death) return;
        //if(dollyCart.enabled == true)
        //   transform.position = new Vector3 (cart.transform.position.x , transform.position.y , cart.transform.position.z);

        if (playerInput.fire)    //fire�� true�� ��� �߻�
        {
            handGun.Fire();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        playerHp--;
        GameManager.gameManager.ChangeHpUi(playerHp);
        if (state == PlayerState.Alive && playerHp <= 0)
        {
            state = PlayerState.Death;
            EffectManager.Instance.Death();
            GameManager.gameManager.GameOver();
        }
    }

    public void GetHeal()
    {
        playerHp++;
        GameManager.gameManager.ChangeHpUi(playerHp);
    }
}
