/**
 * 
 *  스크립트 이름 : ItemUse.cs
 *  스크립트 용도 : 각 아이템 사용
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
    string item_name;
       
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            get_itemName(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            get_itemName(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            get_itemName(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            get_itemName(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            get_itemName(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            get_itemName(5);
    }

    void get_itemName(int num)
    {
        // 해당 칸에 있는 아이템 이름을 가져옴
        item_name = _inventory.item_name[num];

        // 해당 칸에 아이템이 있다면 이름과 갯수,위치를 던짐
        if (item_name != null)
            Use(item_name, _inventory.item_count[num], num);
        else Debug.Log("no item");
    }

    void Use(string name, int count, int pos)
    {
        // 아이템 개수가 1개 이상이면
        if (count > 0)
        {
            _inventory.item_count[pos]--;

            item_option(name); // 각 아이템의 능력 발동

            //아이템 사용후 개수가 0개면 인벤토리에서 아이템 삭제
            if (_inventory.item_count[pos] == 0)
            {
                _inventory.item_name[pos] = null;
                GameObject des_item = GameObject.Find(name);
                if (des_item)
                    Destroy(des_item);
                _inventory.full_count--;
            }
        }
    }

    void item_option(string name)
    {
        // 각 아이템 마다 능력을 나열 및 실행
        switch (name)
        {
            case "HP_Potion(Clone)":
                _player.CurrentHp += 0.3f;
                break;
            case "EXP_Potion(Clone)":
                _player.CurrentExp += 0.2f;
                break;
            case "LEVEL_Potion(Clone)":
                _player.CurrentExp += 1f;
                break;
            default:
                break;
        }
    }
}
