using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D };
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public int score;
    public GameManager manager;

    public Transform target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject[] coins;

    public GameObject smallRock;
    public bool isChase;        // ���� ����
    public bool isAttack = false;       // ���� ����
    public bool isDead;         // Dead ����

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;

private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found.");
            return;
        }

        if(enemyType != Type.D)
        Invoke("ChaseStart", 1.5f);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if(nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }



    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if (!isDead && enemyType != Type.D && !isAttack)
        {
            float targetRadius = 1.5f;
            float targetRange = 3f;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }
            

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                                      targetRadius,
                                      transform.forward, targetRange,
                                      LayerMask.GetMask("Player"));
            if (rayHits.Length > 0)
            {
                isAttack = true; // ���� ���� ���·� ����
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {

        switch (enemyType)
        {
            case Type.A:    // �Ϲ��� ����
                if (meleeArea != null)
                {
                    isChase = false;
                    isAttack = true;
                    anim.SetBool("isAttack", true);
                    yield return new WaitForSeconds(0.2f);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(1f);
                    isChase = true;
                    isAttack = false;
                    anim.SetBool("isAttack", false);
                }
                break;


            case Type.B:    // ������ ����
                    // ���� ����
                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                    meleeArea.enabled = true;
                    yield return new WaitForSeconds(0.5f);

                     rigid.velocity = transform.forward * 10; // ���������� ������ ����
                     yield return new WaitForSeconds(0.5f); // 0.5�� ���� ����
                     rigid.velocity = Vector3.zero; // ���� �� ����
                    meleeArea.enabled = false;
                    // 3. ���� �� ��Ÿ��
                    yield return new WaitForSeconds(5.0f);
                    break;

            case Type.C:    // ���Ÿ��� ����
                yield return new WaitForSeconds(0.5f); // ���ϴ� ������ ����
                if (!isAttack) {
                    isChase = false;
                    isAttack = true;
                    anim.SetBool("isAttack", true);
                    yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // ���� �ִϸ��̼� ���̸�ŭ ���
                    GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                    Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                    Vector3 bulletDirection = (target.position - transform.position).normalized;
                    rigidBullet.AddForce(bulletDirection * 20, ForceMode.VelocityChange);

                }
                yield return new WaitForSeconds(2.0f);
                isChase = true;
                isAttack = false;
                anim.SetBool("isAttack", false);
                break;
        }
    }

    void FixedUpdate()
    {
        Targeting();    
        FreezeVelocity();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, true, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false, false));
            Destroy(other.gameObject);
        }
    }

    public void HitByGrenade(Vector3 explositionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explositionPos;
        StartCoroutine(OnDamage(reactVec, false, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool strongKnockback, bool isGrenade)
    {
        foreach(MeshRenderer mesh in meshs)
        mesh.material.color = Color.red;
        if (strongKnockback)
        {
            reactVec = reactVec.normalized;
            rigid.AddForce(-reactVec * 3, ForceMode.Impulse);
        }
        else
        {
            reactVec = reactVec.normalized;
            rigid.AddForce(-reactVec * 1.5f, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(0.13f);

        // �ǰ� ����
        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        
        // ���� ���
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;
            gameObject.layer = 12;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            Player player = target.GetComponent<Player>();
            player.score += score;
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);

            switch(enemyType)
            {
                case Type.A:
                    manager.enemyCntA--;
                    break;
                case Type.B:
                    manager.enemyCntB--;
                    break;
                case Type.C:
                    manager.enemyCntC--;
                    break;
                case Type.D:
                    manager.enemyCntD--;
                    break;
            }


            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

             Destroy(gameObject, 4);
        }
    }
}
