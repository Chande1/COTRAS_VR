using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(GripState))]
public class GripObject : MonoBehaviour
{
    [Header("대상 오브젝트 정보")]
    [SerializeField]GripState gripstate;
    [SerializeField] GameObject griptimer;
    [Header("오브젝트 시작 위치")]
    [SerializeField] Transform startpos;
    [Header("목표도달 여부")]
    [SerializeField] bool goal;

    [Header("완료 효과")]
    [SerializeField] GameObject effect;                 //완료 이펙트

    [Header("손")]
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
        //시간이 완료되면 원래 자리로 이동
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
        //책상 밖으로 벗어나면 원래 자리로 이동
        if (other.CompareTag("outside"))
            ObjectStartPosSet();
        //목표에 도달하면 목표값 true
        else if (other.CompareTag("goal"))
        {
            GameObject c_effect = GameObject.Instantiate(effect, transform.position, transform.rotation);
            c_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
            Destroy(c_effect, 1f);                          //1초 후에 삭제
            GameObject.Find("BasicControlManager").GetComponent<BasicControlManager>().SuccessCountUp();
            ObjectStartPosSet();
            Destroy(gameObject);
            goal = true;
        }
            

    }

    //처음 위치로
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
        GripTimerOff();     //타이머 비활성화
        GripObjectStay();   //움직임 고정
    }
}
