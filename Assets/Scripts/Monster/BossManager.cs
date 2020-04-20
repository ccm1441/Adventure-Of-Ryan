/**
 * 
 *  스크립트 이름 : BossManager.cs
 *  스크립트 용도 : 보스 매니저
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    // 맵 매니저 참고
    MapManager mapManager;
    // 보스 프리팹
    public GameObject boss;
    // 보스 소환 이팩트
    public ParticleSystem bossEnter;
    // 플레이어
    Transform player;
    // 보스 AI
    NavMeshAgent nvAgentBoss;
    // 보스가 떨어지고 땅에 닿았을때
    bool groundArray = false;
    // 보스 체력
    Image BossValue;
    // 보스 플래그
    bool BossFlag = false;
    bool BossDeath = false;
    // 드롭 아이템 갯수
    int itemCount = 1;
    // 보스 애니메이터
    public Animator BossAni;
    // 보스 사망 사운드
    public AudioSource audio;

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

    private void Start()
    {
        // 각종 컴포넌트 대입
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgentBoss = gameObject.GetComponent<NavMeshAgent>();    
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        BossAni = GetComponent<Animator>();

        // 네비게이션 실행
        nvAgentBoss.enabled = false;
        
        // HP 조정
        MaxHP = 1;
        CurrentHP = 1;

        // 걷는 모션 실행
        BossAni.SetBool("Run Forward", true);
    }

    private void Update()
    {
        // 보스 채력을 시각화 함
        BossValue = GameObject.Find("Chapter_boss").GetComponent<Image>();

        if (currentFill != BossValue.fillAmount)
            BossValue.fillAmount = Mathf.Lerp(BossValue.fillAmount, currentFill, Time.deltaTime * 10f);

        // 보스 사망 판정
        if (BossValue.fillAmount <= 0.01f && !BossDeath)
        {
            BossDeath = true;
            BossAni.SetBool("Run Forward", false);
            nvAgentBoss.enabled = false;
            StartCoroutine("BossDie");
        }

        // 보스가 하늘에서 떨어지면 낙하 이팩트 실행
        if (groundArray && !BossDeath)
        {
            nvAgentBoss.enabled = true;
            nvAgentBoss.destination = player.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 진입 시
        if (other.CompareTag("Player") && !BossDeath)
        {
            groundArray = false;
            nvAgentBoss.enabled = false;
        }

        // 땅에 착지 했을 시
        if (other.CompareTag("Ground") && !groundArray)
        {
            groundArray = true;
            bossEnter.Play();
        }

        // 검 공격 을 받았을 시
        if (other.CompareTag("Sword") && _player.attack && !BossDeath)
        {
            CurrentHP -= _player.Attack_value * 0.0003f;
            BossAni.SetTrigger("Take Damage");
            _player.attack = false;
        }

        // 스킬공격 받았을 시
        if (other.CompareTag("Skill") && !BossFlag && !BossDeath)
            StartCoroutine("HPcool");
    }

    private void OnTriggerStay(Collider other)
    {
        // 스킬공격 받고 있을 시
        if (other.CompareTag("Skill") && !BossFlag && !BossDeath)
            StartCoroutine("HPcool");
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 공격 범위를 나갔을 시 
        if (other.CompareTag("Player"))
        {
            groundArray = true;
            nvAgentBoss.enabled = true;
        }
    }

    IEnumerator HPcool()
    {
        // 스킬공격 받을 시 3초마다 데미지 입음
        BossFlag = true;
        CurrentHP -= _player.Attack_value * 0.0003f;
        BossAni.SetTrigger("Take Damage");
        yield return new WaitForSeconds(1f);
        BossFlag = false;
    }

    IEnumerator BossDie()
    {
        // 보스 사망
        itemCount = Random.Range(20, 30);

        for (int i = 0; i <= itemCount; i++)
        {
            int randitem = Random.Range(1, 101);
            if (randitem >= 25 && randitem <= 75)
                Instantiate(mapManager.itemList[0], transform.position + (Vector3.up * 5), Quaternion.identity);
            else if (randitem >= 90)
                Instantiate(mapManager.itemList[1], transform.position + (Vector3.up * 5), Quaternion.identity);
            else if (randitem <= 5)
                Instantiate(mapManager.itemList[2], transform.position + (Vector3.up * 5), Quaternion.identity);
        }

        if (SceneManager.GetActiveScene().name == "Chapter1")
            _Monster.chapter1Boss = true;
        else if (SceneManager.GetActiveScene().name == "Chapter2")
            _Monster.chapter2Boss = true;
        else if (SceneManager.GetActiveScene().name == "Chapter3")
            _Monster.chapter3Boss = true;

        _player.CurrentExp += 1;
        _Monster.BossSpawn = false;

        audio.Play();
        BossAni.SetTrigger("Die");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
