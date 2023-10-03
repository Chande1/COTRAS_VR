using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindArriveManager : MonoBehaviour
{
    [Header("플레이어(기입용)")]
    [SerializeField] GameObject Player;         //플레이어
    [SerializeField] GameObject vrcamera;       //카메라
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    [Header("목적지 배열(기입용)")]
    [SerializeField] GameObject ArrivePos;      //목적지 배열 부모 오브젝트
    [SerializeField] GameObject[] APoints;      //목적지 배열
    [SerializeField] GameObject StartPos;       //현재 시작 지점
    [SerializeField] GameObject EndPos;         //현재 끝 지점
    [SerializeField] GameObject TempStartPos;        //임시 보관용 좌표
    [SerializeField] GameObject[] TempEndPos;
    [Header("목적지 오브젝트(기입용)")]
    [SerializeField] GameObject ArriveObject;
    [SerializeField] GameObject arriveobject;
    [Header("문제 카운트(기입용)")]
    [SerializeField] int roundcount;        //문제수
    [Header("목적지 카운트(기입용)")]
    [SerializeField] int apointcount;       //목적지 카운트
    [Header("문제/목적지 카운트(확인용)")]
    [SerializeField] int nowrcount;
    [SerializeField] int nowacount;
    [Header("UI(기입용)")]
    [SerializeField] GameObject UIBox;
    [SerializeField] GameObject arrow_ui;
    [SerializeField] GameObject count_ui;
    [SerializeField] GameObject ready_ui;
    [SerializeField] GameObject noarrow_ui;
    [SerializeField] GameObject count2_ui;
    [SerializeField] GameObject good_ui;
    [SerializeField] GameObject well_ui;
    [SerializeField] GameObject bad_ui;

    [Header("화살표(기입용)")]
    [SerializeField] WayPointArrow arrowscript;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject distance;
    [Header("3D화살표(기입용)")]
    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject Distance;
    [Header("3D화살표 사용하기")]
    [SerializeField] bool ObjectArrow;

    [Header("플레이어 목적지 접촉 상태(확인용")]
    [SerializeField] bool imarrive;
    [Header("목적지까지 거리(확인용)")]
    [SerializeField] float arrivedistance;

    [Header("거리측정(기입용)")]
    [SerializeField] int gooddis;
    [SerializeField] int welldis;
    [SerializeField] int baddis;

    [Header("화살표 표시(확인용)")]
    [SerializeField] bool arrownow;

    [Header("시간제한(기입용)")]
    [SerializeField] Text time;     //시간표시
    [SerializeField] Text time2;     //시간표시
    [SerializeField] Text time3;     //시간표시
    [SerializeField] int limit;     //제한시간
    float limit_f;
    bool fail;
    bool ing;


    private void Awake()
    {
        StartSetting();
        ArrowArrive();
    }

    private void Update()
    {
        arrivedistance = Vector3.Distance(EndPos.transform.position, vrcamera.transform.position);
        
        if(ing)
        {
            ClockWork();
        }
        else if(fail)
        {
            if (arrivedistance > baddis)
            {
                CloseUI();
                UIBox.SetActive(true);
                bad_ui.SetActive(true);
                imarrive = true;
            }
            else if (arrivedistance < baddis&&arrivedistance > welldis)
            {
                CloseUI();
                UIBox.SetActive(true);
                well_ui.SetActive(true);
                imarrive = true;
            }
            else if(arrivedistance < welldis)
            {
                CloseUI();
                UIBox.SetActive(true);
                good_ui.SetActive(true);
                imarrive = true;
            }
        }


        //도착지점에 도착했을때
        if(!fail&&!imarrive&&arrivedistance<=1.5f)
        {
            CloseUI();
            UIBox.SetActive(true);
            ready_ui.SetActive(true);
            ing = false;
            imarrive = true;
            Debug.Log("시간 안에 이동 완료!");
        }
        if(imarrive)
        {
            if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)|| LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                //기존 목적지 삭제
                Destroy(arriveobject);
                CloseUI();

                if (arrownow)
                {
                    nowacount += 1; //다음 목적지
                    Debug.Log(nowacount + "번째 화살표 목적지");
                    //화살표 있는 목적지 이동
                    ArrowArrive();
                    
                }
                else
                {
                    nowacount += 1;  //다음 목적지
                    Debug.Log(nowacount + "번째 비화살표 목적지");
                    //화살표 없이 목적지 이동
                    NoArrowResetPoint();
                    
                }

                imarrive = false;
            }
            
        }
    }


    //시작 세팅
    void StartSetting()
    {
        imarrive = false;
        arrownow = true;
        fail = false;
        ing = false;

        if(GameObject.Find("stageinfo"))
        {
            apointcount = StageInfo.stageinfo.findAandOArriveCount;
            roundcount = StageInfo.stageinfo.findAandORoundCount;
        }


        nowrcount = 1;
        nowacount = 1;
        count_ui.GetComponent<Text>().text = nowacount.ToString();
        count2_ui.GetComponent<Text>().text = nowacount.ToString();


        SettingArrivePoint();

        //ui 끄기
        arrow_ui.SetActive(false);
        count_ui.SetActive(false);
        ready_ui.SetActive(false);
        noarrow_ui.SetActive(false);
        count2_ui.SetActive(false);
        good_ui.SetActive(false);
        well_ui.SetActive(false);
        bad_ui.SetActive(false);
        UIBox.SetActive(false);

        if (ObjectArrow)
        {
            Arrow.SetActive(true);
            Distance.SetActive(true);
            arrow.SetActive(false);
            distance.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            distance.SetActive(true);
            Arrow.SetActive(false);
            Distance.SetActive(false);
        }
    }

    //시계 작동
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Mathf.Round(limit_f).ToString()+"초";
        time2.text = Mathf.Round(limit_f).ToString() + "초";
        time3.text = Mathf.Round(limit_f).ToString() + "초";
        float temp = 1000 / limit;

        //시간초가 0이 되면
        if (Mathf.Round(limit_f) == 0)
        {
            fail = true;
            ing = false;
        }
    }


    //화살표가 있는 목적지
    public void ArrowArrive()
    {
        //처음 목적지 생성
        if (nowacount==1)
        {
            RandomArrivePos();          //랜덤 목적지 설정
            TempEndPos = new GameObject[apointcount+1];   //목적지 갯수만큼 목적지 보관함 생성(출발지는 포함)
            TempStartPos = StartPos;    //첫 목적지 보관
            TempEndPos[0] = StartPos;
            TempEndPos[1] = EndPos;

            //화살표 표기
            arrownow = true;
        }
        //제자리에서 다음 목적지 설정
        else if(apointcount>=nowacount)
        {
            OtherArrivePos();                           //제자리에서 다음 목적지 설정
            TempEndPos[nowacount] = EndPos;         //새로운 목적지 목적지 보관함에 보관
            Debug.Log(nowacount+"번째 목적지 좌표 저장");
            //화살표 표기
            arrownow = true;
        }
        else
        {
            Debug.Log("모든 화살표 목적지 이동 완료!");
            nowacount = 1;  //초기화
            //화살표 표기
            arrownow = false;
            Debug.Log(nowacount + "번째 비화살표 목적지");
            //화살표 없이 목적지 이동
            NoArrowResetPoint();
        }
        
        if(arrownow)
        {
            SettingArriveObject();  //플레이어와 목적지 위치로 설정

            //시계작동
            limit_f = limit;
            fail = false;
            ing = true;

            //목적지 순서 업데이트
            count_ui.GetComponent<Text>().text = nowacount.ToString();
            count2_ui.GetComponent<Text>().text = nowacount.ToString();
            //ui활성
            UIBox.SetActive(true);
            arrow_ui.SetActive(true);
            count_ui.SetActive(true);
            //ui비활성
            Invoke("CloseUI", 3f);


            if (ObjectArrow)
            {
                Arrow.SetActive(true);
                Distance.SetActive(true);
            }
            else
            {
                arrow.SetActive(true);
                distance.SetActive(true);
            }
        }
    }

    //화살표 없이 다시한번 목적지로 이동
    public void NoArrowResetPoint()
    {
        if(apointcount <nowacount)
        {
            if(roundcount>nowrcount)
            {
                Debug.Log("모든 비화살표 목적지 이동 완료!");
                Debug.Log(nowrcount+"번째 문제 완료!");
                nowacount = 1;  //초기화
                nowrcount += 1; //문제수 추가
                                //화살표 표기
                arrownow = true;
                CloseUI();
                ArrowArrive();
            }
            else
            {
                CloseUI();
                Debug.Log("모든 문제 완료!");
                arrownow = true;
                imarrive = false;
                fail = false;
                ing = false;
                SceneManager.LoadScene("FindArriveAndObject_Start");
            }
        }

        if (!arrownow)
        {
            SettingArriveObject();  //플레이어와 목적지 처음 위치로 설정
            EndPos = TempEndPos[nowacount];

            //시계작동
            limit_f = limit;
            fail = false;
            ing = true;

            //목적지 순서 업데이트
            count_ui.GetComponent<Text>().text = nowacount.ToString();
            count2_ui.GetComponent<Text>().text = nowacount.ToString();
            //ui활성
            UIBox.SetActive(true);
            noarrow_ui.SetActive(true);
            count2_ui.SetActive(true);
            //ui비활성
            Invoke("CloseUI", 2f);

            //화살표 비활성
            arrownow = false;
            if (ObjectArrow)
            {
                Arrow.SetActive(false);
                Distance.SetActive(false);
            }
            else
            {
                arrow.SetActive(false);
                distance.SetActive(false);
            }
        }
       
    }

    [ContextMenu("도착지점 오브젝트 세팅")]
    private void SettingArrivePoint()
    {
        //자식이 있을때
        if(ArrivePos.transform.childCount!=0)
        {
            APoints = new GameObject[ArrivePos.transform.childCount];

            for(int i=0;i<ArrivePos.transform.childCount;i++)
            {
                APoints[i] = ArrivePos.transform.GetChild(i).gameObject;
            }
        }
    }

    [ContextMenu("랜덤한 출발지와 목적지 산출")]
    private void RandomArrivePos()
    {
        int start = Random.Range(0, APoints.Length);  //랜덤한 시작 좌표 숫자 선정
        int end = Random.Range(0, APoints.Length);    //랜덤한 시작 좌표 숫자 선정

        //만약 숫자가 겹칠때
        while(start==end)
        {
            end = Random.Range(0, APoints.Length);
        }

        StartPos = APoints[start];
        EndPos = APoints[end];
    }

    //제자리에서 새로운 목적지만 찾기
    private void OtherArrivePos()
    {
        int end = Random.Range(0, APoints.Length);    //랜덤한 시작 좌표 숫자 선정
        EndPos = APoints[end];

        //만약 숫자가 겹칠때
        while (TempEndPos[nowacount-1]==EndPos)
        {
            end = Random.Range(0, APoints.Length);    //랜덤한 시작 좌표 숫자 선정
            EndPos = APoints[end];
        }

    }


    //플레이어 위치와 목적지 프리팹 설정
    void SettingArriveObject()
    {
        //플레이어
        Player.transform.position = TempEndPos[nowacount-1].transform.position;  //이전 위치로 플레이어 이동
                                                                  //Player.transform.eulerAngles = StartPos.transform.eulerAngles;

        //목적지 오브젝트
        arriveobject = GameObject.Instantiate(ArriveObject, TempEndPos[nowacount].transform.position, ArriveObject.transform.rotation);
        arriveobject.GetComponent<ParticleSystem>().Play(); //파티클 재생

    }

    void CloseUI()
    {
        UIBox.SetActive(false);
        if (arrow_ui.activeSelf)
            arrow_ui.SetActive(false);
        if (count_ui.activeSelf)
            count_ui.SetActive(false);
        if (ready_ui.activeSelf)
            ready_ui.SetActive(false);
        if (noarrow_ui.activeSelf)
            noarrow_ui.SetActive(false);
        if (count2_ui.activeSelf)
            count2_ui.SetActive(false);
        if (good_ui.activeSelf)
            good_ui.SetActive(false);
        if (well_ui.activeSelf)
            well_ui.SetActive(false);
        if (bad_ui.activeSelf)
            bad_ui.SetActive(false);
        
    }


    public Transform GetEndPos()
    {
        return EndPos.transform;
    }
}
