/**
 * 
 *  스크립트 이름 : Chapter3Monster.cs
 *  스크립트 용도 : 챕터 3 몬스터 담당
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chapter3Monster : MonoBehaviour
{
    // 몬스터, 플레이어 정보
    Transform monster;
    Transform player;
    // 몬스터 AI를 위한 네비게이션 컴포넌트
    NavMeshAgent nvAgent;
    // 몬스터 애니메이션
    public Animation monsterAni;

    monsterHit monsterhit;

    bool playerEnter = false;
    bool playerAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        monsterhit = transform.Find("hitBox").GetComponent<monsterHit>();
        monster = gameObject.GetComponent<Transform>();
        monsterAni = gameObject.GetComponent<Animation>();

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (monsterhit.monsterdeath)
            nvAgent.enabled = false;

        if (!playerEnter)
            nvAgent.SetDestination(player.transform.position);
        else transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !monsterhit.monsterdeath)
        {
            playerEnter = true;
            nvAgent.enabled = false;
            if (!playerAttack)
                StartCoroutine(AnimationManager("get_hit_front"));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !monsterhit.monsterdeath)
        {
            playerEnter = false;
            nvAgent.enabled = true;
        }
    }

    IEnumerator AnimationManager(string name)
    {
        playerAttack = true;
        monsterAni.CrossFade(name);

        if (!_player.Death)
            _player.CurrentHp -= 0.15f;

        yield return new WaitForSeconds(2f);

        playerAttack = false;

        if (!monsterhit.monsterdeath)
            monsterAni.CrossFade("Anim_Run");
    }
}
