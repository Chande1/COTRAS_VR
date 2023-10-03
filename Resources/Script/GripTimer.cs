using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GripTimer : MonoBehaviour
{
    [Header("������ ������Ʈ")]
    [SerializeField]GameObject target;                  //Ÿ�̸� ���� ������Ʈ
    [SerializeField] GripStateValue targetgripstate;    //���� ������Ʈ�� ����
    [Header("Ÿ�̸� ����")]
    [SerializeField] Image clock;                       //������
    [SerializeField] Text time;                         //�ð��� ǥ�� �ؽ�Ʈ
    [Header("Ÿ�̸� ����")]
    [SerializeField] GameObject timerinfo;
    [SerializeField] int limit;                         //������ �ð�
    [SerializeField] float timer;                       //������ �帣�� �ð�
    [SerializeField] bool stop;                         //Ʈ���ŷ� ���� �ð� ������ �ִ� ���
    [SerializeField] bool endDestroy;                   //������ ����

    [Header("�Ϸ� ȿ��")]
    [SerializeField] bool end;
    [SerializeField] bool destroy;
    [SerializeField] GameObject effect;                 //�Ϸ� ����Ʈ

    GameObject vrcamera;

    private void Awake()
    {
        end = false;
        destroy = false;
        vrcamera = GameObject.Find("VRCamera");
        target = transform.parent.gameObject;
        targetgripstate = target.GetComponent<GripState>().GetGripStateValue();
        timerinfo.SetActive(false);
    }

    // Update is called once per framew
    void Update()
    {
        gameObject.transform.LookAt(vrcamera.transform);  //ī�޶� �ٶ󺸱�

        targetgripstate = target.GetComponent<GripState>().GetGripStateValue(); //��ü�� ���� ���� �ǽð� Ȯ��

        if(!end) //�Ϸ� ��
        {
            switch (targetgripstate)
            {
                case GripStateValue.Gripping:
                    timerinfo.SetActive(true);
                    if(!stop)
                        ClockWork();
                    break;
                case GripStateValue.GripStop:
                    ClockReset();
                    timerinfo.SetActive(false);
                    break;
                default:
                    ClockReset();
                    timerinfo.SetActive(false);
                    break;
            }
        }
        else if (!destroy && end) //�Ϸ� ��
        {
            //���� ����Ʈ ����
            GameObject c_effect = GameObject.Instantiate(effect, transform.position, transform.rotation);
            c_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
            Destroy(c_effect, 1f);                          //2�� �Ŀ� ����

            if(endDestroy)
                Destroy(gameObject, 1f);
            destroy = true;

        }
    }

    //�ð� �۵�
    void ClockWork()
    {
        timer -= Time.deltaTime;
        time.text = string.Format("{0:0.##}", timer);
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //�ð��ʰ� 0�� �Ǹ�(�Ҽ��� 2��° �ڸ�)
        if (Math.Truncate(timer*100)/100 == 0)
        {
            end = true;
        }
    }

    //Ÿ�̸� �ʱ�ȭ
    public void ClockReset()
    {
        timer = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;
        end = false;
        destroy = false;
    }

    public bool GetTimeOut()
    {
        if (end)
            return true;
        else
            return false;
    }

    public bool GetTimerDone()
    {
        if(destroy)
        {
            Debug.Log("timer is done!");
            return true;
        }
        else
        {
            return false;
        }
           
    }

    public void StopTimer()
    {
        stop = true;
    }

    public void StartTimer()
    {
        stop = false;
    }
}
