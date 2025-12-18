using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] Vector3 offset;

    void LateUpdate() //카메라가 플레이어 추적
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
