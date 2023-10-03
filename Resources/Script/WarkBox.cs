using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WarkBox : MonoBehaviour
{
    [SerializeField]
    bool righthand;
    [SerializeField]
    GameObject otherHand;

    bool wboxwork;
    bool RHandIn;
    bool LHandIn;

    private void Start()
    {
        wboxwork = false;
        RHandIn = false;
        LHandIn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (righthand)
        {
            if (other.CompareTag("RHand"))
            {
                RHandIn = true;

                if(otherHand.GetComponent<WarkBox>().LHandWark())
                {
                    otherHand.GetComponent<WarkBox>().ResetWBox();
                    wboxwork = true;
                }
            }
        }
        else
        {
            if(other.CompareTag("LHand"))
            {
                LHandIn = true;

                if(otherHand.GetComponent<WarkBox>().RHandWark())
                {
                    otherHand.GetComponent<WarkBox>().ResetWBox();
                    wboxwork = true;
                }
                   
            }
        }
    }

    public bool WarkNow()
    {
        if (wboxwork)
            return true;
        else
            return false;
    }

    public bool RHandWark()
    {
        if (RHandIn)
            return true;
        else
            return false;
    }

    public bool LHandWark()
    {
        if (LHandIn)
            return true;
        else
            return false;
    }

    public void ResetWBox()
    {
        wboxwork = false;
        RHandIn = false;
        LHandIn = false;

    }
}
