using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    public Gun handGun;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        handGun = GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.fire)    //fire가 true일 경우 발사
        {
            handGun.Fire();
        }
    }
}
