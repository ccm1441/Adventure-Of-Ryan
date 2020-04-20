/**
 * 
 *  스크립트 이름 : Quset.cs
 *  스크립트 용도 : 업적 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Quset : MonoBehaviour
{
    // 게임매니져 임포트
    public Gamemanager gamemanager;
    // 업적을 표시할 텍스트
    public List<Text> Quest_text = new List<Text>();
    // 반복 업적 업데이트할 텍스트
    public List<Text> Repeat_text = new List<Text>();
    // 업적 완료 버튼
    public List<Button> Quest_button = new List<Button>();
    // 업적 UI
    public GameObject Quest_backgroud;


    // Start is called before the first frame update
    void Start()
    {
        // 업적 초기화
        if (!_Quest.quest_init)
            Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // 반복 업적 업데이트
        Repeat_text[0].text = "몬스터 " + _Quest.need_count[0] + " 마리 잡기";
        Repeat_text[1].text = "챕터 " + _Quest.need_count[1] + " 클리어 하기";
        Repeat_text[2].text = "무기 +" + _Quest.need_count[3] + " 강화하기";

        //업적 업데이트
        for (int i = 0; i < Quest_text.Count; i++)
        {
            Quest_text[i].text = _Quest.current_count[i] + " / " + _Quest.need_count[i];

            if (_Quest.current_count[i] >= _Quest.need_count[i])
            {
                _Quest.quest_clear[i] = true;
                Quest_text[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // NPC 오픈
        if (Input.GetMouseButtonDown(2))
            if (other.CompareTag("Player") && !_player.shopOverlap)
            {
                Quest_backgroud.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _player.shop = true; // 이동,마우스회전 잠금
                _player.shopOverlap = true; // 중복 방지
            }
    }

    public void offShop()
    {
        // 업적 UI 닫기
        _player.shop = false;
        _player.shopOverlap = false;
        Quest_backgroud.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Initialize()
    {
        //업적 초기화
        _Quest.need_count[0] = 100;
        _Quest.need_count[1] = 1;
        _Quest.need_count[2] = 1;
        _Quest.need_count[3] = 4;
        _Quest.need_count[4] = 1;
        _Quest.need_count[5] = 1;

        _Quest.quest_init = true;
    }

    public void get_item()
    {
        // 아이템 지급
        // 현재 눌린 버튼의 이름을 가져옴
        string button_name = EventSystem.current.currentSelectedGameObject.name;

        // 버튼 이름을 통하여 판단 후 아이템 지급
        for (int i = 0; i < Quest_button.Count; i++)
        {
            if (button_name == Quest_button[i].name && _Quest.quest_clear[i])
            {
                Quest_button[i].interactable = false;
                Text button_text = Quest_button[i].transform.GetChild(0).GetComponent<Text>();
                money(i);
                button_text.text = "수령 완료";

                // 반복 퀘스트 조건 업데이트
                if (i == 0)
                {
                    Quest_button[i].interactable = true;
                    _Quest.need_count[i] += 100;
                    button_text.text = "보상받기";
                    Quest_text[i].gameObject.SetActive(true);
                }
                if (i == 1)
                {
                    _Quest.need_count[i] += 1;
                    if(_Quest.need_count[i] < 4)
                    {
                        Quest_button[i].interactable = true;
                        button_text.text = "보상받기";
                        Quest_text[i].gameObject.SetActive(true);
                    }
                }
                if (i == 3)
                {
                    _Quest.need_count[i] += 3;
                    if (_Quest.need_count[i] < 13)
                    {
                        Quest_button[i].interactable = true;
                        button_text.text = "보상받기";
                        Quest_text[i].gameObject.SetActive(true);
                    }
                    else if (_Quest.need_count[i] == 13)
                        _Quest.need_count[i] = 10;
                }
                break;
            }
            else gamemanager.State("업적을 클리어 해주세요!");
        }
    }

    void money(int questNum)
    {
        // 재화 지급
        int coin = 0, dia = 0;

        switch (questNum)
        {
            case 0:
                coin = 50000;
                dia = 10;
                break;
            case 1:
                coin = 50000;
                dia = 50;
                break;
            case 2:
                coin = 5000000;
                dia = 1000;
                break;
            case 3:
                coin = 20000;
                dia = 30;
                break;
            case 4:
                coin = 200000;
                dia = 100;
                break;
            case 5:
                coin = 500000;
                dia = 200;
                break;
            default:
                break;
        }

        _player.Coin += coin;
        _player.Dia += dia;
        gamemanager.State("정상적으로 보상을 수령하였습니다!");
    }
}
