/**
 * 
 *  스크립트 이름 : FinalBoss.cs
 *  스크립트 용도 : 최종 보스 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class FinalBoss : MonoBehaviour
{
    // 소환할 몬스터
    public List<GameObject> monster = new List<GameObject>();

    // 보스 체력 이미지
    public Image FirstHp;
    public Image SecondHp;

    // 2페이즈 이팩트
    public GameObject PageEffect;

    // 보스 플래그
    bool Page2 = false;
    bool Death = false;
    bool Playerskill = false;
    bool flag = false;
    bool playerin = false;
    bool bossattack = false;

    // 기타 플래그
    bool LightOff = false;

    // 네비게이션
    NavMeshAgent nvAgent;
    // 애니메이터
    Animator BossAni;
    // 플레이어
    Transform player;
    // 스크립트 참고
    MapManager mapManager;   
    // 랜덤 x 좌표
    float xPos;
    // 랜덤 z 좌표
    float zPos;
    // 몬스터 소환 라이트
    public Light SpawnLight;
    // 페이드인 아웃 그림
    public Image fadeinout;
    public FaidInOut FaidInOut;
    public SaveManager saveManager;

    //######################
    // 보스 체력
    public static float MaxValue { get; set; }
    // 벨류를 이미지 단위에 맞게 계산후 담을 변수
    public static float currentFill;
    // 현재 벨류
    private static float currentValue;
    public static float CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MaxValue) currentValue = MaxValue;
            else if (value < 0) currentValue = 0;
            else currentValue = value;

            currentFill = currentValue / MaxValue;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        BossAni = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = gameObject.GetComponent<NavMeshAgent>();

        MaxValue = 1;
        CurrentValue = 1;
        nvAgent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (FirstHp.fillAmount != currentFill && !Page2)
            FirstHp.fillAmount = Mathf.Lerp(FirstHp.fillAmount, currentFill, Time.deltaTime * 10f);
        else if (Page2 && SecondHp.fillAmount != currentFill)
            SecondHp.fillAmount = Mathf.Lerp(SecondHp.fillAmount, currentFill, Time.deltaTime * 10f);

        // 보스 2페이즈 돌입
        if (FirstHp.fillAmount <= 0.01 & !Page2)
        {
            CurrentValue = 1;
            Page2 = true;
            StartCoroutine("PageChange");
        }


        // 보스 사망 판정
        if (SecondHp.fillAmount <= 0.01f && !Death)
        {
            Death = true;
            nvAgent.enabled = false;
            StartCoroutine("bossDeath");
        }

        // AI
        if (!Death)
        {
            transform.LookAt(player);
            nvAgent.SetDestination(player.position);
        }

        // 몬스터 소환 라이트 삭제
        if (GameObject.Find("SpawnLight(Clone)") && LightOff)
            Destroy(GameObject.Find("SpawnLight(Clone)"));
        else
            LightOff = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bossattack)
        {
            nvAgent.enabled = false;
            playerin = true;
            StartCoroutine("Attack");
        }

        // 검 공격 을 받았을 시
        if (other.CompareTag("Sword") && _player.attack && !Death && !flag)
        {
            CurrentValue -= _player.Attack_value * 0.00005f;
            BossAni.SetTrigger("isDamage");
            _player.attack = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !bossattack)
        {
            nvAgent.enabled = false;
            playerin = true;
            StartCoroutine("Attack");
        }

        // 스킬공격 받았을 시
        if (other.CompareTag("Skill") && !Death && !Playerskill)
            StartCoroutine("HPcool");

        // 검 공격 을 받았을 시
        if (other.CompareTag("Sword") && _player.attack && !Death && !flag)
        {
            CurrentValue -= _player.Attack_value * 0.00005f;           
            BossAni.SetTrigger("isDamage");
            _player.attack = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !flag && !Death)
        {
            nvAgent.enabled = true;
            playerin = false;
        }
    }

    IEnumerator Spawn()
    {
        // 몬스터 스폰 전 라이트 소환, 2초뒤 그자리에 몬스터 스폰
        xPos = Random.Range(-35f, 35.1f);
        zPos = Random.Range(45f, -50.1f);

        int mon = Random.Range(2, 6);        

        Vector3 monsterPos = new Vector3(xPos, 2.25f, zPos);

        Instantiate(SpawnLight, monsterPos + (Vector3.up * 5f), SpawnLight.transform.rotation);

        yield return new WaitForSeconds(2f);

        LightOff = true;
        Instantiate(monster[mon], monsterPos, Quaternion.identity);
    }

    IEnumerator Attack()
    {
        int ranAttack = Random.Range(0, 11);

        bossattack = true;
        if (0 <= ranAttack && ranAttack <= 4)
            BossAni.SetTrigger("isAttack1");
        else if (5 <= ranAttack && ranAttack <= 8)
            BossAni.SetTrigger("isAttack2");
        else
        {
            for (int i = 0; i < monster.Count; i++)
                StartCoroutine("Spawn");
        }
        yield return new WaitForSeconds(1.5f);

        if (playerin)
            _player.CurrentHp -= 0.3f;
        bossattack = false;
    }

    IEnumerator HPcool()
    {
        // 스킬공격 받을 시 3초마다 데미지 입음
        Playerskill = true;
        CurrentValue -= _player.Attack_value * 0.00001f;
        BossAni.SetTrigger("isDamage");
        yield return new WaitForSeconds(1f);
        Playerskill = false;
    }

    IEnumerator PageChange()
    {
        GameObject child = Instantiate(PageEffect, transform.position, PageEffect.transform.rotation);
        child.transform.parent = transform;

        nvAgent.enabled = false;
        flag = true;

        BossAni.SetTrigger("isPage1");

        yield return new WaitForSeconds(4f);

        flag = false;
        BossAni.SetBool("isPage2", true);
        nvAgent.enabled = true;

        if (GameObject.Find("FlameEmission(Clone)"))
            Destroy(GameObject.Find("FlameEmission(Clone)"));

    }

    IEnumerator bossDeath()
    {
        // 보스 사망
        int itemCount = Random.Range(90, 100);
        for (int i = 0; i <= itemCount; i++)
        {
            int randitem = Random.Range(1, 101);
            if (randitem >= 25 && randitem <= 75)
                Instantiate(mapManager.itemList[0], transform.position + (Vector3.up * 10), Quaternion.identity);
            else if (randitem >= 90)
                Instantiate(mapManager.itemList[1], transform.position + (Vector3.up * 7), Quaternion.identity);
            else if (randitem <= 5)
                Instantiate(mapManager.itemList[2], transform.position + (Vector3.up * 5), Quaternion.identity);
        }

        BossAni.SetTrigger("isDeath");
        _player.level += 20;
        _player.CurrentExp += 1;
        yield return new WaitForSeconds(3f);
        _chapter.Boss = true;
        _Quest.current_count[2] = 1;

        saveManager.save_data();
        fadeinout.gameObject.SetActive(true);
        _Faid.NextScene = "Ending";
        FaidInOut.InStartFadeAnim();
    }
}
