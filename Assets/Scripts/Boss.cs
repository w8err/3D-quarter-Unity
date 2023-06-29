using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy                           // ��� ������! Awake()�Լ��� �ڽ� ��ũ��Ʈ�� �ܵ� ������!
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public bool isLook;
    public GameObject[] childEnemy;


    Vector3 lookVec;
    Vector3 tauntVec;

    void Awake()
    {

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Update()
    {
        if(isDead)
        {
            StopAllCoroutines();    // �� ��ũ��Ʈ�� ��� �ڷ�ƾ�� ����
            return;
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think()
    {
    yield return new WaitForSeconds(0.1f);

    int ranAction = Random.Range(0,10);
        switch (ranAction)       // Switch������ break���� �����ؼ� ������ �ø� �� �ִ�.
        {
            case 0:
                // ���� ��ȯ ����
                //StartCoroutine(SpawnChild());
            case 1:
            case 2:
            case 3:
                // �̻��� ����
                StartCoroutine(MissileShot());
                break;
                  
            case 4:
            case 5:
                // ���� �� ����
                StartCoroutine(SmallRockShot());
                break;
            case 6:
            case 7:
                // ū �� ����
                StartCoroutine(RockShot());
                break;

            case 8:
            case 9:
            case 10:
                // ������� ����
                StartCoroutine(Taunt());
                break;

        }
    }

    IEnumerator MissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;

        StartCoroutine(Think());
    }

    IEnumerator SmallRockShot()
    {
        isLook = false;
        anim.SetTrigger("doBigShot");
        for (int i = 0; i < 15; i++)
        {
            Instantiate(smallRock, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);
        isLook = true;

        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;        
        
        StartCoroutine(Think());
    }

    IEnumerator SpawnChild()
    {
        yield return null;
    }
}
