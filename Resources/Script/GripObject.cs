using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(GripState))]
public class GripObject : MonoBehaviour
{
    [Header("��� ������Ʈ ����")]
    [SerializeField]GripState gripstate;
    [SerializeField] GameObject griptimer;
    [Header("������Ʈ ���� ��ġ")]
    [SerializeField] Transform startpos;
    [Header("��ǥ���� ����")]
    [SerializeField] bool goal;

    [Header("�Ϸ� ȿ��")]
    [SerializeField] GameObject effect;                 //�Ϸ� ����Ʈ

    [Header("��")]
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    bool time;

    void Awake()
    {
        time = false;
        goal = false;
        gripstate = GetComponent<GripState>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "TimeWindow")
                griptimer = transform.GetChild(i).gameObject;
        }

        RHand = GameObject.Find("RightHand").GetComponent<Hand>();
        LHand = GameObject.Find("LeftHand").GetComponent<Hand>();

        ObjectStartPosSet();
    }

    private void Update()
    {
        //�ð��� �Ϸ�Ǹ� ���� �ڸ��� �̵�
        if(!time&&griptimer&&griptimer.GetComponent<GripTimer>().GetTimeOut())
        {
            Debug.Log(gameObject.name + ": success count!");
            GameObject.Find("BasicControlManager").GetComponent<BasicControlManager>().SuccessCountUp();
            time = true;
            goal = true;
        }
        else if(goal&&!griptimer)
        {
            ObjectStartPosSet();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //å�� ������ ����� ���� �ڸ��� �̵�
        if (other.CompareTag("outside"))
            ObjectStartPosSet();
        //��ǥ�� �����ϸ� ��ǥ�� true
        else if (other.CompareTag("goal"))
        {
            GameObject c_effect = GameObject.Instantiate(effect, transform.position, transform.rotation);
            c_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
            Destroy(c_effect, 1f);                          //1�� �Ŀ� ����
            GameObject.Find("BasicControlManager").GetComponent<BasicControlManager>().SuccessCountUp();
            ObjectStartPosSet();
            Destroy(gameObject);
            goal = true;
        }
            

    }

    //ó�� ��ġ��
    void ObjectStartPosSet()
    {
        if (gameObject.transform.parent)
        {
            RHand.DetachObject(gameObject);
            LHand.DetachObject(gameObject);
            gameObject.transform.parent = null;
        }
            
        GripObjectStay();
        gameObject.transform.position = startpos.position;
        GripObjectMove();
        goal = false;
    }

    public bool GetGripObjectGoal()
    {
        if (goal)
            return true;
        else
            return false;
    }

    public void GripTimerOn()
    {
        griptimer.SetActive(true);
    }

    public void GripTimerOff()
    {
        griptimer.SetActive(false);
    }

    public bool TimerOut()
    {
        if (griptimer.GetComponent<GripTimer>().GetTimeOut())
            return true;
        else
            return false;
    }

    public void GripObjectStay()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void GripObjectMove()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void GripObjectStartSetting()
    {
        GripTimerOff();     //Ÿ�̸� ��Ȱ��ȭ
        GripObjectStay();   //������ ����
    }
}
