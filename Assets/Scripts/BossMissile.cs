using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet // 상속받는 클래스, 이 클래스는 자식 클래스가 됨
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
