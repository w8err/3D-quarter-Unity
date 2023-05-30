using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }


    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;   // 속도 없애기
        meshObj.SetActive(false);
        effectObj.SetActive(true);              // 매쉬 비활성화, 이펙트 활성화

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                     15,
                                                     Vector3.up, 0f,
                                                     LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5);
    }

}
