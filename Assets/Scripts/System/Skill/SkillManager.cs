/**
* 
*  스크립트 이름 : SkillManager.cs
*  스크립트 용도 : 스킬을 관리 합니다.
*  
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Gamemanager gamemanager;

    // 스킬 이미지(쿨타임시 필요) 
    public Image skillFirst;
    public Image skillSecond;
    public Image skillBuff;

    // 스킬 설명 텍스트
    public Text skillSecondText;
    public Text skillBuffText;

    // 스킬 이팩트
    public ParticleSystem skillBuffEffect;
    public GameObject skillSecontEffect;

    // Player
    Transform Player;

    private void Start()
    {
        // 스킬 쿨타임 초기 셋팅
        skillFirst.fillAmount = _Skill.skillFirstFill;
        skillSecond.fillAmount = _Skill.skillSecondFill;
        skillBuff.fillAmount = _Skill.skillBuffFill;

        // 플레이어 컴포넌트 겟
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    void Update()
    {
        // 스킬 쿨타임 업데이트
        skillFirst.fillAmount = _Skill.skillFirstFill;
        skillSecond.fillAmount = _Skill.skillSecondFill;
        skillBuff.fillAmount = _Skill.skillBuffFill;
        
        // 스킬 누르면 쿨타임 시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_Skill.skillFirstCoolTime)
            {
                _Skill.skillFirstFill = 0;
                _Skill.skillFirstCoolTime = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!_Skill.skillSecondCoolTime)
            { 
                // 쿨타임 시작
                _Skill.skillSecondFill = 0;

                // 버프 이팩트 생성
                if (GameObject.Find("Lighting_Skill_Sword_Demo(Clone)"))
                    Destroy(GameObject.Find("Lighting_Skill_Sword_Demo(Clone)"));

                Instantiate(skillSecontEffect, Player.position, skillSecontEffect.transform.rotation);
              
                // 현재 스킬 사용 했고 쿨타임 중임
                _Skill.skillSecondCoolTime = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        { // 공격력 증가 스킬
            if (!_Skill.skillBuffCoolTime)
            {
                // 쿨타임 시작
                _Skill.skillBuffFill = 0;

                // 버프 이팩트 생성 후 캐릭터 자식으로 삽입
                ParticleSystem child = Instantiate(skillBuffEffect, Player.position, skillBuffEffect.transform.rotation);
                Transform Parent = Player.GetComponent<Transform>();
                child.transform.parent = Parent;

                // 현재 스킬 사용 했고 쿨타임 중임
                _Skill.skillBuffCoolTime = true;

                // 스킬 능력(공격력 상승)
                _Skill.AttackValueUp = int.Parse((_player.Attack_value * 0.3f).ToString());
                _player.Attack_value += _Skill.AttackValueUp;
            }
            else gamemanager.State("이미 사용 중인 스킬입니다.");
        }

        // 쿨 타임 시작시 시계방향으로 쿨 타임 보여줌
        if (_Skill.skillFirstCoolTime)
        {
            _Skill.skillFirstFill += Time.deltaTime / 0.5f;
            if (_Skill.skillFirstFill >= 1)
                _Skill.skillFirstCoolTime = false;
        }
        if (_Skill.skillSecondCoolTime)
        {
            _Skill.skillSecondFill += Time.deltaTime / 20;
            if (_Skill.skillSecondFill >= 1)
                _Skill.skillSecondCoolTime = false;
        }
        if (_Skill.skillBuffCoolTime)
        {
            _Skill.skillBuffFill += Time.deltaTime / 30;
            if (_Skill.skillBuffFill >= 1)
            {
                _player.Attack_value -= _Skill.AttackValueUp;
                Destroy(GameObject.Find("Buff(Clone)"));
                _Skill.skillBuffCoolTime = false;
            }
        }
    }
}
