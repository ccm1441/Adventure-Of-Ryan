/**
 * 
 *  스크립트 이름 : Upgrade.cs
 *  스크립트 용도 : 플레이어 무기 강화 NPC
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public Gamemanager gamemanager;
    // 강화 UI
    public GameObject Upgrade_backgroud;
    // 강화 표시 UI
    public Text upgrade_value;
    // 공격력 표시
    public Text current_attack;
    public Text next_attack;
    // 재화 표시 UI
    public Text need_coin;
    public Text need_dia;
    public Text need_unique;
    // 강화 확률 표시 UI
    public Text chance_value;
    // 강화 버튼
    public Button Upgrade_button;
    // 강화 성공 여부 텍스트
    public Text Upgrade_text;
    // 특수 능력 텍스트
    public Text unique_text;
    // 강화 사운드
    public AudioSource up;
    public AudioSource down;


    void Update()
    {
        // 필요 재화 업데이트
        need_coin.text = _Upgrade.need_coin.ToString();
        need_dia.text = _Upgrade.need_dia.ToString();
        need_unique.text = _Upgrade.need_unique.ToString();
        //강화 확률 업데이트
        chance_value.text = "확률 : "+_Upgrade.chance_value.ToString()+"%";
        // 공격력 업데이트
        current_attack.text = "공격력 : " +_player.Attack_value.ToString();
        next_attack.text = "공격력 : +" + _Upgrade.attack_value.ToString();


        // 등급별 텍스트 업데이트
        if (_player.Upgrade_value == "유니크")
        {
            unique_text.text = "특수 능력 부여 : " + (_Upgrade.unique_value+10) + "% 확률로 몬스터가 즉사합니다.";
            upgrade_value.text = _player.Upgrade_value + " >> " + "<color=#00FF10>레전드리</color>";

        }
        else if (_player.Upgrade_value == "레전드리")
            upgrade_value.text = "<color=#00FF10>레전드리</color>";
        else if (int.Parse(_player.Upgrade_value) <= 9)
        {
            int next = int.Parse(_player.Upgrade_value) + 1;
            upgrade_value.text = "+" + _player.Upgrade_value + " >> " + "+" + next;
        }
        else if (int.Parse(_player.Upgrade_value) == 10)
        {
            unique_text.gameObject.SetActive(true);
            unique_text.text = "특수 능력 부여 : " + _Upgrade.unique_value + "% 확률로 몬스터가 즉사합니다.";
            upgrade_value.text = "+" + _player.Upgrade_value + " >> <color=#ECFF00>유니크</color>";

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(2))
            if (other.CompareTag("Player") && !_player.shopOverlap)
            {
                Upgrade_backgroud.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _player.shop = true; // 이동,마우스회전 잠금
                _player.shopOverlap = true; // 중복 방지
            }
    }

    public void offShop()
    {
        // 강화창 닫음
        _player.shop = false;
        _player.shopOverlap = false;
        Upgrade_backgroud.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void weapon_Up()
    {
        // 강화 함수
        if (_player.Upgrade_value == "유니크")
        {
            if (_Upgrade.need_coin > _player.Coin)
                gamemanager.State("금화가 부족합니다!");
            else if (_Upgrade.need_dia > _player.Dia)
                gamemanager.State("다이아가 부족합니다!");
            else if (_Upgrade.need_unique > _player.Unique)
                gamemanager.State("스크롤이 부족합니다!");
            else
            {
                // 재화 차감
                _player.Coin -= _Upgrade.need_coin;
                _player.Dia -= _Upgrade.need_dia;
                _player.Unique -= _Upgrade.need_unique;
                // 공격력 추가
                _Upgrade.attack_value += 100;
                // 특수 능력 조정
                _Upgrade.unique_value += 10;
                StartCoroutine(text_up("강화 성공!", "00FF10"));

                // 업적 업데이트
                _Quest.current_count[5] = 1;
                gamemanager.State("강화의 달인 업적 클리어!");

                _player.Upgrade_value = "레전드리";
                Upgrade_button.interactable = false;

                // 마지막 단계이므로 필요 재화초기화
                _Upgrade.need_coin = 0;
                _Upgrade.need_dia = 0;
                _Upgrade.need_unique = 0;

              
            }
        }
        else if (int.Parse(_player.Upgrade_value) <= 9)
        {
            int next = int.Parse(_player.Upgrade_value) + 1;

            if (_Upgrade.need_coin > _player.Coin)
                gamemanager.State("금화가 부족합니다!");
            else if (int.Parse(_player.Upgrade_value) == 9)
            {
                if (success())
                {
                    // 강화 확률 재조정
                    _Upgrade.chance_value = 100;
                    // 필요 재화 추가
                    _Upgrade.need_coin += 51000;
                    _Upgrade.need_dia += 100;
                    _Upgrade.need_unique += 3;
                    // 공격력 추가
                    _Upgrade.attack_value += 100;

                    _player.Upgrade_value = next.ToString();
                }
                // 재화차감
                _player.Coin -= _Upgrade.need_coin;
            }
            else
            {
                if (success())
                {                   
                    // 필요 재화추가
                    _Upgrade.need_coin += 1500;
                    // 공격력 추가
                    _Upgrade.attack_value += 10;
                    _player.Upgrade_value = next.ToString();
                    // 강화 업적 클리어
                    if(_player.Upgrade_value == _Quest.need_count[3].ToString())
                        gamemanager.State("강화 업적 달성!");
                }
                // 재화차감
                _player.Coin -= _Upgrade.need_coin;
            }
        }
        else if (int.Parse(_player.Upgrade_value) == 10)
        {
            if(_Upgrade.need_coin > _player.Coin)
                gamemanager.State("금화가 부족합니다!");
            else if (_Upgrade.need_dia > _player.Dia)
                gamemanager.State("다이아가 부족합니다!");
            else if (_Upgrade.need_unique > _player.Unique)
                gamemanager.State("스크롤이 부족합니다!");
            else
            {               
                // 재화 차감
                _player.Coin -= _Upgrade.need_coin;
                _player.Dia -= _Upgrade.need_dia;
                _player.Unique -= _Upgrade.need_unique;

                StartCoroutine(text_up("강화 성공!", "00FF10"));
                _player.Upgrade_value = "유니크";

                // 이팩트 출력 허용
                _Upgrade.weapon_effect = true;

                // 필요 재화 추가
                _Upgrade.need_coin += 80000;
                _Upgrade.need_dia += 150;
                _Upgrade.need_unique += 12;

                // 업적 업데이트
                _Quest.current_count[4] = 1;
                gamemanager.State("강화 업적 달성!");
            }
        }
    }

    // 확률적으로 성공/실패 여부 판단
    bool success()
    {
        // 현재 강화 단계
        int current_value = int.Parse(_player.Upgrade_value);
        // 강화 확률 조정
        int value = 100 - (current_value * 10);
       
        // 확률 설정
        int num = Random.Range(1,101);
        
        if (1 <= num && num <= value)
        {
            StartCoroutine(text_up("강화 성공!", "00FF10"));
            up.Play();
            _player.Attack_value += _Upgrade.attack_value;
            _Upgrade.chance_value = 100 - ((current_value + 1) * 10);
            return true;
        }
        else
        {
            StartCoroutine(text_up( "강화 실패!", "FF0000"));
            down.Play();
            return false;
        }            
    }

    IEnumerator text_up(string text,string color)
    {
        Upgrade_text.gameObject.SetActive(true);
        Upgrade_text.text = "<color=#" +color +">" +text +"</color>";
        yield return new WaitForSeconds(0.5f);
        Upgrade_text.gameObject.SetActive(false);

    }
}
