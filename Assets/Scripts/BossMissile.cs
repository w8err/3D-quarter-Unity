using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet // ��ӹ޴� Ŭ����, �� Ŭ������ �ڽ� Ŭ������ ��
{
    public Transform target;
    NavMeshAgent nav;
    void Awake()
    {
        FindTarget();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(target.position);
    }

    private void FindTarget()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject == null)
        {
            target = playerObject.transform;
        }
    }
}
