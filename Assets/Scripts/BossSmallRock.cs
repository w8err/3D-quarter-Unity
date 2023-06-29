using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossSmallRock : Bullet
{
    private float originalScaleValue;
    private float originalAngularPower;
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
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
        originalScaleValue = scaleValue;
        originalAngularPower = angularPower;

        while (!isShoot)
        {
            angularPower += 0.003f;
            scaleValue += 0.0033f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}

