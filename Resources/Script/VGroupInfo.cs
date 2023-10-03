using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VGroupInfo : MonoBehaviour
{
    [Header("자식 바이러스")]
    [SerializeField] GameObject[] vg_child;  //자식 바이러스

    void Start()
    {
       for(int i=0;i<transform.childCount;i++)
        {
            vg_child[i] = transform.GetChild(i).gameObject;
        }
    }

    public void DestroyVGroup()
    {
        for(int i=0;i<vg_child.Length;i++)
        {
            if(vg_child[i]!=null)
            {
                vg_child[i].GetComponent<VirusInfo>().DestoyVirus();
            }

            if(i==vg_child.Length-1)
            {
                Destroy(gameObject,1f);
            }
                
        }

    }
}
