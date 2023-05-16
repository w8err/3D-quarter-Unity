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
            StartCoroutine("Swing");   // 코루틴함수는 stratCoroutine("string형"); , stopCoroutin("string형); 으로 선언해야 함
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
    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 (교차실행)

    // if coroutine?
    // Use() 메인루틴 +(같이실행) Swing() 코루틴 (co-routine) (co-op)
}
