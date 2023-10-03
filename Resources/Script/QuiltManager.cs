using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiltManager : MonoBehaviour
{
    [SerializeField] GameObject R_1;
    [SerializeField] GameObject R_2;
    [SerializeField] GameObject L_1;
    [SerializeField] GameObject L_2;
    [SerializeField] GameObject Quilt1;
    [SerializeField] GameObject Quilt2;
    [SerializeField] GameObject Quilt3;
    [SerializeField] GameObject Effect;
    [SerializeField] int quiltmovecount;    //손의 움직임 횟수

    private void Awake()
    {
        quiltmovecount = 0;
        Quilt1.SetActive(false);
        Quilt2.SetActive(false);
        Quilt3.SetActive(false);
        Effect.SetActive(false);
    }

    public bool QuiltClean()
    {
        if (quiltmovecount == 10)
            return true;
        else
            return false;
    }

    public void AllQuiltMoveNow()
    {
        if (R_1.GetComponent<QuiltMove>().QuiltMoveNow() &&
            R_2.GetComponent<QuiltMove>().QuiltMoveNow() &&
            L_1.GetComponent<QuiltMove>().QuiltMoveNow() &&
            L_2.GetComponent<QuiltMove>().QuiltMoveNow())
        {
            Debug.Log("All Quilt Move Count:"+quiltmovecount);
            ShowQuilt(quiltmovecount++);
            AllQuiltReset();
        }   
        else
        {
            //Debug.Log("All Quilt is not move Now.");
        } 
    }

    public void AllQuiltReset()
    {
        R_1.GetComponent<QuiltMove>().ResetQuilt();
        R_2.GetComponent<QuiltMove>().ResetQuilt();
        L_1.GetComponent<QuiltMove>().ResetQuilt();
        L_2.GetComponent<QuiltMove>().ResetQuilt();
    }

    public void ShowQuilt(int _quiltmovecount)
    {
        switch(_quiltmovecount)
        {
            case 3:
                Effect.SetActive(true);
                Effect.GetComponent<ParticleSystem>().Play();
                Invoke("EffectOff", 1f);
                Quilt1.SetActive(true);
                break;
            case 6:
                Effect.SetActive(true);
                Effect.GetComponent<ParticleSystem>().Play();
                Invoke("EffectOff", 1f);
                Quilt1.SetActive(false);
                Quilt2.SetActive(true);
                break;
            case 9:
                Effect.SetActive(true);
                Effect.GetComponent<ParticleSystem>().Play();
                Invoke("EffectOff", 1f);
                Quilt2.SetActive(false);
                Quilt3.SetActive(true);
                break;
        }
    }

    void EffectOff()
    {
        Debug.Log("effectOff");
        Effect.SetActive(false);
    }
}
