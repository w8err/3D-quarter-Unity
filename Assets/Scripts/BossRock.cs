using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossRock : Bullet
{
    private float originalScaleValue;
    private float originalAngularPower;
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPower());
        StartCoroutine(GainPowerTimer());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        originalScaleValue = scaleValue;
        originalAngularPower = angularPower;

        while (!isShoot)
        {
            angularPower += 0.003f;
            scaleValue += 0.0035f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}

