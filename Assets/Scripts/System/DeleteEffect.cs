/**
 * 
 *  스크립트 이름 : DeleteEffect.cs
 *  스크립트 용도 : 시간이 끝난 이팩트 삭제
 *  
 **/
using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ParticleSystem>().isPlaying)
            Destroy(gameObject);
    }
}
