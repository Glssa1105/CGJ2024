using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // 虚拟相机的引用
    public CinemachineVirtualCamera virtualCamera;

    // 物理相机的引用
    private Camera _camera;

    void Start()
    {
        // 获取物理相机组件
        _camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (virtualCamera != null)
        {
            // 获取虚拟相机的Transform组件
            Transform vcamTransform = virtualCamera.transform;
            // 将物理相机的位置和旋转与虚拟相机同步
            _camera.transform.position = vcamTransform.position;
            _camera.transform.rotation = vcamTransform.rotation;
        }
    }
}