/**
 * 
 *  스크립트 이름 : Shop.cs
 *  스크립트 용도 : 상점을 담당
 *  
 **/
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // 인벤토리 임포트
    public Inventory inventory;
    // 게임매니져 임포트
    public Gamemanager gamemanager;
    // 상점 UI
    public GameObject Shop_backgroud;
    // 현재 플레이어 코인을 가져옴
    public Text Player_coin;

    void Update()
    {
        // 플레이어의 코인을 표시
        Player_coin.text = (_player.Coin).ToString();

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(2))
            if (other.CompareTag("Player") && !_player.shopOverlap)
            {
                Shop_backgroud.SetActive(true);
                Cursor.visible = true;
                _player.shop = true; // 이동,마우스회전 잠금
                Cursor.lockState = CursorLockMode.None;
                _player.shopOverlap = true; // 중복 방지
            }
    }

    public void BuyPotion()
    {
        // hp 포션 구매
        if (_player.Coin >= 500)
        {
            _player.Coin -= 500;
            inventory.Add("HP_Potion");
        }
        else gamemanager.State("금화가 부족합니다!");
    }

    public void BuyExpPotion()
    {
        // EXP 포션 구매
        if (_player.Coin >= 50000)
        {
            _player.Coin -= 50000;
            inventory.Add("EXP_Potion");
        }
        else gamemanager.State("금화가 부족합니다!");
    }

    public void BuyLevelPotion()
    {
        // LEVEL 포션 구매
        if (_player.Dia >= 50)
        {
            _player.Dia -= 50;
            inventory.Add("LEVEL_Potion");
        }
        else gamemanager.State("다이아가 부족합니다!");
    }

    public void offShop()
    {
        _player.shop = false;
        _player.shopOverlap = false;
        Shop_backgroud.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
