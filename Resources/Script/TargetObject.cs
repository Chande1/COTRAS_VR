using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [SerializeField] ArrowManager manager;
    [SerializeField] GunManager manager2;
    [SerializeField] Animator ani;
    [SerializeField] BoxCollider rigdbox;
    [SerializeField]int enemyscore;
    [SerializeField] bool hit;

    private void OnTriggerEnter(Collider other)
    {
        if(!hit)
        {
            if (other.tag == "projectile")
            {
                if (rigdbox != null)
                    rigdbox.enabled = false;
                ani.SetBool("Down", true);
                if(manager!=null)
                    manager.AddScore(enemyscore);
                else
                    manager2.AddScore(enemyscore);
                Debug.Log("hit!  +" + enemyscore + "score");
                hit = true;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!hit)
        {
            if (other.tag == "projectile")
            {
                ani.SetBool("Down", true);
                if (manager != null)
                    manager.AddScore(enemyscore);
                else
                    manager2.AddScore(enemyscore);
                Debug.Log("hit!  +" + enemyscore + "score");
                hit = true;
            }
        }
    }
}
