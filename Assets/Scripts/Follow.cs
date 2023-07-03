using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float cameraSpeed = 30f;   // 카메라 이동 속도

    private Vector3 desiredPosition;

    void Update()
    {

        desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed * Time.deltaTime);
    }
}