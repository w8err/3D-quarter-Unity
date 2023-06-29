using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossRock : Bullet
{
    private float originalScaleValue;
    private float originalAngularPower;
    Rigidbody rigid;
    bool isShoot = false;

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

        float angularPower = (float)Random.Range(0, 100) / 100f;
        float scaleValue = (float)Random.Range(0, 50) / 100f;
        while (!isShoot)
        {
            angularPower += 0.7f * Time.deltaTime;
            scaleValue += 0.5f * Time.deltaTime;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}

