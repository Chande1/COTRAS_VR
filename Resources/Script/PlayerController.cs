using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject HeadDirect;
    [SerializeField]
    GameObject WarkBoxR;
    [SerializeField]
    GameObject WarkBoxL;
    [SerializeField]
    float warkdistance;
    [SerializeField]
    UnityEngine.AI.NavMeshAgent agent;

    bool changerot;
    bool imwarking;

    private void Start()
    {
        changerot = true;
        imwarking = false;
    }

    void Update()
    {
        //양손 번갈아
        if(WarkBoxL.GetComponent<WarkBox>().WarkNow()|| WarkBoxR.GetComponent<WarkBox>().WarkNow())
        {
            if(changerot)
            {
                imwarking = true;
                if (agent == null)
                    ChangeRotation();
                Invoke("StandNow", 0.5f);
                changerot = false;
            }

            if(agent==null)
            {
                transform.Translate(new Vector3(0, 0, warkdistance) * Time.deltaTime);
            } 
            else
            {
                Vector3 movedir = HeadDirect.transform.rotation * Vector3.forward; //카메라가 보는 앞
                agent.Move(movedir * Time.deltaTime * warkdistance);
            }
            
        }
            
    }

    private void ChangeRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, HeadDirect.transform.eulerAngles.y, transform.eulerAngles.z);
        //HeadDirect.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void StandNow()
    {
        if (WarkBoxL.GetComponent<WarkBox>().WarkNow())
        {
            WarkBoxL.GetComponent<WarkBox>().ResetWBox();
        }
        else if (WarkBoxR.GetComponent<WarkBox>().WarkNow())
        {
            WarkBoxR.GetComponent<WarkBox>().ResetWBox();
        }

        changerot = true;
    }

    public bool GetPlayerWalk()
    {
        return imwarking;
    }

    public void ResetWBox()
    {
        WarkBoxL.GetComponent<WarkBox>().ResetWBox();
        WarkBoxR.GetComponent<WarkBox>().ResetWBox();
    }
}
