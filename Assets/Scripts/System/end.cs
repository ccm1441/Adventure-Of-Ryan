/**
 * 
 *  스크립트 이름 : end.cs
 *  스크립트 용도 : 엔딩씬 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour
{
    public Text storyText;
    public Text last;
    string writerText;

    // 페이드인 아웃 그림
    public Image fadeinout;
    public FaidInOut FaidInOut;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("BGM"))
            Destroy(GameObject.Find("BGM"));

        Cursor.visible = false;
        fadeinout.gameObject.SetActive(true);
         FaidInOut.OutStartFadeAnim();
        StartCoroutine("data");
    }

    IEnumerator Story(string narration, int se)
    {
        writerText = "";
        if (se == 1)
        {
            for (int i = 0; i < narration.Length; i++)
            {
                writerText += narration[i];
                yield return new WaitForSeconds(0.01f);
                storyText.text = writerText;
                yield return null;
            }
        }
        else if (se == 2)
        {
            for (int i = 0; i < narration.Length; i++)
            {
                writerText += narration[i];
                yield return new WaitForSeconds(0.01f);
                last.text = writerText;
                yield return null;
            }
        }
    }

    IEnumerator data()
    {
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("드디어 몬스터들의 최종 보스를 죽였다...",1));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Story("플레이어님 고마워요!", 1));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Story("저와 함께 싸워주셔서..",1));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Story("이제 마을은 영원한 평화를 얻게 되었어요!",1));
        yield return new WaitForSeconds(2f);
        storyText.gameObject.SetActive(false);
        yield return new WaitForSeconds(6f);
        yield return StartCoroutine(Story("기획\n최철민", 2));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("프로그래밍\n최철민", 2));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("에셋\n유니티 에셋스토어", 2));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("애니메이션\nMixamo", 2));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("도와주신분\n[포탈,제단 모델링]\n전지빈\n구글", 2));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Story("즐겨주셔서\n감사합니다!", 2));
        yield return new WaitForSeconds(4f);
        fadeinout.gameObject.SetActive(true);
        _Faid.NextScene = "Title";
        FaidInOut.InStartFadeAnim();
    }
}
