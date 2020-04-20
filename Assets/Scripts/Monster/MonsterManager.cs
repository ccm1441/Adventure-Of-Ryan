/**
 * 
 *  스크립트 이름 : MonsterManager.cs
 *  스크립트 용도 : 몬스터 소환 및 각종 기능 담당
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    // 몬스터 스폰 포탈
    // public GameObject monportal;
    // 몬스터 프리팹
    public GameObject monster;
    // 보스 오브젝트
    public GameObject boss;
    // 보스 스폰 지역
    public GameObject bossSpawn;
    // 랜덤 x 좌표
    float xPos;
    // 랜덤 z 좌표
    float zPos;
    // 소환할 몬스터 카운트
    int monCount = 0;
    // 소환 트리거
    bool startSpawn = false;
    // 몬스터 소환 라이트
    public Light SpawnLight;
    bool LightOff = false;
    // 워닝 텍스트
    public Text Warning;

    void Start()
    {
        spawnMonster(3);
    }

    void Update()
    {
        // 몬스터 소환 라이트 삭제
        if (GameObject.Find("SpawnLight(Clone)") && LightOff)
            Destroy(GameObject.Find("SpawnLight(Clone)"));
        else
            LightOff = false;

        // 맵 게이지 별 몬스터 스폰 및 보스 스폰
        if (_chapter.CurrentValue == 0.3f && !startSpawn)
        {
            monCount = 0;
            spawnMonster(7);
            startSpawn = true;
        }
        else if (_chapter.CurrentValue == 1 && startSpawn)
        {
            StartCoroutine("WarningOn");
        }
    }

    void spawnMonster(int count)
    {
        // 맵 기준 랜덤으로 소환
        while (count != monCount)
        {
            StartCoroutine("Spawn");

            monCount++;
        }
    }

    IEnumerator Spawn()
    {
        // 몬스터 스폰 전 라이트 소환, 2초뒤 그자리에 몬스터 스폰
        xPos = Random.Range(-35f, 35.1f);
        zPos = Random.Range(45f, -50.1f);

        Vector3 monsterPos = new Vector3(xPos, 2.25f, zPos);

        Instantiate(SpawnLight, monsterPos + (Vector3.up * 5f), SpawnLight.transform.rotation);

        yield return new WaitForSeconds(2f);
        LightOff = true;
        Instantiate(monster, monsterPos, Quaternion.identity);
    }

    void spawnBoss()
    {
        // 보스 스폰
        Instantiate(boss, bossSpawn.transform.position, boss.transform.rotation);
        _Monster.BossSpawn = true;
    }

    IEnumerator WarningOn()
    {
        startSpawn = false;
        Warning.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Warning.gameObject.SetActive(false);
        spawnBoss();        
    }

}
