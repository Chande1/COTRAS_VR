using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagObject : MonoBehaviour
{
    [Header("�浹�Ǵ� ������Ʈ �±� �̸�")]
    [SerializeField] string tagname;

    [Header("�浹�Ǵ� ������Ʈ ���� ����")]
    [SerializeField] bool destroy;

    [Header("�ִϸ��̼�")]
    [SerializeField] bool haveAni;
    [SerializeField] Animator animator;
    [SerializeField] string stateboolname;

    [Header("����Ƽ �̺�Ʈ")]
    [SerializeField] bool haveEvent;
    [SerializeField] UnityEvent OnTouch = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(tagname))
        {
            if(haveAni)
            {
                animator.SetBool(stateboolname, true);
                Debug.Log(gameObject.name + "�ִϸ��̼� �۵�");
            }

            if(haveEvent)
            {
                OnTouch.Invoke();   //�̺�Ʈ ����
                Debug.Log(gameObject.name + "�̺�Ʈ �۵�");
            }

            if(destroy)
            {
                Destroy(other.gameObject,0.5f);
                Debug.Log(gameObject.name + "����");
            }
        }
    }
}
