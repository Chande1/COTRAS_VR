using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGravity : MonoBehaviour
{
    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    //������ ��ü�� �������� ��ü ����
    public void ActiveKinematick()
    {
        rig.isKinematic = true;
    }

    //������ ��ü�� ������� ��ü �̵�
    public void InActiveKinematick()
    {
        rig.isKinematic = false;
    }
}
