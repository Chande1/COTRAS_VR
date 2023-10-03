using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixBowl : MonoBehaviour
{
    [SerializeField] float spinspeed;
    [SerializeField] float mixtime;
    [SerializeField] float mixingtime;
    [SerializeField] bool mixdone;
    [SerializeField] bool brokendone;
    [SerializeField] int objectcount;
    [SerializeField] GameObject[] tempcountobject;
    bool same;

    [SerializeField] GameObject egg1;
    [SerializeField] GameObject egg2;
    [SerializeField] GameObject egg3;

    [SerializeField] GameObject eggmix1;
    [SerializeField] GameObject eggmix2;
    [SerializeField] GameObject eggmix3;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Egg"))
        {
            Debug.Log(other.gameObject.name + " ¡¢√À");

            for (int i = 0; i <= objectcount; i++)
            {
                if (tempcountobject[i] != null && tempcountobject[i].name == other.gameObject.name)
                {
                    same = true;
                }
            }

            if (!same)
            {
                tempcountobject[objectcount] = other.gameObject;
                objectcount += 1;
                if (!egg1.activeInHierarchy)
                {
                    egg1.SetActive(true);
                    eggmix1.SetActive(true);
                }
                else if (egg1.activeInHierarchy && !egg2.activeInHierarchy)
                    egg2.SetActive(true);
                else if (egg1.activeInHierarchy && egg2.activeInHierarchy && !egg3.activeInHierarchy)
                {
                    egg3.SetActive(true);
                    brokendone = true;
                }


            }
            else
            {
                same = false;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item"&&!mixdone)
        {
            transform.Rotate(new Vector3(0,spinspeed*Time.deltaTime,0),Space.Self);


            mixingtime -= Time.deltaTime;

            if (mixingtime <= mixtime/2)
            {
                if(!eggmix2.activeInHierarchy)
                {
                    egg1.SetActive(false);
                    egg2.SetActive(false);
                    egg3.SetActive(false);
                    eggmix1.SetActive(false);
                    eggmix2.SetActive(true);
                }
            }
            if(mixingtime <= 0)
            {
                if (!eggmix3.activeInHierarchy)
                {
                    eggmix2.SetActive(false);
                    eggmix3.SetActive(true);
                    mixdone = true;
                }
            }

        }
    }


    private void Awake()
    {
        egg1.SetActive(false);
        egg2.SetActive(false);
        egg3.SetActive(false);
        eggmix1.SetActive(false);
        eggmix2.SetActive(false);
        eggmix3.SetActive(false);

        mixingtime = mixtime;
        mixdone = false;
        brokendone = false;
    }

    public bool GetBrokenDone()
    {
        if (brokendone)
            return true;
        else
            return false;
    }


    public bool GetMixDone()
    {
        if (mixdone)
            return true;
        else
            return false;
    }
}
