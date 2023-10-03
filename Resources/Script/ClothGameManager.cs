using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using System;

public class OCGResult
{
    public string playdate;  //플레이 날짜
    public int playtime;   //소요 시간
    public int correct;    //정답
    public int incorrect;  //오답           
}


public class ClothGameManager : MonoBehaviour
{
    [Header("PC모드")]
    [SerializeField] bool pcmode;
    [SerializeField] PCPlayerController pcplayercam;
    [Header("옷정리 오브젝트")]
    [SerializeField] Hand RHand;
    [SerializeField] GameObject s_win;   //시작창
    [SerializeField] GameObject e_win;   //종료창
    [SerializeField] GameObject w_win;   //주간창
    [SerializeField] GameObject m_win;   //월간창
    [SerializeField] GameObject pointer; //포인터
    [SerializeField] GameObject c_zone;  //옷장
    [SerializeField] GameObject c_point; //옷

    [Header("(기입용)")]
    [SerializeField] Text R_date;
    [SerializeField] Text R_time;
    [SerializeField] Text R_correct;
    [SerializeField] Text R_incorrect;

    [Header("정답/오답")]
    [SerializeField] Text O_count;
    [SerializeField] Text X_count;
    [SerializeField] int ox_count;  //전체 문제 수
    [Header("시간제한")]
    [SerializeField] Image clock;   //시계 이미지
    [SerializeField] Text time;     //시간표시
    [SerializeField] int limit;     //제한시간
    float limit_f;


    OCGResult result=new OCGResult();
    
    
    bool start; //시작버튼
    bool ing;   //진행중
    bool end;   //게임중단

    // Start is called before the first frame update
    void Awake()
    {
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            //pc모드
            if(pcmode)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    pcplayercam.enabled = true;
                    StartGame(); //게임시작!
                    start = false;
                }
            }
            //vr모드
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //오른손 트리거가 눌리는중이면
            {
                StartGame(); //게임시작!
                start = false;
            }
        }
        if(ing)
        {
            CheckGame();    //게임진행 확인
            ClockWork();    //시간제한
        }
       if(end)
        {
            if(pcmode)
            {
                pcplayercam.enabled = false;
            }    

            EndGame();
            MakeResult();
            PrintResult();
            end = false;
        }

        if (w_win.activeSelf)
        {
            //pc모드
            if(pcmode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    w_win.SetActive(false);
                }
            }
            //vr모드
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //오른손 트리거가 눌리는중이면
            {
                Debug.Log("주간창 닫힘");
                w_win.SetActive(false);
            }
        }
        if (m_win.activeSelf)
        {
            //pc모드
            if(pcmode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_win.SetActive(false);
                }
            }
            //vr모드
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //오른손 트리거가 눌리는중이면
            {
                Debug.Log("월간창 닫힘");
                m_win.SetActive(false);
            }
        }

    }

    //시계 작동
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Math.Round(limit_f).ToString();
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime*temp*0.001f);

        //시간초가 0이 되면
        if(Math.Round(limit_f)==0)
        {
            ing = false;    //게임 오버
            end = true;       
        }
    }

    //게임 진행 확인
    void CheckGame()
    {
        int temp = int.Parse(O_count.text) + int.Parse(X_count.text);
        
        //더 이상 의상이 없을때
        if (temp==ox_count)
        {
            ing = false;
            end = true;
        }
    }

    void MakeResult()
    {
        result.playdate = DateTime.Now.ToString("yyyy. MM. dd");   //오늘 날짜 입력
        result.playtime = limit-(int)limit_f;
        result.correct = int.Parse(O_count.text);
        result.incorrect = int.Parse(X_count.text);
    }

    void PrintResult()
    {
        R_date.text = result.playdate;
        R_time.text = result.playtime.ToString();
        R_correct.text = result.correct.ToString();
        R_incorrect.text = result.incorrect.ToString();
        e_win.SetActive(true);
    }

    public void WeekButton()
    {
        Debug.Log("주간창 열림");
        w_win.SetActive(true);
    }

    public void MonthButton()
    {
        Debug.Log("월간창 열림");
        m_win.SetActive(true);
    }


    //시작전 세팅
    void StartSetting()
    {
        start = true;
        ing = false;
        end = false;
        limit_f = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;

        s_win = GameObject.Find("StartWindow");
        e_win = GameObject.Find("EndWindow");
        w_win = GameObject.Find("WeekWindow");
        m_win = GameObject.Find("MonthWindow");

        pointer = GameObject.Find("vrPointer");
        c_zone = GameObject.Find("ClothZone");
        c_point = GameObject.Find("CSPoint");
        O_count = GameObject.Find("O_count").GetComponent<Text>();
        X_count = GameObject.Find("X_count").GetComponent<Text>();
        
        if(pcmode)
        {
            pcplayercam = GameObject.Find("ComputerCam").GetComponent<PCPlayerController>();
            pcplayercam.enabled = false;
        }

        s_win.SetActive(true);
        e_win.SetActive(false);
        w_win.SetActive(false);
        m_win.SetActive(false);

        pointer.SetActive(false);
        c_zone.SetActive(false);
        c_point.SetActive(false);
    }

    void StartGame()
    {
        Debug.Log("Game Start!");
        ing = true;
        s_win.SetActive(false);
        pointer.SetActive(true);
        c_zone.SetActive(true);
        c_point.SetActive(true);
    }

    void EndGame()
    {
        Debug.Log("Game End!");
        //pointer.SetActive(false);
        c_zone.SetActive(false);
        c_point.SetActive(false);
        if(GameObject.Find("SelectCloth"))
            GameObject.Find("SelectCloth").SetActive(false);
    }
}
