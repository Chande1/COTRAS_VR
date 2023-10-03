using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagObject : MonoBehaviour
{
    [Header("충돌되는 오브젝트 태그 이름")]
    [SerializeField] string tagname;

    [Header("충돌되는 오브젝트 삭제 여부")]
    [SerializeField] bool destroy;

    [Header("애니메이션")]
    [SerializeField] bool haveAni;
    [SerializeField] Animator animator;
    [SerializeField] string stateboolname;

    [Header("유니티 이벤트")]
    [SerializeField] bool haveEvent;
    [SerializeField] UnityEvent OnTouch = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(tagname))
        {
            if(haveAni)
            {
                animator.SetBool(stateboolname, true);
                Debug.Log(gameObject.name + "애니메이션 작동");
            }

            if(haveEvent)
            {
                OnTouch.Invoke();   //이벤트 실행
                Debug.Log(gameObject.name + "이벤트 작동");
            }

            if(destroy)
            {
                Destroy(other.gameObject,0.5f);
                Debug.Log(gameObject.name + "제거");
            }
        }
    }
}
