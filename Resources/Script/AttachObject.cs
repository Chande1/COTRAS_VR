using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObject : MonoBehaviour
{
    [Header("���� ������Ʈ(����� 1���϶�)")]
    [SerializeField] GameObject attachobject;
    [Header("���� �±�(����� �������϶�)")]
    [SerializeField] string objecttag;
    [SerializeField] bool attach;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("���� �õ� ������Ʈ:" + other.name);
        if (objecttag!=null&&!attach)
        {
            if(other.tag==objecttag)
            {
                attachobject = other.gameObject;
                attach = true;
            }
        }
        else
        {
            if (other.name == attachobject.name)
            {
                Debug.Log("�������� ������Ʈ:" + other.name);
                attach = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (attach)
        {
            attachobject.transform.position = gameObject.transform.position;
            attachobject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DetachObject()
    {
        if (attachobject != null)
        {
            attach = false;
            attachobject.tag = "Untagged";
            attachobject.GetComponent<Rigidbody>().isKinematic = false;
            attachobject = null;
        }
    }

    public void DetachObjects(GameObject _pos)
    {
        if(attachobject!=null)
        {
            attach = false;
            attachobject.tag = "Untagged";
            attachobject.GetComponent<Rigidbody>().isKinematic = false;
            attachobject.transform.position = _pos.transform.position;
            attachobject = null;
        }
        
    }
}
