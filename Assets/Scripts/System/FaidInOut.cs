/**
 * 
 *  스크립트 이름 : FaidInOut.cs
 *  스크립트 용도 : 페이드 인 아웃을 담당, 끝나면 화면 전환
 *  
 **/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaidInOut : MonoBehaviour
{
    public AudioSource audioSource;

    // 페이드 인 아웃 변수
    public float FadeTime = 2f; // Fade효과 재생시간
    Image fadeImg;
    float start;
    float end;
    float time = 0f;
    bool isPlaying = false;

    void Awake()
    {
        fadeImg = GetComponent<Image>();
    }

    public void OutStartFadeAnim()
    {// 페이드 아웃
        if (isPlaying == true) //중복재생방지
            return;

        start = 1f;
        end = 0f;
        StartCoroutine("fadeoutplay");    //코루틴 실행
    }

    public void InStartFadeAnim()
    {// 페이드 인
        if (isPlaying == true) //중복재생방지
            return;

        start = 0f;
        end = 1f;
        StartCoroutine("fadeinplay");
    }

    IEnumerator fadeoutplay()
    {// 페이드 아웃
        isPlaying = true;

        Color fadecolor = fadeImg.color;

        time = 0f;

        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }
        this.gameObject.SetActive(false);
        isPlaying = false;
    }

    IEnumerator fadeinplay()
    {// 페이드인
        isPlaying = true;

        Color fadecolor = fadeImg.color;

        time = 0f;

        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a < 1f)
        {
            time += Time.deltaTime / FadeTime;

            if (SceneManager.GetActiveScene().name == "Title" 
                || SceneManager.GetActiveScene().name == "Story"
                 || SceneManager.GetActiveScene().name == "Ending")
                audioSource.volume -= time * 0.001f;
          
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }        

        if(_Faid.NextScene != null)
            SceneManager.LoadScene(_Faid.NextScene);                 
        
       
        isPlaying = false;
    }
}
