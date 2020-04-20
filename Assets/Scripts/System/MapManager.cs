/**
 * 
 *  스크립트 이름 : MapGenerater.cs
 *  스크립트 용도 : 챕터 별 스테이지를 랜덤으로 생성
 *                  첫 번째 맵에는 들어온 포탈 생성
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    // 드롭 아이템 리스트( 0 = coin / 1 = unique / 2 = dia)
    public List<GameObject> itemList = new List<GameObject>();

    public Image MapValue;
    public GameObject Map;
    public GameObject Boss;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "Boss")
        {
            Map.SetActive(true);
            Boss.SetActive(false);
            Initialize(0, 1);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Boss")
        {
            if (_chapter.currentFill != MapValue.fillAmount)
                MapValue.fillAmount = Mathf.Lerp(MapValue.fillAmount, _chapter.currentFill, Time.deltaTime * 2.5f);

            if (_Monster.BossSpawn)
            {
                Map.SetActive(false);
                Boss.SetActive(true);
            }
        }

    }
    // 맵 게이지 초기화
    public void Initialize(float currentValue, float maxValue)
    {
        _chapter.MaxValue = maxValue;
        _chapter.CurrentValue = currentValue;
    }

}
