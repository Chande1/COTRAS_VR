using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandReckonManager : MonoBehaviour
{
    [Header("설명 패널")]
    [SerializeField] GameObject Desciption;
    [Header("문제 결과 패널")]
    [SerializeField] GameObject CurResult;
    [SerializeField] GameObject Currect;
    [SerializeField] GameObject InCurrect;
    [SerializeField] GameObject ReactTime; //rt_text
    [SerializeField] GameObject QuestionCount;  //qc_text
    [Header("최종 결과 패널")]
    [SerializeField] GameObject TotalResult;
    [Header("하단 버튼")]
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject QuitButton;
    [Header("사칙연산 오브젝트 (기입용)")]
    [SerializeField] GameObject Calc;
    [SerializeField] GameObject Numbers;
    [SerializeField] GameObject Hand_Right;
    [SerializeField] GameObject Hand_Left;
    [Header("사칙연산 오브젝트 (확인용)")]
    [SerializeField] GameObject[] calc;
    [SerializeField] GameObject[] number;
    [SerializeField] GameObject[] righthand;
    [SerializeField] GameObject[] lefthand;
    [Space(10)]
    [Header("사직연산 시스템")]
    [ContextMenuItem("RandomQMake", "RandomQMake")] //우클 버튼 랜덤 문제 출제 함수 호출
    [SerializeField] string NowQText;   //현재 문제 표기
    [SerializeField] int calcobjectnum;       //부호 번호
    [SerializeField] int lefthandnum;   //왼손 번호
    [SerializeField] int righthandnum;  //오른손 번호
    [SerializeField] int resultnum;     //결과값
    [Space(10)]
    [Header("숫자 맞추기 시스템")]
    [SerializeField] int AllQcount; //문제 수
    [SerializeField] int Curcount;  //정답 수
    [SerializeField] int InCurcount;//오답 수
    [SerializeField] int Tempcount; //현재 문제
    [Space(10)]
    [Header("이펙트 (기입용)")]
    [SerializeField] GameObject[] CurEffect;  //정답 이펙트
    [SerializeField] GameObject EndEffect;  //끝 이펙트

    bool start; //문제 시작
    bool qing;  //문제 진행중
    bool end;   //문제 끝
    int myresult;   //내가 선택한 정답
    bool myanswer;  //정답을 선택했는지
    float temptime;  //반응시간 계산용
    float avrgtime; //평균반응시간 계산용
    bool starttime;
    private void Awake()
    {
        //사칙 연산 오브젝트 자식 오브젝트 세팅
        SettingReckonObject();
        //시작 세팅
        StartSetting();
    }

    void Update()
    {
        if(!end)
        {
            if (start)
            {
                qing = true;
                start = false;
            }

            if (qing)
            {
                Desciption.SetActive(true);
                RandomQMake();
                ShowHandReckon();
                qing = false;
            }

            if (myanswer)
            {
                Desciption.SetActive(false);
                //부호 모형 비활성화
                calc[calcobjectnum].SetActive(false);
                //손 모형 비활성화
                righthand[righthandnum - 1].SetActive(false);
                lefthand[lefthandnum - 1].SetActive(false);

                //정답일때
                if (resultnum == myresult)
                {
                    Curcount += 1;  //정답 추가
                    Tempcount += 1; //문제 추가
                                    //반응 시간
                    ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "초";
                    //문제 횟수
                    QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;
                    //패널 활성&비활성
                    CurResult.SetActive(true);
                    InCurrect.SetActive(false);
                    //이펙트
                    for(int i=0;i<CurEffect.Length;i++)
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
                    //패널 활성&비활성
                    CurResult.SetActive(true);
                    Currect.SetActive(false);
                }

                //전체 문제 풀이 끝!
                if(Tempcount==AllQcount)
                {
                    end = true;
                }

                myanswer = false;
            }
            else if (starttime && !myanswer && !CurResult.activeSelf)
            {
                temptime += Time.deltaTime;
            }
        }
        //문제 끝!
        else
        {
            if(!TotalResult.activeSelf)
            {
                EndReset(); //결과창 출력
            }
        }
    }

    //시작버튼 함수
    public void GameStart()
    {
        Debug.Log("Game Start!");
        if (TotalResult.activeSelf)
            ReStartSetting();
        start = true;
        starttime = true;
    }
    //숫자 오브젝트 클릭시 대답 결정
    public void MyAnswer(int _num)
    {
        Debug.Log("My choice is " + _num);
        myresult = _num;
        myanswer = true;
    }
    //다음 버튼
    public void GameNext()
    {
        //문제 리셋
        QReset();
        qing = true;
    }
    //그만두기 버튼
    public void GameDone()
    {
        //부호 모형 비활성화
        calc[calcobjectnum].SetActive(false);
        //손 모형 비활성화
        righthand[righthandnum - 1].SetActive(false);
        lefthand[lefthandnum - 1].SetActive(false);

        end = true;
    }


    //결과창을 위한 리셋
    void EndReset()
    {
        //패널 활성&비활성
        Desciption.SetActive(false);
        CurResult.SetActive(false);
        Currect.SetActive(true);
        InCurrect.SetActive(true);
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(false);
        EndEffect.SetActive(true);
        EndEffect.GetComponent<ParticleSystem>().Play();


        TotalResult.SetActive(true);    //결과창 출력
        GameObject.Find("ccount").GetComponent<Text>().text = Curcount.ToString(); //정답수
        GameObject.Find("incount").GetComponent<Text>().text = InCurcount.ToString(); //오답수
        GameObject.Find("avrgrtime").GetComponent<Text>().text = Mathf.Round(avrgtime/(Curcount+InCurcount)).ToString(); //평균반응시간
        GameObject.Find("accuracy").GetComponent<Text>().text = (Curcount * 10) + "%";  //정확도
        GameObject.Find("qcount").GetComponent<Text>().text = Tempcount + "/" + AllQcount;  //문제 풀이 수
    }

    //다음문제를 위한 리셋
    void QReset()
    {
        //시간 설정
        avrgtime += Mathf.Round(temptime);
        temptime = 0;
        //패널 활성&비활성
        CurResult.SetActive(false);
        Currect.SetActive(true);
        InCurrect.SetActive(true);
        //이펙트 비활성
        //이펙트
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }

    }

    void SettingReckonObject()
    {
        //연산 기호
        calc = new GameObject[Calc.transform.childCount];
        for (int i = 0; i < Calc.transform.childCount; i++)
        {
            calc[i] = Calc.transform.GetChild(i).gameObject;
        }

        //숫자
        number = new GameObject[Numbers.transform.childCount];
        for(int j=0;j<Numbers.transform.childCount;j++)
        {
            number[j] = Numbers.transform.GetChild(j).gameObject;
        }

        //오른손
        righthand = new GameObject[Hand_Right.transform.childCount];
        for (int j = 0; j < Hand_Right.transform.childCount; j++)
        {
            righthand[j] = Hand_Right.transform.GetChild(j).gameObject;
        }

        //왼손
        lefthand = new GameObject[Hand_Left.transform.childCount];
        for (int j = 0; j < Hand_Left.transform.childCount; j++)
        {
            lefthand[j] = Hand_Left.transform.GetChild(j).gameObject;
        }
    }

    void ReStartSetting()
    {
        start = false;
        qing = false;
        end = false;
        myanswer = false;
        starttime = false;

        myresult = 0;
        calcobjectnum = 0;
        righthandnum = 0;
        lefthandnum = 0;
        resultnum = 0;

        //타이머용
        temptime = 0;
        avrgtime = 0;

        //정답 표기 용
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //결과창 비활성화
        TotalResult.SetActive(false);

        //이펙트 비활성화
        EndEffect.SetActive(false);
    }

    void StartSetting()
    {
        start = false;
        qing = false;
        end = false;
        myanswer = false;
        starttime = false;

        myresult = 0;
        calcobjectnum = 0;
        righthandnum = 0;
        lefthandnum = 0;
        resultnum = 0;

        //타이머용
        temptime = 0;
        avrgtime = 0;

        //정답 표기 용
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //칠판 패널
        Desciption = GameObject.Find("Description");
        CurResult = GameObject.Find("CurResult");
        Currect = GameObject.Find("Currect");
        InCurrect = GameObject.Find("InCurrect");
        ReactTime = GameObject.Find("rt_text");
        QuestionCount = GameObject.Find("qc_text");
        TotalResult = GameObject.Find("TotalResult");
        StartButton = GameObject.Find("StartBtn");
        NextButton = GameObject.Find("NextBtn");
        QuitButton = GameObject.Find("EndBtn");

        //칠판 패널 비활성화
        //Desciption.SetActive(false);
        CurResult.SetActive(false);
        TotalResult.SetActive(false);
        //StartButton.SetActive(false);
        //NextButton.SetActive(false);
        //QuitButton.SetActive(false);

        //사칙연산 오브젝트 비활성화
        for (int i = 0; i < calc.Length; i++)
            calc[i].gameObject.SetActive(false);
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(false);
        for (int i = 0; i < righthand.Length; i++)
            righthand[i].gameObject.SetActive(false);
        for (int i = 0; i < lefthand.Length; i++)
            lefthand[i].gameObject.SetActive(false);

        //이펙트 비활성화
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }
        EndEffect.SetActive(false);
    }

    //0~9까지 출력되는 사칙연산 프로그램
    void RandomQMake()
    {
        //int calcnum = 2;
        int randnum = Random.Range(1, 10);   //부호
        int rightnum = Random.Range(1, 5);  //오른손
        int leftnum = Random.Range(1, 5);   //왼손
        int calcnum=0;
        int result = 0;                     //결과값
        
        if (randnum > 5)
            calcnum = 0;
        else
            calcnum = 1;


        switch (calcnum)
        {
            //더하기 일 때
            case 0:
                //10이상의 수가 나오면 안되므로 오른손이 5일때 왼손은 5 제외
                if (rightnum >= 5)
                {
                    leftnum = Random.Range(1, 4);
                }

                result = (leftnum + rightnum);
                NowQText = "(왼손)" + leftnum + "+ (오른손)" + rightnum + "=" + result;
                break;
            //빼기 일 때
            case 1:
                //왼손의 수가 오른손보다 크거나 같아야 함
                if (rightnum > leftnum)
                {
                    leftnum = Random.Range(rightnum, 5);
                }
                result = (leftnum - rightnum);
                NowQText = "(왼손)" + leftnum + "- (오른손)" + rightnum + "=" + result;
                break;
            default:
                break;
        }

        //값 입력
        calcobjectnum = calcnum;
        lefthandnum = leftnum;
        righthandnum = rightnum;
        resultnum = result;
    }
    //사칙 연산 모형 출력
    void ShowHandReckon()
    {
        //사칙연산 오브젝트 비활성화
        for (int i = 0; i < calc.Length; i++)
            calc[i].gameObject.SetActive(false);
        for (int i = 0; i < righthand.Length; i++)
            righthand[i].gameObject.SetActive(false);
        for (int i = 0; i < lefthand.Length; i++)
            lefthand[i].gameObject.SetActive(false);

        //부호 모형 화면에 출력
        calc[calcobjectnum].SetActive(true);
        //손 모형 화면에 출력
        righthand[righthandnum-1].SetActive(true);
        lefthand[lefthandnum-1].SetActive(true);
        //숫자 모형 출력
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(true);
    }
   
}
