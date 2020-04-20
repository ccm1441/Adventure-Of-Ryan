/**
 * 
 *  스크립트 이름 :  DestorySkil.cs
 *  스크립트 용도 : 이펙트 시전 시간이 끝난 이팩트 제거
 *  
 **/
using UnityEngine;

public class DestorySkill : MonoBehaviour
{
    ParticleSystem This;
    public GameObject Parent;
    void Start()
    {
        This = GetComponent<ParticleSystem>();  
    }

    void Update()
    {     
        if (!This.isPlaying)           
            Destroy(Parent);
    }
}
