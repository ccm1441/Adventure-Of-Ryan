/**
 * 
 *  스크립트 이름 : PlayerManager.cs
 *  스크립트 용도 : 플레이어의 상태(체력,레벨 등) 및 다른 오브젝트와 충돌 여부를 판단
 *  
 **/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    private MapManager MapManager;
    public SampleAnimation sampleAnimation;

    // 플레이어 체력 부분
    [SerializeField]
    private float lerpSpeed;
    public Image Player_hpBar;

    // 플레이어 경험치 부분
    public Image Player_expBar;

    // 플레이어 레벨 부분
    public Text Player_lavel;

    // 플에이어 재화 부분
    public Text Coin;
    public Text Dia;
    public Text Unique;

    // 플레이어 장비 창
    public GameObject Equipment_UI;
    public Text Attack_value;
    public Text Upgrade_value;
    public Text Unique_value;

    // 무기 이팩트
    public ParticleSystem weapon_effect;

    // 레벨업 이펙트
    public GameObject LevelUp;

    // 페이드 인 아웃
    public Image fadeinout;
    public FaidInOut FaidInOut;

    // 플레이어 사망시 오브젝트
    public GameObject DeathObj;

    // 사망 카운트
    public Text DeathText;
    float DeathTime = 4;



    private void Start()
    {
        if (GameObject.Find("MapManager"))
            MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();

        Player_lavel.text = _player.level.ToString();

        if (!_player.init)
            Initialize(1, 0);
    }

    private void Update()
    {
        // 플레이어 체력과 경험치 바 계산 부분
        if (_player.currentFill != Player_hpBar.fillAmount)
            Player_hpBar.fillAmount = Mathf.Lerp(Player_hpBar.fillAmount, _player.currentFill, Time.deltaTime * lerpSpeed);
        if (_player.currentExpFill != Player_expBar.fillAmount)
            Player_expBar.fillAmount = Mathf.Lerp(Player_expBar.fillAmount, _player.currentExpFill, Time.deltaTime * lerpSpeed);

        // 플레이어 레벨업 부분
        if (Player_expBar.fillAmount >= 0.98f)
        {
            sampleAnimation.audioSources[1].volume = 0.2f;
            sampleAnimation.audioSources[1].Play();
            _player.level++;

            GameObject child = Instantiate(LevelUp, transform.position, LevelUp.transform.rotation);
            child.transform.parent = transform;
            Player_lavel.text = _player.level.ToString();

            // 상태 조정
            _player.CurrentHp = 1;
            _player.CurrentExp = 0;
            _player.Attack_value += 10;
        }

        // 플레이어가 E 키를 누르면 장비창 로드
        if (Input.GetKeyDown(KeyCode.E))
            Equipment_UI.SetActive(!Equipment_UI.activeSelf);

        // 플레이어 무기 정보 업데이트
        Attack_value.text = "공격력 : " + _player.Attack_value.ToString();
        Upgrade_value.text = "강화 수치 : +" + _player.Upgrade_value;
        if (_player.Upgrade_value == "유니크" || _player.Upgrade_value == "레전드리")
            Unique_value.text = _Upgrade.unique_value + "% 확률로 몬스터가 즉사합니다.";


        // 무기 이팩트 활성화
        if (_Upgrade.weapon_effect)
            weapon_effect.Play();

        // 플레이어 재화 업데이트
        Coin.text = _player.Coin.ToString();
        Dia.text = _player.Dia.ToString();
        Unique.text = _player.Unique.ToString();

        // 플레이어 사망
        if (_player.Death)
        {
            DeathObj.SetActive(true);

            DeathTime -= Time.deltaTime;
            DeathText.text = (int)DeathTime + ("초 뒤 마을에서 부활");

            if (DeathTime <= 0)
                faidin("Main");

            // 플레이어 사망 패널티
            _player.Coin = 0;
            _player.Unique = 0;
            _player.CurrentExp = 0;

            if (_player.Upgrade_value == "유니크" || _player.Upgrade_value == "레전드리") { }
            else
            {
                _player.Attack_value = 100;
                _player.Upgrade_value = "0";
            }

            _chapter.ChapterInOut = false;

            for (int i = 0; i < 6; i++)
            {
                _inventory.item_count[i] = 0;
                _inventory.item_name[i] = null;
                _inventory.full_count = 0;
            }
        }
    }

    // 플레이어 충돌 부분
    private void OnTriggerEnter(Collider other)
    {
        // 포탈 클리어 부분
        // 임시 부분임
        if (!_chapter.ChapterInOut)
        {
            if (other.CompareTag("Portal") && other.name == "Chapter1")
            {
                _chapter.CurrentChapter = 1;
                _chapter.ChapterInOut = true;
                faidin("Chapter1");
            }
            else if (other.CompareTag("Portal") && other.name == "Chapter2" && _chapter.Chapter1 == true)
            {
                _chapter.CurrentChapter = 2;
                _chapter.ChapterInOut = true;
                faidin("Chapter2");
            }
            else if (other.CompareTag("Portal") && other.name == "Chapter3" && _chapter.Chapter2 == true)
            {
                _chapter.CurrentChapter = 3;
                _chapter.ChapterInOut = true;
                faidin("Chapter3");
            }
            else if (other.CompareTag("Portal") && other.name == "Boss" && _chapter.Chapter3 == true && !_chapter.Boss)
            {
                _chapter.CurrentChapter = 4;
                _chapter.ChapterInOut = true;
                faidin("Boss");
            }
        }

        // 챕터 속 맵 클리어 부분
        if (other.CompareTag("Portal") && other.name == "next_portal")
            _chapter.MapClear = true;
        else if (other.CompareTag("Portal") && other.name == "MAP_out_portal")
        {
            // 중간 보스 처치 후 챕터 1 클리어 처리
            if (SceneManager.GetActiveScene().name == "Chapter1" && _Monster.chapter1Boss)
                _chapter.Chapter1 = true;
            if (SceneManager.GetActiveScene().name == "Chapter2" && _Monster.chapter2Boss)
                _chapter.Chapter2 = true;
            if (SceneManager.GetActiveScene().name == "Chapter3" && _Monster.chapter3Boss)
                _chapter.Chapter3 = true;
            _chapter.ChapterInOut = false;
            faidin("Main");
        }

        // 아이템 섭취
        if (other.CompareTag("Item") && other.name == "Pirate Coin(Clone)")
            _player.Coin += 150;
        else if (other.CompareTag("Item") && other.name == "scroll")
            _player.Unique++;
        if (other.CompareTag("Item") && other.name == "Diamondo(Clone)")
            _player.Dia++;

        // 바다에 빠질시
        if (other.CompareTag("Ocean"))
            _player.CurrentHp = 0;
    }

    void faidin(string name)
    {
        // 던전 입장시 페이드 인 이펙트
        fadeinout.gameObject.SetActive(true);
        _Faid.NextScene = name;
        FaidInOut.InStartFadeAnim();
    }

    // 플레이어 체력 초기화
    public void Initialize(float currentValue, float currendExp)
    {
        // 플레이어 체력 초기화
        _player.CurrentHp = currentValue;

        // 플레이어 경험치 초기화
        _player.CurrentExp = currendExp;

        // 플레이어 재화 초기화
        _player.Coin = 10000;
        _player.Dia = 100;
        _player.Unique = 0;

        // 플레이어 무기 초기화
        _player.Attack_value = 100;
        _player.Upgrade_value = "0";

        _player.init = true;

    }
}
