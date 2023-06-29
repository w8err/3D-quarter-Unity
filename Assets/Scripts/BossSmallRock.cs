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
        float xForce = Random.Range(50f, 400f);
        float yForce = Random.Range(50f, 400f);
        float zForce = Random.Range(-200f, 200f);
        Vector3 torque = new Vector3(xForce, yForce, zForce);

        rigid.AddForce(transform.forward * xForce, ForceMode.Impulse);
        rigid.AddForce(transform.up * yForce, ForceMode.Impulse);
        rigid.AddForce(transform.right * zForce, ForceMode.Impulse);
        rigid.AddTorque(torque, ForceMode.Acceleration);

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 3);
    }
}

