/**
 * 
 *  스크립트 이름 : Titlemanager.cs
 *  스크립트 용도 : 타이틀 총괄
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


public class Titlemanager : MonoBehaviour
{
    // 페이드인 아웃 그림
    public Image fadeinout;
    public FaidInOut FaidInOut;

    // 상태 바
    public Animator State_bar;
    public Text State_text;

    // 파일 경로
    string savepath = "Adventure.save";

    private void Awake() => Screen.SetResolution(1280, 800, false);

    public void Gamestart()
    {
        fadeinout.gameObject.SetActive(true);
        _Faid.NextScene = "Story";
        FaidInOut.InStartFadeAnim();
    }

    public void Dataload()
    {
        if (File.Exists(savepath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savepath, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            _player.init = save.init;
            _player.level = save.level;
             _player.Coin = save.Coin;
            _player.Dia= save.Dia;
            _player.Unique=save.Unique;
            _player.CurrentHp= save.HP;
            _player.CurrentExp= save.EXP;
            _player.Attack_value= save.Attack_value;
            _player.Upgrade_value= save.Upgrade_value;

            _chapter.Chapter1= save.Chapter1;
            _chapter.Chapter2= save.Chapter2;
            _chapter.Chapter3= save.Chapter3;
           _chapter.Boss = save.Boss;

            _inventory.inventory_full= save.inventoryFull;
            _inventory.full_count= save.InventoryCount;

            _Quest.quest_init= save.QueatInit;

            for (int i = 0; i < 6; i++)
            {
                _inventory.item_count[i]= save.item_count[i];
                _inventory.item_name[i]= save.item_name[i];

                _Quest.current_count[i]= save.current_count[i];
                _Quest.need_count[i]= save.need_count[i];
                _Quest.quest_clear[i]= save.quest_clear[i];
            }

            _Upgrade.need_coin= save.need_coin;
            _Upgrade.need_dia= save.need_dia;
            _Upgrade.need_unique= save.need_unique;
            _Upgrade.chance_value= save.chance_value;
            _Upgrade.attack_value= save.attack_value;
            _Upgrade.unique_value= save.unique_value;
            _Upgrade.weapon_effect= save.weapon_effect;

            _inventory.invenLoad = true;
            fadeinout.gameObject.SetActive(true);
            _Faid.NextScene = "Main";
            FaidInOut.InStartFadeAnim();
        }           
        else State("세이브 파일이 없습니다!");
    }

    public void State(string text)
    {
        State_text.text = text;
        State_bar.SetBool("move", true);
        StartCoroutine("State_s");
    }

    IEnumerator State_s()
    {
        yield return new WaitForSeconds(1.5f);
        State_bar.SetBool("move", false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
