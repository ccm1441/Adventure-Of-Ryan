/**
 * 
 *  스크립트 이름 : SaveUIOpen.cs
 *  스크립트 용도 : 세이브 UI 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIOpen : MonoBehaviour
{
    public Image Save_background;
    
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(2))
            if (other.CompareTag("Player") && !_player.shopOverlap)
            {
                Save_background.gameObject.SetActive(true);
                Cursor.visible = true;
                _player.shop = true; // 이동,마우스회전 잠금
                Cursor.lockState = CursorLockMode.None;
                _player.shopOverlap = true; // 중복 방지
            }
    }

    public void offShop()
    {
        // 저장 UI 닫음
        _player.shop = false;
        _player.shopOverlap = false;
        Save_background.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
