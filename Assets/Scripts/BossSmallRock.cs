using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossSmallRock : Bullet
{
    float originalScaleValue;
    float originalAngularPower;
    Rigidbody rigid;
    bool isShoot = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(SGainPower());
        StartCoroutine(SGainPowerTimer());
    }

    IEnumerator SGainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator SGainPower()
    {
        while (!isShoot)
        {
            for (int i = 0; i < 5; i++)
            {
                float launchForce = Random.Range(1, 3);
                rigid.AddTorque(transform.forward * launchForce, ForceMode.Impulse);
                float launchAngle = Random.Range(3, 4);
                rigid.AddForce(transform.up * launchAngle, ForceMode.Impulse);
            }
            yield return null;
        }
    }
}

