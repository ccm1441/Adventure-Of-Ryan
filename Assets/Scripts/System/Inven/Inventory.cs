/**
 * 
 *  스크립트 이름 : Inventory.cs
 *  스크립트 용도 : 유저 인벤토리 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // 인벤토리에 표시할 이미지
    public List<GameObject> item_obj = new List<GameObject>();
    // 인벤토리 칸
    public List<GameObject> inventory = new List<GameObject>();
    public List<Text> item_count = new List<Text>();

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (_inventory.item_count[i] == 0)
                item_count[i].gameObject.SetActive(false);
            item_count[i].text = _inventory.item_count[i].ToString();

            if (_inventory.full_count == 6)
                _inventory.inventory_full = true;
            else _inventory.inventory_full = false;
        }
    }

    private void Initialized()
    {
        // 인벤토리 초기화
        if (!_inventory.inventory_init)
        {
            for (int i = 0; i < item_obj.Count; i++)
                _inventory.item_obj.Add(item_obj[i]);

            _inventory.inventory_init = true;
        }

        // 데이터 로드 후 저장된 인벤토리 복구
        for (int i = 0; i < _inventory.item_name.Length; i++)
        {
            if (_inventory.item_name[i] != null)
            {
                for (int j = 0; j < _inventory.item_name.Length; j++)
                {
                    if (_inventory.item_name[i] == _inventory.item_obj[j].name + "(Clone)")
                    {
                        GameObject child = Instantiate(_inventory.item_obj[j], inventory[i].transform.position, Quaternion.identity);
                        Transform parent = inventory[i].transform;
                        child.transform.SetParent(parent, false);
                        item_count[i].gameObject.SetActive(true);
                        item_count[i].text = _inventory.item_count.ToString();
                        break;
                    }
                }
            }
        }
    }

    public void Add(string name)
    {
        if (_inventory.inventory_full)
        {
            Debug.Log("inventory full");
            return;
        }


        //아이템 삽입
        int obj_num = 0;

        // 아이템 obj의 위치를 파악
        for (int i = 0; i < inventory.Count; i++)
        {
            if (_inventory.item_obj[i].name == name)
            {
                obj_num = i;
                break;
            }
        }

        // 인벤토리의 아이템 겟수를 통해 인벤토리 검사 후 아이템 더함
        for (int i = 0; i < inventory.Count; i++)
        {
            if (_inventory.item_count[i] == 0)
            {
                // 아이템 추가시 해당 칸의 자식으로 넣음.
                // 해당 칸에 프리팹 생성 및 이름과 카운터 저장
               GameObject child = Instantiate(_inventory.item_obj[obj_num], inventory[i].transform.position, Quaternion.identity,inventory[i].transform);
               

                item_count[i].gameObject.SetActive(true);
                _inventory.item_name[i] = child.name;
                _inventory.item_count[i]++;
                _inventory.full_count++;

                break;
            }
            else
            {
                // 아이템 중복일시 갯수 카운트
                if (inventory[i].transform.GetChild(1).name == name + "(Clone)")
                {
                    _inventory.item_count[i]++;
                    break;
                }
            }
        }
    }
}
