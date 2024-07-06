using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : SingletonMono<CameraManager>
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;
    
    public CinemachineVirtualCamera virtualCamera1;
    public CinemachineVirtualCamera virtualCamera2;
    
    
    
    void Start()
    {
        // 配置实际相机
        SetupCamera(camera1, virtualCamera1);
        SetupCamera(camera2, virtualCamera2);
    }
    
    
    void SetupCamera(Camera cam, CinemachineVirtualCamera virtualCam)
    {
        var cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            cinemachineBrain = cam.gameObject.AddComponent<CinemachineBrain>();
        }

        // 保证该相机只受到特定虚拟相机的影响
        virtualCam.gameObject.AddComponent<CameraLink>().linkedCamera = cam;
    }
}
public class CameraLink : MonoBehaviour
{
    public Camera linkedCamera;
    private CinemachineVirtualCamera virtualCam;

    void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        virtualCam.Priority = 10; // 确保虚拟相机的优先级足够高
    }

    void OnEnable()
    {
        // 确保只有链接的相机会受这个虚拟相机的影响
        var cinemachineBrain = linkedCamera.GetComponent<CinemachineBrain>();
        if (cinemachineBrain != null)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = 0;
            virtualCam.enabled = true;
        }
    }

    void OnDisable()
    {
        if (virtualCam != null)
        {
            virtualCam.enabled = false;
        }
    }
}