using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private float cameraSpeed = 2f;   // 카메라 이동 속도
    private bool isCtrlPressed = false; // Ctrl 키 눌림 여부

    void Update()
    {
        transform.position = target.position + offset;
    }
}
