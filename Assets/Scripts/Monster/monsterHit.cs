/**
 * 
 *  스크립트 이름 : monsterHit.cs
 *  스크립트 용도 : 몬스터 히트박스 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class monsterHit : MonoBehaviour
{
    // 챕터 스크립트
    public Chapter1Monster Chapter1Monster;
    public Chapter2Monster Chapter2Monster;
    public Chapter3Monster Chapter3Monster;

    // 사망 사운드
    public AudioSource audio;

    MapManager mapManager;
    // 플레이어 
    Transform player;
    // 히트박스의 부모(몬스터 본체)
    public GameObject parent;
    // 드롭 아이템 갯수
    int itemCount;
    // 몬스터 체력
    public Image MonsterHP;
    // 스킬로 맞을시 일정한 시간마다 데미지 부여
    public bool monsterFlag = false;
    // 몬스터 사망 플래그
    public bool monsterdeath = false;
    // 플레이어 데미지 계수 
    float Damage;

    //######################
    // 몬스터 최대 체력
    float MaxHP { get; set; }
    // 현재 게이지 벨류를 이미지 단위에 맞게 계산후 담을 변수
    float currentFill;
    // 현재 체력
    float currentHP;
    public float CurrentHP
    {
        get
        {
            return currentHP;
        }

        set
        {
            if (value > MaxHP) currentHP = MaxHP;
            else if (value < 0) currentHP = 0;
            else currentHP = value;

            currentFill = currentHP / MaxHP;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        MaxHP = 1;
        CurrentHP = 1;

        if (SceneManager.GetActiveScene().name == "Chapter1")
            Damage = 0.005f;
        else if (SceneManager.GetActiveScene().name == "Chapter2")
            Damage = 0.003f;
        if (SceneManager.GetActiveScene().name == "Chapter3")
            Damage = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {

        // 몬스터 체력
        if (currentFill != MonsterHP.fillAmount)
            MonsterHP.fillAmount = Mathf.Lerp(MonsterHP.fillAmount, currentFill, Time.deltaTime * 10f);


        if (MonsterHP.fillAmount <= 0.01f && !monsterdeath)
        {
            monsterdeath = true;
            StartCoroutine("Die");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && _player.attack)
        {
            CurrentHP -= _player.Attack_value * Damage;

            if (SceneManager.GetActiveScene().name == "Chapter1")
                Chapter1Monster.monsterAni.Play("Anim_Damage");

            _player.attack = false;
        }

        if (other.CompareTag("Skill") && !monsterFlag)
            StartCoroutine("HPcool");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Skill") && !monsterFlag)
            StartCoroutine("HPcool");

    }

    IEnumerator HPcool()
    {
        monsterFlag = true;
        if(_player.Upgrade_value =="유니크" || _player.Upgrade_value == "레전드리")
        {
            int headShot = Random.Range(1, 101);

            if(_Upgrade.unique_value == 10 && (headShot <= 5 || headShot >= 95))
                CurrentHP -= 1;
            else if(_Upgrade.unique_value == 20 && (headShot <= 10 || headShot >= 90))
                CurrentHP -= 1;
        }else CurrentHP -= _player.Attack_value * 0.003f;
        yield return new WaitForSeconds(1f);
        monsterFlag = false;
    }

    IEnumerator Die()
    {
        _chapter.CurrentValue += 0.1f;
        itemCount = Random.Range(3, 11);

        for (int i = 0; i <= itemCount; i++)
        {
            int randitem = Random.Range(1, 101);

            if (randitem >= 25 && randitem <= 75)
                Instantiate(mapManager.itemList[0], parent.transform.position + (Vector3.up * 5), Quaternion.identity);
            else if (randitem >= 90)
                Instantiate(mapManager.itemList[1], parent.transform.position + (Vector3.up * 5), Quaternion.identity);
            else if (randitem <= 5)
                Instantiate(mapManager.itemList[2], parent.transform.position + (Vector3.up * 5), Quaternion.identity);
        }

        //몬스터 잡는 횟수 증가
        _Quest.current_count[0]++;
        _player.CurrentExp += 0.3f;
        audio.Play();
        if (SceneManager.GetActiveScene().name == "Chapter1")
            Chapter1Monster.monsterAni.Play("Anim_Death");

        yield return new WaitForSeconds(3f);
        Destroy(parent);
    }

}
