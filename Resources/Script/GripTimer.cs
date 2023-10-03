using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GripTimer : MonoBehaviour
{
    [Header("설정된 오브젝트")]
    [SerializeField]GameObject target;                  //타이머 설정 오브젝트
    [SerializeField] GripStateValue targetgripstate;    //설정 오브젝트의 상태
    [Header("타이머 정보")]
    [SerializeField] Image clock;                       //게이지
    [SerializeField] Text time;                         //시간초 표시 텍스트
    [Header("타이머 설정")]
    [SerializeField] GameObject timerinfo;
    [SerializeField] int limit;                         //설정된 시간
    [SerializeField] float timer;                       //실제로 흐르는 시간
    [SerializeField] bool stop;                         //트리거로 인한 시간 정지가 있는 경우
    [SerializeField] bool endDestroy;                   //끝나고 삭제

    [Header("완료 효과")]
    [SerializeField] bool end;
    [SerializeField] bool destroy;
    [SerializeField] GameObject effect;                 //완료 이펙트

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
        gameObject.transform.LookAt(vrcamera.transform);  //카메라 바라보기

        targetgripstate = target.GetComponent<GripState>().GetGripStateValue(); //물체를 부착 상태 실시간 확인

        if(!end) //완료 전
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
        else if (!destroy && end) //완료 후
        {
            //정답 이펙트 생성
            GameObject c_effect = GameObject.Instantiate(effect, transform.position, transform.rotation);
            c_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
            Destroy(c_effect, 1f);                          //2초 후에 삭제

            if(endDestroy)
                Destroy(gameObject, 1f);
            destroy = true;

        }
    }

    //시계 작동
    void ClockWork()
    {
        timer -= Time.deltaTime;
        time.text = string.Format("{0:0.##}", timer);
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //시간초가 0이 되면(소수점 2번째 자리)
        if (Math.Truncate(timer*100)/100 == 0)
        {
            end = true;
        }
    }

    //타이머 초기화
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
