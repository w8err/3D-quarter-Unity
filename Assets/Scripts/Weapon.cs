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
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {

    if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");   // �ڷ�ƾ�Լ��� stratCoroutine("string��"); , stopCoroutin("string��); ���� �����ؾ� ��
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
    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ (��������)

    // if coroutine?
    // Use() ���η�ƾ +(���̽���) Swing() �ڷ�ƾ (co-routine) (co-op)
}
