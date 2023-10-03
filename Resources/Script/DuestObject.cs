using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuestObject : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject Duest_A;
    [SerializeField] GameObject Duest_B;
    [SerializeField] GameObject Duest_C;
    [SerializeField] GameObject timer;
    [Header("ȿ��")]
    [SerializeField] GameObject Effect;
    int duestcount;

    private void Awake()
    {
        duestcount = 1;
        Duest_A.SetActive(false);
        Duest_B.SetActive(false);
        Duest_C.SetActive(false);
        Effect.SetActive(false);
    }

    public bool DuestClean()
    {
        if (duestcount >= 4)
        {
            Debug.Log("All Duest Clean!");
            return true;
        }   
        else
            return false;
    }


    public void DuestStart()
    {
        Duest_A.SetActive(true);
        Duest_A.GetComponent<ParticleSystem>().Play();
    }

    public void DuestCleanNow()
    {
        //Ÿ�̸Ӱ� �����ٸ�
        if(timer.GetComponent<GripTimer>().GetTimerDone())
        {
            switch(duestcount)
            {
                case 1:
                    Duest_A.SetActive(false);
                    Duest_B.SetActive(true);
                    Duest_B.GetComponent<ParticleSystem>().Play();
                    duestcount++;
                    break;
                case 2:
                    Duest_B.SetActive(false);
                    Duest_C.SetActive(true);
                    Duest_C.GetComponent<ParticleSystem>().Play();
                    duestcount++;
                    break;
                case 3:
                    Duest_C.SetActive(false);
                    duestcount++;
                    break;
                default:
                    break;
            }
            Effect.SetActive(true);
            Effect.GetComponent<ParticleSystem>().Play();
            Invoke("StopEffect", 2f);
            //�ð� �ʱ�ȭ
            timer.GetComponent<GripTimer>().ClockReset();
        }
    }

    void StopEffect()
    {
        Effect.SetActive(false);
    }
}
