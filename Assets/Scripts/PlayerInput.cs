using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string fireButtonName = "Fire1"; // �߻� ��ư
    public string reloadButton = "Reload"; // ������

    public bool fire { get; private set; }  // ������ �߻� �Է°�
    public bool reload { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���ӿ��� ������ �� Ű�� �ȹ޴� ����ó�� �ʿ�

        fire = Input.GetButtonDown(fireButtonName); // fire �Է� ����
        reload = Input.GetButton(reloadButton); // reload �Է� ����
    }
}
