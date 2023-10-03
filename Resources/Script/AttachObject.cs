using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObject : MonoBehaviour
{
    [Header("지정 오브젝트(대상이 1개일때)")]
    [SerializeField] GameObject attachobject;
    [Header("지정 태그(대상이 여러개일때)")]
    [SerializeField] string objecttag;
    [SerializeField] bool attach;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("부착 시도 오브젝트:" + other.name);
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
                Debug.Log("부착중인 오브젝트:" + other.name);
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
