/**
 * 
 *  스크립트 이름 : BossAttack.cs
 *  스크립트 용도 : 보스 공격 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public BossManager BossManager;

    bool attacking = false;
    bool playerIn = false;

    private void OnTriggerStay(Collider other)
    {
        // 플레이어 공격 판단
        if (other.CompareTag("Player") && !attacking)
        {
            playerIn = true;
            attacking = true;
           BossManager.BossAni.SetBool("Run Forward", false);
            StartCoroutine("Attack");          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerIn = false;
    }

    IEnumerator Attack()
    {
        BossManager.BossAni.SetTrigger("Attack 01");
        yield return new WaitForSeconds(1f);
        BossManager.BossAni.SetBool("Run Forward", true);
        if (playerIn)
            _player.CurrentHp -= 0.3f;
        attacking = false;
    }
}
