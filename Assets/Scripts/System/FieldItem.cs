/**
 * 
 *  스크립트 이름 : FieldItem.cs
 *  스크립트 용도 : 드랍된 아이템을 자동으로 유저에게 이동하여 얻도록 함
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldItem : MonoBehaviour
{
    GameObject Player;
    bool get = false;
    float speed;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("GetItem");
        audioSource = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().name == "Boss")
            speed = 3f;
        else speed = 0.7f;

    }

    // Update is called once per frame
    void Update()
    {
        // 아이템 위치 에서 플레이어 위치까지 선형 보간 이동
        if (get)
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + Vector3.up * 2, speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            StartCoroutine("delete");           
        }
    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }

    IEnumerator GetItem()
    {
        yield return new WaitForSeconds(0.6f);
        get = true;
    }
}
