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

    //손으로 물체를 놓았을때 물체 고정
    public void ActiveKinematick()
    {
        rig.isKinematic = true;
    }

    //손으로 물체를 잡았을때 물체 이동
    public void InActiveKinematick()
    {
        rig.isKinematic = false;
    }
}
