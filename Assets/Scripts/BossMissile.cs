using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMissile : Bullet
{
    public Transform target;
    UnityEngine.AI.NavMeshAgent nav;
    void Awake()
    {
        FindTarget();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(target.position);
    }

    private void FindTarget()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }
}
