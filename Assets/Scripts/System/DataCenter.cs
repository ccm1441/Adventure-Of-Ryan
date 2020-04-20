
// 챕터 공유 클래스
using System.Collections.Generic;
using UnityEngine;
/**
* 
*  스크립트 이름 : DataCenter.cs
*  스크립트 용도 : 공유 하는 모든 데이터를 저장, 각 스크립트에서 공통으로 사용
*  
**/

// 챕터 클리어 여부
public static class _chapter
{
    // 챕터 클리어 여부
    public static bool Chapter1 { get; set; }
    public static bool Chapter2 { get; set; }
    public static bool Chapter3 { get; set; }
    public static bool Boss { get; set; }

    // 맵 클리어 여부
    public static bool MapClear { get; set; }

    // 챕터 이동 여부
    // 무슨 챕터로 들어갔는지 저장
    // 1 => chapter1 // 2 => chapter2 // 3 => chapter3
    public static int CurrentChapter = 0;
    // 챕터 in-out 판단
    public static bool ChapterInOut = false;

    //######################
    // 챕터 최대 게이지
    public static float MaxValue { get; set; }
    // 현재 게이지 벨류를 이미지 단위에 맞게 계산후 담을 변수
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

}

// 플레이어 관련 공유 클래스
public static class _player
{
    // 최초 플레이어 상태 초기화 여부
    public static bool init = false;
    // 플레이어 레벨
    public static int level = 1;
    // 플레이어 상점 오픈 여부(키,마우스 회전 잠금)
    public static bool shop = false;
    // 플레이어 상점 중복 오픈 방지
    public static bool shopOverlap = false;
    // 플레이어 코인
    public static int Coin { get; set; }
    // 플레이어 다이아
    public static int Dia { get; set; }
    // 플레이어 무기 전용 재화
    public static int Unique { get; set; }
    // 플레이어 무기 공격력
    public static int Attack_value { get; set; }
    // 플레이어 무기 강화수치
    public static string Upgrade_value { get; set; }
    // 플레이어 공격 여부
    public static bool attack = false;
    // 플레이어 사망 여부
    public static bool Death = false;

    //######################
    // 플레이어 최대 체력
    public static float MaxHp = 1;
    // 현재 체력을 이미지 단위에 맞게 계산후 담을 변수
    public static float currentFill;
    // 현재 체력
    private static float currentHp;
    public static float CurrentHp
    {
        get
        {
            return currentHp;
        }

        set
        {
            if (value > MaxHp) currentHp = MaxHp;
            else if (value < 0) currentHp= 0;
            else currentHp= value;

            currentFill = currentHp / MaxHp;
        }
    }

    //######################
    // 플레이어 경험치
    public static float MaxExp = 1;
    // 현재 경험치을 이미지 단위에 맞게 계산후 담을 변수
    public static float currentExpFill;
    // 현재 경험치
    private static float currentExp;
    public static float CurrentExp
    {
        get
        {
            return currentExp;
        }

        set
        {
            if (value > MaxExp) currentExp = MaxExp;
            else if (value < 0) currentExp = 0;
            else currentExp = value;
                       
            currentExpFill = currentExp / MaxExp;
        }
    }
}

// 플레이어 인벤토리
public static class _inventory
{
    // 인벤토리 초기화 여부
    public static bool inventory_init = false;
    // 인벤토리 꽉참 여부
    public static bool inventory_full = false;
    // 사용중인 인벤토리 칸 수
    public static int full_count = 0;
    // 인벤토리에 표시할 이미지
    public static List<GameObject> item_obj = new List<GameObject>();
    // 각 칸에 아이템 갯수
    public static int[] item_count = new int[6];
    // 각 칸의 아이템 이름 저장(찾기 편하게)
    public static string[] item_name = new string[6];
    // 로드 후 인벤토리 이미지 로드를 위해 플래그
    public static bool invenLoad = false;
}

// 플레이어 스킬
public static class _Skill
{
    // 현재 쿨타임
    public static float skillFirstFill = 1;
    public static float skillSecondFill = 1;
    public static float skillBuffFill = 1;

    // 스킬 쿨 타임 여부
   public static bool skillFirstCoolTime = false; // 평타
    public static bool skillSecondCoolTime = false; // 스킬
    public static bool skillBuffCoolTime = false; // 버프

    // 공격력 스킬 상승 공격력
    public static int AttackValueUp=0;
}

// 업적
public static class _Quest
{   
    // 업적 초기화 여부
    public static bool quest_init = false;
    // 현재 각 업적마다의 진행도
    public static int[] current_count = new int[6];
    // 각 업적마다 필요도
    public static int[] need_count = new int[6];
    // 업적 클리어 여부
    public static bool[] quest_clear = new bool[6];
}

// 업그레이드
public static class _Upgrade
{
    // 각 강화 단계에 따른 필요한 재화
    public static int need_coin = 2000;
    public static int need_dia = 0;
    public static int need_unique = 0;
    // 강화 확률
    public static int chance_value = 100;
    public static int attack_value = 10;
    // 특수 능력치
    public static int unique_value = 10;
    // 유니크 부터 무기에 이팩트 출력
    public static bool weapon_effect = false;
}

// 몬스터
public static class _Monster
{
    // 보스 소환 여부
    public static bool BossSpawn = false;
    // 보스 클리어 여부
    public static bool chapter1Boss = false;
    public static bool chapter2Boss = false;
    public static bool chapter3Boss = false;

}

// 화면 전환용(페이드 인 아웃)
public static class _Faid
{
    public static string NextScene { get; set; }
}