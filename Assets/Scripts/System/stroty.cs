/**
 * 
 *  스크립트 이름 : stroty.cs
 *  스크립트 용도 : 스토리씬 관리
 *  
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stroty : MonoBehaviour
{
    public Text storyText;
    string writerText;

    // 페이드인 아웃 그림
    public Image fadeinout;
    public FaidInOut FaidInOut;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        fadeinout.gameObject.SetActive(true);
        FaidInOut.OutStartFadeAnim();
        StartCoroutine("data");
    }

    IEnumerator Story(string narration)
    {
        writerText = "";

        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            yield return new WaitForSeconds(0.01f);
            storyText.text = writerText;
            yield return null;
        }
    }

    IEnumerator data()
    {
        yield return StartCoroutine(Story("몬스터로 둘러싸인 마을 한 가운데에는 칼이 꽂힌 제단이 있다."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(Story("이 칼은 선택 받은 자를 만나면 강력한 빛이 나온다고 한다."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(Story("몇 년동안 수 많은 사람들이 칼을 잡아 보았지만 빛은 나지 않았다."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(Story("오늘 새로운 도전자인 리안이 칼을 잡아본다고 한다."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(Story("언제나 그렇듯 주민들은 기대를 하고 있다."));
        yield return new WaitForSeconds(8f);
        yield return StartCoroutine(Story("리안이 칼을 잡자 엄청난 빛과 함께 돌에서 빛기둥이 하늘로 솟아올랐다."));
        yield return new WaitForSeconds(9f);
        yield return StartCoroutine(Story("그리고 이 빛기둥때문에 몬스터를 막아줄 보호막이 생겨 마을을 덮었다."));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Story("리안은 깜짝 놀랬지만 그것도 잠시 마을에 영원한 평화를 가져오기 다짐 했다."));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Story("리안과 함께 싸워서 영원한 평화를 가져다 주세요!"));
        yield return new WaitForSeconds(1f);
        fadeinout.gameObject.SetActive(true);
        _Faid.NextScene = "Main";
        FaidInOut.InStartFadeAnim();
    }
}
