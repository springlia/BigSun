using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 0.1f;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position -  target.position;
    }

    private void LateUpdate() //배경 따라오게
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
