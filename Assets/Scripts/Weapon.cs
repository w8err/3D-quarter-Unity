using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isSwing = false;
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;


    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {

    if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");   // 코루틴함수는 stratCoroutine("string형"); , stopCoroutin("string형); 으로 선언해야 함
        }
        if (type == Type.Range && curAmmo > 0) 
        {
            curAmmo--;
            StartCoroutine("Shot");   // 코루틴함수는 stratCoroutine("string형"); , stopCoroutin("string형); 으로 선언해야 함
        }
    }
    
    IEnumerator Swing()
    {
        isSwing = true;
        //1
        // yield return null; // 1프레임 대기
        //2
        // yield return null; // 여러번 사용가능. 이거 wait 트리거처럼 쓰는거같음

        yield return new WaitForSeconds(0.1f);  // 0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);  // 0.3초 대기
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.2f);  // 0.3초 대기
        trailEffect.enabled = false;
        isSwing = false;

    }

    IEnumerator Shot()
    {
        if(curAmmo != 0)
        {
            // #1. 총알 발사
            GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 50;

            // #2. 탄피 배출
            yield return null;
            GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-4, -2) + Vector3.up * Random.Range(2, 3);
            caseRigid.AddForce(caseVec, ForceMode.Impulse);
            caseRigid.AddTorque(Vector3.right * Random.Range(1, 5));

        }
    }
    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 (교차실행)

    // if coroutine?
    // Use() 메인루틴 +(같이실행) Swing() 코루틴 (co-routine) (co-op)
}
