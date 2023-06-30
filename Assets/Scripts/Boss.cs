using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy                           // 상속 주의점! Awake()함수는 자식 스크립트만 단독 실행함!
{
    public GameObject normalMissile;
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public bool isLook;
    public GameObject[] childEnemy = new GameObject[3];


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
        if (isDead)
        {
            StopAllCoroutines();    // 이 스크립트의 모든 코루틴을 정지
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

        int ranAction = Random.Range(0, 10);
        switch (ranAction)       // Switch문에서 break문을 생략해서 조건을 늘릴 수 있다.
        {
            case 0:
            // 몬스터 소환 패턴
            StartCoroutine(SpawnChild());
                break;
            case 1:
            case 2:
                // 일반 미사일 패턴
                StartCoroutine(normalMissileShot());
                break;
            case 3:
                // 미사일 패턴
                StartCoroutine(MissileShot());
                break;

            case 4:
            case 5:
                // 작은 돌 패턴
                StartCoroutine(SmallRockShot());
                break;
            case 6:
            case 7:
                // 큰 돌 패턴
                StartCoroutine(RockShot());
                break;

            case 8:
            case 9:
            case 10:
                // 내려찍기 패턴
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

    IEnumerator normalMissileShot()
    {
        missilePortA.Translate(0, -5f, 0); missilePortB.Translate(0, -5f, 0);
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject normalMissileA = Instantiate(normalMissile, missilePortA.position, missilePortA.rotation);   // 미사일A 생성
        normalMissileA.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);                                    // 미사일A 스케일 조정
        Rigidbody aRigidBullet = normalMissileA.GetComponent<Rigidbody>();
        Vector3 aBulletADirection = (target.position - transform.position).normalized;
        aRigidBullet.AddForce(aBulletADirection * 35, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.3f);
        GameObject normalMissileB = Instantiate(normalMissile, missilePortB.position, missilePortB.rotation);   // 미사일B 생성
        normalMissileB.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);                                    // 미사일B 스케일 조정
        Rigidbody bRigidBullet = normalMissileB.GetComponent<Rigidbody>();
        Vector3 bBulletADirection = (target.position - transform.position).normalized;
        bRigidBullet.AddForce(bBulletADirection * 35, ForceMode.VelocityChange);

        missilePortA.Translate(0, 5f, 0); missilePortB.Translate(0, 5f, 0);
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
        int randInt = Random.Range(0, 3); // 0부터 2 사이의 랜덤한 정수 값 생성
        isLook = false;
        anim.SetTrigger("doBigShot");

        yield return new WaitForSeconds(1f); // 약간의 딜레이

        GameObject instantiatedEnemy = Instantiate(childEnemy[randInt],transform.position, transform.rotation);
        Rigidbody rigidEnemy = instantiatedEnemy.GetComponent<Rigidbody>();

        rigidEnemy.AddForce(new Vector3(0, 30, -30), ForceMode.Impulse);

        Enemy enemyScript = instantiatedEnemy.GetComponent<Enemy>();
        switch (randInt)
        {
            case 0:
                enemyScript.enemyType = Enemy.Type.A;
                break;
            case 1:
                enemyScript.enemyType = Enemy.Type.B;
                break;
            case 2:
                enemyScript.enemyType = Enemy.Type.C;
                break;
        }
        enemyScript.target = target.transform;
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }
}
