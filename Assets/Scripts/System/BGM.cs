/**
 * 
 *  스크립트 이름 : BGM.cs
 *  스크립트 용도 : 메인 브금 관리
 *  
 **/
using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    int count = 0;
    void Start()
    {
        foreach (GameObject bgm in GameObject.FindGameObjectsWithTag("bgm"))
        {
            count++;
        }

        if (count >= 2)
            Destroy(gameObject);

   
        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();

       
    }
}
