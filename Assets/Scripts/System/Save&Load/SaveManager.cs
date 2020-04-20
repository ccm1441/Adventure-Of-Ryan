/**
 * 
 *  스크립트 이름 : SaveManager.cs
 *  스크립트 용도 : 세이브 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public Gamemanager gamemanager;
    string button_name;
    string savepath = "Adventure.save";

    public void save_data()
    {
        if(SceneManager.GetActiveScene().name != "Boss")
        button_name = EventSystem.current.currentSelectedGameObject.name;

        // 저장할 데이터를 생성후 파일 저장
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();

        if (!File.Exists(savepath))
        {
            FileStream file = File.Create(savepath);
            bf.Serialize(file, save);
            file.Close();
        }
        else
        {
            File.Delete(savepath);
            FileStream file = File.Create(savepath);
            bf.Serialize(file, save);
            file.Close();
        }

        gamemanager.State("저장 완료");

        if (button_name == "saveexit")
            Application.Quit();
        else if (button_name == "saveTitle")
        {
            if (GameObject.Find("BGM"))
                Destroy(GameObject.Find("BGM"));
            SceneManager.LoadSceneAsync("Title");
        }
    }

    private Save CreateSaveGameObject()
    {
        // 현재 게임 데이터를 세이브 클래스에 담은 뒤 리턴
        Save save = new Save();

        save.init = _player.init;
        save.level = _player.level;
        save.Coin = _player.Coin;
        save.Dia = _player.Dia;
        save.Unique = _player.Unique;
        save.HP = _player.CurrentHp;
        save.EXP = _player.CurrentExp;
        save.Attack_value = _player.Attack_value;
        save.Upgrade_value = _player.Upgrade_value;

        save.Chapter1 = _chapter.Chapter1;
        save.Chapter2 = _chapter.Chapter2;
        save.Chapter3 = _chapter.Chapter3;
        save.Boss = _chapter.Boss;

        save.inventoryFull = _inventory.inventory_full;
        save.InventoryCount = _inventory.full_count;

        save.QueatInit = _Quest.quest_init;

        for (int i = 0; i < 6; i++)
        {
            save.item_count[i] = _inventory.item_count[i];
            save.item_name[i] = _inventory.item_name[i];

            save.current_count[i] = _Quest.current_count[i];
            save.need_count[i] = _Quest.need_count[i];
            save.quest_clear[i] = _Quest.quest_clear[i];
        }

        save.need_coin = _Upgrade.need_coin;
        save.need_dia = _Upgrade.need_dia;
        save.need_unique = _Upgrade.need_unique;
        save.chance_value = _Upgrade.chance_value;
        save.attack_value = _Upgrade.attack_value;
        save.unique_value = _Upgrade.unique_value;
        save.weapon_effect = _Upgrade.weapon_effect;

        return save;
    }
}

// 세이브 할 데이터 명시
[System.Serializable]
public class Save
{
    // 플레이어
    public bool init;
    public int level, Coin, Dia, Unique;
    public float HP, EXP;
    public int Attack_value;
    public string Upgrade_value;

    // 챕터
    public bool Chapter1, Chapter2, Chapter3, Boss;

    // 인벤토리
    public bool inventoryFull;
    public int InventoryCount;
    public int[] item_count = new int[6];
    public string[] item_name = new string[6];

    // 업적
    public bool QueatInit;
    public int[] current_count = new int[6];
    public int[] need_count = new int[6];
    public bool[] quest_clear = new bool[6];

    //업그레이드
    public int need_coin, need_dia, need_unique;
    public int chance_value, attack_value, unique_value;
    public bool weapon_effect;
  
}
