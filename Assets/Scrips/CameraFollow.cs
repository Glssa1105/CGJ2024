using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 跟随的目标对象
    public float smoothSpeed = 0.125f; // 平滑速度
    public Vector3 offset; // 偏移量

    void LateUpdate()
    {

        // 目标位置加上偏移量
        Vector3 desiredPosition = target.position + offset;

        // 使用插值计算相机的新位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        smoothedPosition.x=Mathf.Max(0, smoothedPosition.x);
        
        smoothedPosition.z = -100;
        // 设置相机位置
        transform.position = smoothedPosition;
        
    }
}