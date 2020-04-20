/**
* 
*  스크립트 이름 : SampleAnimation.cs
*  스크립트 용도 : 캐릭터의 애니메이션을 관리
*  
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAnimation : MonoBehaviour
{
    private Animator animator;

    // 사운드 여려개를 재생하기 위함
    public List<AudioClip> sound = new List<AudioClip>();
    public AudioSource[] audioSources = new AudioSource[7];

    // 빌드용
    bool firstcool = _Skill.skillFirstCoolTime;
    bool secondcool = _Skill.skillSecondCoolTime;
    bool buffcool = _Skill.skillBuffCoolTime;

    // 사운드 플래그
    bool walking = false;

    // 애니메이션
    private const string key_isRun = "IsRun";
    private const string key_isAttack01 = "IsAttack01";
    private const string key_isAttack02 = "IsAttack02";
    private const string key_isJump = "IsJump";
    private const string key_isDamage = "IsDamage";
    private const string key_isDead = "IsDead";
    private const string key_isBack = "IsBack";
    private const string key_isBuff = "IsBuff";

    private void Awake()
    {
        // 시작전 오디오소스 셋팅
        for (int i = 0; i < sound.Count; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = sound[i];
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if(!walking)
            StartCoroutine("walkSound");
            animator.SetBool(key_isRun, true);
        }              
        else
            animator.SetBool(key_isRun, false);


        if (Input.GetKeyDown(KeyCode.Q))
        {// 버프
            if(!buffcool)
            {
                    audioSources[3].volume = 0.3f;
                    audioSources[3].Play();                             
                animator.SetBool("IsBuff", true);
            }
        } else animator.SetBool("IsBuff", false);


        if (Input.GetKeyDown(KeyCode.R))
        { // 평타
            if(!firstcool)
            animator.SetBool(key_isAttack01, true);
            if (!_player.attack)
                StartCoroutine("AttackUse");
        }
        else animator.SetBool(key_isAttack01, false);

        if (Input.GetKeyDown(KeyCode.T))
        { // 스킬          
            if (!secondcool)
            {
                animator.SetBool(key_isAttack02, true);
                StartCoroutine("SkillSound");
            }
          
        }           
        else animator.SetBool(key_isAttack02, false);


        if (Input.GetKeyDown("space"))
        {
            audioSources[4].Play();
            animator.SetBool(key_isJump, true);
        }
        else
            animator.SetBool(key_isJump, false);


        if (_player.CurrentHp <= 0.01f && !_player.Death)
        {
            _player.Death = true;
            animator.SetTrigger(key_isDead);
        }
    }

    IEnumerator AttackUse()
    {
        _player.attack = true;
        audioSources[2].volume = 0.2f;
        audioSources[2].Play();
        yield return new WaitForSeconds(0.8f);
        _player.attack = false;
    }

    IEnumerator walkSound()
    {
        walking = true;
        audioSources[0].volume = 0.15f;
             audioSources[0].Play();
        yield return new WaitForSeconds(0.25f);
        walking = false;
    }

    IEnumerator SkillSound()
    {
            audioSources[5].volume = 0.15f;
            audioSources[5].Play();
       
        yield return new WaitForSeconds(3f);
    }
}
