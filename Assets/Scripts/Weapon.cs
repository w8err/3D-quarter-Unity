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
            StartCoroutine("Swing");   // �ڷ�ƾ�Լ��� stratCoroutine("string��"); , stopCoroutin("string��); ���� �����ؾ� ��
        }
        if (type == Type.Range && curAmmo > 0) 
        {
            curAmmo--;
            StartCoroutine("Shot");   // �ڷ�ƾ�Լ��� stratCoroutine("string��"); , stopCoroutin("string��); ���� �����ؾ� ��
        }
    }
    
    IEnumerator Swing()
    {
        isSwing = true;
        //1
        // yield return null; // 1������ ���
        //2
        // yield return null; // ������ ��밡��. �̰� wait Ʈ����ó�� ���°Ű���

        yield return new WaitForSeconds(0.1f);  // 0.1�� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);  // 0.3�� ���
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.2f);  // 0.3�� ���
        trailEffect.enabled = false;
        isSwing = false;

    }

    IEnumerator Shot()
    {
        if(curAmmo != 0)
        {
            // #1. �Ѿ� �߻�
            GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = bulletPos.forward * 50;

            // #2. ź�� ����
            yield return null;
            GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
            Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
            Vector3 caseVec = bulletCasePos.forward * Random.Range(-4, -2) + Vector3.up * Random.Range(2, 3);
            caseRigid.AddForce(caseVec, ForceMode.Impulse);
            caseRigid.AddTorque(Vector3.right * Random.Range(1, 5));

        }
    }
    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ (��������)

    // if coroutine?
    // Use() ���η�ƾ +(���̽���) Swing() �ڷ�ƾ (co-routine) (co-op)
}
