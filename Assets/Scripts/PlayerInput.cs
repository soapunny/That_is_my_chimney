using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string fireButtonName = "Fire1"; // 발사 버튼
    public string reloadButton = "Reload"; // 재장전

    public bool fire { get; private set; }  // 감지된 발사 입력값
    public bool reload { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //게임오버 상태일 때 키값 안받는 예외처리 필요

        fire = Input.GetButtonDown(fireButtonName); // fire 입력 감지
        reload = Input.GetButton(reloadButton); // reload 입력 감지
    }
}
