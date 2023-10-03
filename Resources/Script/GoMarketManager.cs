using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoMarketManager : MonoBehaviour
{
    enum GMProgress
    {
        MoveStart=0,
        Moving,
        MovingEnd,
        GameStart,
        Gaming,
        GameEnd
    }

    [Header("진행상황")]
    [SerializeField] GMProgress ing;
    [Header("플레이어")]
    [SerializeField] GameObject Player;
    [Header("포인터")]
    [SerializeField] GameObject vrPointer;
    [Header("캔버스")]
    [SerializeField] GameObject StartWindow;    //시작화면
    [SerializeField] GameObject GameWindow;     //게임화면
    [SerializeField] GameObject MainPnl;        //게임시작화면
    [SerializeField] GameObject CurResultPnl;   //중간결과창
    [SerializeField] GameObject TotalResultPnl; //최종결과창
    [SerializeField] GameObject Currect;
    [SerializeField] GameObject InCurrect;
    [SerializeField] GameObject ReactTime;      //r_time
    [SerializeField] GameObject QuestionCount;   //qcount
    [Header("버튼")]
    [SerializeField] GameObject Answer;
    [Header("시장 아이템 오브젝트")]
    [SerializeField] GameObject[] Items;
    [SerializeField] GameObject[] Shows;
    [Header("문제 오브젝트 (기입용)")]
    [SerializeField] GameObject[] QuestionItem;
    [Header("랜덤 아이템 갯수 (기입용)")]
    [SerializeField] int ShowItemCount;
    [Header("이동좌표")]
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject EndPos;
    [Header("시간제한")]
    [SerializeField] Image clock;   //시계 이미지
    [SerializeField] Text time;     //시간표시
    [SerializeField] int limit;     //제한시간
    [Header("물건 맞추기 시스템")]
    [SerializeField] int AllQcount; //모든 문제 수
    [SerializeField] int Curcount;  //정답 수
    [SerializeField] int InCurcount;//오답 수
    [SerializeField] int Tempcount; //현재 문제
    [Header("문제용 오브젝트")]
    [SerializeField] GameObject QuestionObject;
    [Space(10)]
    [Header("이펙트 (기입용)")]
    [SerializeField] GameObject[] CurEffect;  //정답 이펙트
    [SerializeField] GameObject EndEffect;  //끝 이펙트


    float limit_f;  //현재 시간
    bool myanswer;  //내 선택 여부
    bool myresult;  //내 선택
    bool currectanswer; //정답

    bool makeq;     //문제 생성

    float temptime;  //반응시간 계산용
    float avrgtime; //평균반응시간 계산용
    bool starttime;

    bool start;

    private void Awake()
    {
        StartSetting();
        HideAllItem();
        ShowRandomItem();
    }

    private void Update()
    {
        switch(ing)
        {
            case GMProgress.MoveStart:
                if (start)
                {
                    //플레이어 이동 제한
                    Player.GetComponent<PlayerController>().enabled = true;
                    //GameObject.Find("WarkBox").SetActive(true);
                    vrPointer.SetActive(false);  //포인터 활성화
                    StartWindow.SetActive(false);
                    ing = GMProgress.Moving;
                }
                break;
            case GMProgress.Moving:
                ClockWork();
                break;
            case GMProgress.MovingEnd:
                //플레이어 위치 조정
                Player.transform.position = EndPos.transform.position;
                Player.transform.eulerAngles = EndPos.transform.eulerAngles;
                //플레이어 이동 제한
                Player.GetComponent<PlayerController>().enabled = false;
                GameObject.Find("WarkBox").SetActive(false);
                GameObject.Find("TimeWindow").SetActive(false); //손목 타이머 비활성
                GameWindow.SetActive(true);
                vrPointer.SetActive(true);  //포인터 활성화
                starttime = true;
                makeq = true;

                ing = GMProgress.GameStart;
                break;
            case GMProgress.GameStart:
                
                break;
            case GMProgress.Gaming:

                if(makeq)
                {
                    //문제 생성
                    MakeQuestion();
                    myanswer = false;
                    makeq = false;
                }

                //답을 선택하면!
                if(myanswer)
                {
                    MainPnl.SetActive(false);

                    QuestionResult();
                    Destroy(QuestionObject.transform.GetChild(0).gameObject);   //자식으로 생성되어 있는 문제 오브젝트 제거
                    
                    //정답일때
                    if(myresult==currectanswer)
                    {
                        Curcount += 1;  //정답 추가
                        Tempcount += 1; //문제 추가
                                        //반응 시간
                        ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "초";
                        //문제 횟수
                        QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;

                        InCurrect.SetActive(false);
                        CurResultPnl.SetActive(true);

                        //이펙트
                        for (int i = 0; i < CurEffect.Length; i++)
                        {
                            CurEffect[i].SetActive(true);
                            CurEffect[i].GetComponent<ParticleSystem>().Play();
                        }
                    }
                    //오답일때
                    else
                    {
                        InCurcount += 1;  //오답 추가
                        Tempcount += 1; //문제 추가
                                        //반응 시간
                        ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "초";
                        //문제 횟수
                        QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;

                        Currect.SetActive(false);
                        CurResultPnl.SetActive(true);
                    }
                    //전체 문제 풀이 끝!
                    if (Tempcount == AllQcount)
                    {
                        ing = GMProgress.GameEnd;
                    }
                    myanswer = false;
                }
                //문제 맞추기 전!
                else if (starttime && !myanswer && !CurResultPnl.activeSelf)
                {
                    temptime += Time.deltaTime;
                }

                break;
            case GMProgress.GameEnd:
                //패널 활성&비활성
                MainPnl.SetActive(false);
                CurResultPnl.SetActive(false);
                Currect.SetActive(true);
                InCurrect.SetActive(true);
                Answer.SetActive(false);

                if(QuestionObject.transform.childCount!=0)
                    Destroy(QuestionObject.transform.GetChild(0).gameObject);   //자식으로 생성되어 있는 문제 오브젝트 제거

                EndEffect.SetActive(true);
                EndEffect.GetComponent<ParticleSystem>().Play();


                TotalResultPnl.SetActive(true);    //결과창 출력
                GameObject.Find("ccount").GetComponent<Text>().text = Curcount.ToString(); //정답수
                GameObject.Find("incount").GetComponent<Text>().text = InCurcount.ToString(); //오답수
                GameObject.Find("avrgrtime").GetComponent<Text>().text = Mathf.Round(avrgtime / (Curcount + InCurcount)).ToString(); //평균반응시간
                GameObject.Find("accuracy").GetComponent<Text>().text = (Curcount * 10) + "%";  //정확도
                GameObject.Find("qcount").GetComponent<Text>().text = Tempcount + "/" + AllQcount;  //문제 풀이 수
                break;
        }
    }

    public void StartWind()
    {
        start = true;
    }

    //시작버튼 함수
    public void GameStart()
    {
        Debug.Log("Game Start!");
        if (TotalResultPnl.activeSelf)
            ReStartSetting();
        Answer.SetActive(true); //선택 버튼 활성화
        ing = GMProgress.Gaming;
    }
    //다음 버튼
    public void GameNext()
    {
        //문제 리셋
        QReset();
        //문제 생성
        makeq = true;
    }
    //그만두기 버튼
    public void GameDone()
    {
        ing = GMProgress.GameEnd;
    }

    //내가 선택한 정답
    public void MyAnswer(bool _cur)
    {
        myresult = _cur;
        myanswer = true;
    }

    void ReStartSetting()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        /*
        ing = GMProgress.MoveStart;

        //시장 구경 타이머
        limit_f = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;

        //내가 선택한 답과 정답
        myanswer = false;
        myresult = false;
        currectanswer = false;

        //반응 시간 
        temptime = 0;  //반응시간 계산용
        avrgtime = 0; //평균반응시간 계산용
        starttime = false;

        //문제 생성
        makeq = false;

        //정답 표기 용
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //결과창 비활성화
        TotalResultPnl.SetActive(false);

        //플레이어 위치 조정
        Player.transform.position = StartPos.transform.position;
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        //포인터 비활성화
        vrPointer.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = true;
        GameObject.Find("WarkBox").SetActive(true);
        GameObject.Find("TimeWindow").SetActive(true); //손목 타이머 활성
        GameWindow.SetActive(false);
        StartWindow.SetActive(true);

        //이펙트 비활성
        //이펙트
        EndEffect.SetActive(false);
        */
    }

    void QReset()
    {
        //시간 설정
        avrgtime += Mathf.Round(temptime);
        temptime = 0;

        myanswer = false;           //선택 초기화

        //오브젝트 활성/비활성화
        Currect.SetActive(true);
        InCurrect.SetActive(true);
        CurResultPnl.SetActive(false);
        MainPnl.SetActive(true);
    }


    void QuestionResult()
    {
        currectanswer = false;

        if (QuestionObject.transform.GetChild(0).gameObject)
        {
            for(int i=0;i<Shows.Length;i++)
            {
                if(Shows[i].name== QuestionObject.transform.GetChild(0).gameObject.name)
                {
                    currectanswer = true;
                }
            }
        }
        else
        {
            Debug.Log("생성된 문제 오브젝트가 없습니다.");
        }
    }

    //문제 오브젝트 생성
    void MakeQuestion()
    {
        int randnum = Random.Range(0, QuestionItem.Length);

        GameObject tempitem=Instantiate(QuestionItem[randnum], QuestionObject.transform.position, Quaternion.identity);
        tempitem.transform.SetParent(QuestionObject.transform);
        tempitem.name = tempitem.name.Replace("(Clone)", "");

        Debug.Log("문제 생성 완료!");
    }


    //시계 작동
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Mathf.Round(limit_f).ToString();
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //시간초가 0이 되면
        if (Mathf.Round(limit_f) == 0)
        {
            ing = GMProgress.MovingEnd;
        }
    }

    //랜덤 아이템 활성화
    void ShowRandomItem()
    {
        int showitemcounter=0;
        int tempcount = 0;
        bool retry = false;

        for(int i=0;i<ShowItemCount;i++)
        {
            int randnum = Random.Range(0, Items.Length);

            while (!retry)
            {
                if(!Items[randnum].activeSelf)
                {
                    Items[randnum].SetActive(true);
                    Debug.Log(i + "번째 등록된 아이템 : " + Items[randnum].name);
                    retry = true;
                }
                else
                {
                    randnum = Random.Range(0, Items.Length);
                }
            }
                

            for(int j=0;j<Items.Length;j++)
            {
                if(Items[randnum].name==Items[j].name)
                {
                    Items[j].SetActive(true);
                    retry = false;
                    showitemcounter++;
                }
            }
        }

        Shows = new GameObject[showitemcounter];

        for(int k=0;k<Items.Length;k++)
        {
            if (Items[k].activeSelf)
                Shows[tempcount++] = Items[k];
        }
    }

    //모든 아이템 비활성화
    void HideAllItem()
    {
        for(int i=0;i<Items.Length;i++)
        {
            Items[i].SetActive(false);
        }
    }

    void StartSetting()
    {
        //시장 구경 타이머
        limit_f = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;

        //내가 선택한 답과 정답
        myanswer = false;
        myresult = false;
        currectanswer = false;

        //반응 시간 
        temptime=0;  //반응시간 계산용
        avrgtime=0; //평균반응시간 계산용
        starttime = false ;

        //문제 생성
        makeq = false;

        //정답 표기 용
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        start = false;

        Items = GameObject.FindGameObjectsWithTag("Item");  //해당 태그 오브젝트 저장

        Player = GameObject.Find("Player");
        vrPointer = GameObject.Find("vrPointer");
        StartWindow = GameObject.Find("StartWindow");
        GameWindow = GameObject.Find("GameWindow");
        MainPnl = GameObject.Find("MainPnl");
        CurResultPnl = GameObject.Find("CurResultPnl");
        TotalResultPnl = GameObject.Find("TotalResultPnl");
        Currect = GameObject.Find("Currect");
        InCurrect = GameObject.Find("InCurrect");
        Answer = GameObject.Find("Answer");
        
        QuestionObject = GameObject.Find("QuestionObject");

        //오브젝트 비활성화
        CurResultPnl.SetActive(false);
        TotalResultPnl.SetActive(false);
        GameWindow.SetActive(false);
        Answer.SetActive(false);

        //플레이어 위치 조정
        Player.transform.position = StartPos.transform.position;
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        //플레이어 이동 제한
        Player.GetComponent<PlayerController>().enabled = false;
        //GameObject.Find("WarkBox").SetActive(false);
        //포인터 비활성화
        //vrPointer.SetActive(false);
        //이펙트 비활성
        //이펙트
        EndEffect.SetActive(false);
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }
    }
}
