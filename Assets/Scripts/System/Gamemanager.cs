/**
 * 
 *  스크립트 이름 : Gamemanager.cs
 *  스크립트 용도 : 현재 씬을 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    // 챕터별 포탈 관련
    // 포탈 이팩트 0번 인덱스 => 챕터1
    public List<ParticleSystem> portal_Particle = new List<ParticleSystem>();
   
    // 포탈 콜라이더
    public List<MeshCollider> portal_Wall = new List<MeshCollider>();
    public List<Transform> portal = new List<Transform>();
    
    // 상태 바
    public Animator State_bar;
    public Text State_text;

    // 페이드인 아웃 그림
    public Image fadeinout;
    FaidInOut FaidInOut;

    // 플레이어 게임오브젝트
    GameObject Player;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Boss")
            return;

        Player = GameObject.FindGameObjectWithTag("Player");
        FaidInOut = fadeinout.GetComponent<FaidInOut>();
        // 맵 돌아올시 페이드 아웃
        if(_Faid.NextScene != null)
        {
            fadeinout.gameObject.SetActive(true);
            _Faid.NextScene = "Main";
            FaidInOut.OutStartFadeAnim();
            _Faid.NextScene = null;
        }
         Cursor.visible = false;     
        Cursor.lockState = CursorLockMode.Locked;

        if (_player.Death)
        {
            _player.Death = false;
            _player.CurrentHp = 1;
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Boss")
            return;

        portal_onoff();
        Player_back();

        // 업적 최신화
        if (_player.Upgrade_value == "유니크" || _player.Upgrade_value == "레전드리") { }
        else _Quest.current_count[3] = int.Parse(_player.Upgrade_value);
    }

    public void State(string text)
    {
        State_text.text = text;
        State_bar.SetBool("move", true);
        StartCoroutine("State_s");
    }

    IEnumerator State_s()
    {
        yield return new WaitForSeconds(1.5f);
        State_bar.SetBool("move", false);
    }

    void Player_back()
    {
        // 어떠한 챕터를 다녀왔을 때 해당 챕터 포탈 앞에 소환
        if (!_chapter.ChapterInOut)
        {
            if (_chapter.CurrentChapter == 1)
            {
                Player.transform.position = portal[_chapter.CurrentChapter - 1].position;
                _chapter.CurrentChapter = 0;
            }
            else if (_chapter.CurrentChapter == 2)
            {
                Player.transform.position = portal[_chapter.CurrentChapter - 1].position;
                _chapter.CurrentChapter = 0;
            }
            else if (_chapter.CurrentChapter == 3)
            {
                Player.transform.position = portal[_chapter.CurrentChapter - 1].position;
                _chapter.CurrentChapter = 0;
            }
        }

    }

    void portal_onoff()
    {
        // 해당 챕터를 클리어 할시 다음 챕터가 열림
        if (_chapter.Chapter1 == true && !portal_Particle[0].isPlaying)
        { // 챕터1 클리어 > 챕터2 오픈
            portal_Particle[0].Play();
            portal_Wall[0].gameObject.GetComponent<MeshCollider>().isTrigger = false;
        }
        else if (_chapter.Chapter2 == true && !portal_Particle[1].isPlaying)
        { // 챕터2 클리어 > 챕터3 오픈
            portal_Particle[1].Play();
            portal_Wall[1].gameObject.GetComponent<MeshCollider>().isTrigger = false;
        }
        else if (_chapter.Chapter3 == true && !portal_Particle[2].isPlaying)
        { // 전 챕터 클리어 > 최종 보스 오픈
            portal_Particle[2].Play();
            portal_Wall[2].gameObject.GetComponent<MeshCollider>().isTrigger = false;
        }
    }


}
