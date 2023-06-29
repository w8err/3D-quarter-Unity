using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private float cameraSpeed = 2f;   // ī�޶� �̵� �ӵ�
    private bool isCtrlPressed = false; // Ctrl Ű ���� ����

    void Update()
    {
        transform.position = target.position + offset;
    }
}
