/**
 * 
 *  스크립트 이름 : InParticle.cs
 *  스크립트 용도 : 스킬 파티클 충돌 여부
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InParticle : MonoBehaviour
{
    public static bool attack = false;


    private void OnParticleTrigger()
    {
        if (!attack)
            StartCoroutine("skillAttack");
    }

    IEnumerator skillAttack()
    {
        attack = true;
        yield return new WaitForSeconds(1f);
       // attack = false;
    }
}
