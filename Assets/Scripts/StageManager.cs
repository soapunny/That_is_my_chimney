using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
enum ZoomState
{
    ZOOM_IN, ZOOM_OUT
}
[RequireComponent(typeof(BoxCollider))]
public class StageManager : MonoBehaviour
{
    GameObject player;
    CinemachineDollyCart dollyCart;
    [SerializeField]
    CinemachineVirtualCamera movingCamara;
    [SerializeField]
    CinemachineVirtualCamera stage3Camara;
    CinemachineFollowZoom stage3FollowZoom;
    [SerializeField]
    List<GameObject> lRespawnSpots;
    Transform target;
    bool isStage3 = false;
    private float elapsedTime = 0f;
    private ZoomState zoomState = ZoomState.ZOOM_OUT;
    // Start is called before the first frame update
    void Start()
    {
        stage3FollowZoom = stage3Camara.GetComponent<CinemachineFollowZoom>();
        player = GameObject.FindWithTag("Player");
        target = stage3Camara.LookAt;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStage3)
        {
            elapsedTime += Time.deltaTime;
            if (4f <= elapsedTime  && elapsedTime < 8f && zoomState == ZoomState.ZOOM_OUT)
            {
                stage3Camara.enabled = true;
                stage3Camara.Priority = 11;
                zoomState = ZoomState.ZOOM_IN;
                target = lRespawnSpots[0].transform;
                stage3Camara.LookAt = target;
                stage3FollowZoom.m_Width = 5;
            }
            else if (8f <= elapsedTime && elapsedTime < 12f && zoomState == ZoomState.ZOOM_IN)
            {
                stage3Camara.Priority = 9;
                stage3Camara.enabled = false;
                zoomState = ZoomState.ZOOM_OUT;
                lRespawnSpots.RemoveAt(0);
            }
            else if(12f <= elapsedTime)
            {
                elapsedTime = 0f;
                if(lRespawnSpots.Count == 0)
                {
                    NextStage();
                }

            }
        }
    }

    private void NextStage()
    {
        movingCamara.GetComponent<CinemachineVirtualCamera>().enabled = false;
        movingCamara.GetComponent<CinemachineFreeLook>().enabled = true;
        player.GetComponent<PlayerController>().Run();
        isStage3 = false;
        dollyCart.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        dollyCart = other.GetComponent<CinemachineDollyCart>();
        if (dollyCart)
        {
            //
            dollyCart.enabled = false;
            isStage3 = true;
            player.GetComponent<PlayerController>().StopRunning();
            movingCamara.GetComponent<CinemachineVirtualCamera>().enabled = true;
            movingCamara.GetComponent<CinemachineFreeLook>().enabled = false;
        }
    }
}
