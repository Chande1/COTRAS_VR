using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutLine : MonoBehaviour
{
    bool cutting;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("knife"))
        {
            cutting = true;
        }
    }

    public bool GetCutting()
    {
        if (cutting)
            return true;
        else
            return false;
    }
}
