using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, true));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
            Destroy(other.gameObject);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec, bool strongKnockback)
    {
        mat.color = Color.red;
        if (strongKnockback)
        {
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 3, ForceMode.Impulse);
        }
        else
        {
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 1.5f, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(0.13f);

        if(curHealth > 0)
        { 
            mat.color = Color.white;

        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 12;

            reactVec = reactVec.normalized;

            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 3);
        }
    }
}
