using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "PlayerInfo", fileName = "playerinfo")]
public class PlayerInfo : ScriptableObject
{
    //기본 정보
    string id="id";                                              //아이디
    string pw = "pw";                                            //비밀번호
    string nickname = "nickname";                                //닉네임

    //옷장정리게임 정보
    List<OCGInfo> ocginfo = new List<OCGInfo>();                 //회차별 기록
    int highcorrect = 0;                                         //최고 정답수
    int lowincorrect = 0;                                        //최저 오답수
    int avrgtime = 0;                                            //평균 소요시간
    int avrgcorrect = 0;                                         //평균 정답수
    int avrgincorrect = 0;                                       //평균 오답수 

    List<OCGInfo> w_ocginfo = new List<OCGInfo>();                                            //주간 게임 기록
    int w_highcorrect = 0;                                         //최고 정답수
    int w_lowincorrect = 0;                                        //최저 오답수
    int w_avrgtime = 0;                                            //평균 소요시간
    int w_avrgcorrect = 0;                                         //평균 정답수
    int w_avrgincorrect = 0;                                       //평균 오답수 

    List<OCGInfo> m_ocginfo = new List<OCGInfo>();                                            //월간 게임 기록
    int m_highcorrect = 0;                                         //최고 정답수
    int m_lowincorrect = 0;                                        //최저 오답수
    int m_avrgtime = 0;                                            //평균 소요시간
    int m_avrgcorrect = 0;                                         //평균 정답수
    int m_avrgincorrect = 0;                                       //평균 오답수 


    //옷장정리게임 1회 플레이 정보 추가
    public void AddOCGInfo(DateTime _date, int _time, int _cor, int _incor)
    {
        OCGInfo addinfo=new OCGInfo();                      //1회 플레이 임시 객체
        addinfo.SetOCGInfo(_date, _time, _cor, _incor);     //플레이 기록 설정
        ocginfo.Add(addinfo);                               //새로운 플레이 정보 추가
    }

    //주간&월간 플레이 정보 추가
    public void InputWMOCGInfo()
    {
        //대입되는 값은 모두 비어있지 않음
        //주간기준:최근 기록을 포함한 지난 7일 (EX) 11.01~11.07[최근기준점]
        //월간기준:현재 달을 기준으로한 지난 날들 (EX) 11.01~11.20[최근기준점]

        OCGInfo lastdate = ocginfo[ocginfo.Count];               //마지막 플레이 날짜:가장 최신 기록
        string lastyear = lastdate.GetDate().Year.ToString();    //마지막 플레이 날짜의 년도
        string lastmonth = lastdate.GetDate().Month.ToString();  //마지막 플레이 날짜의 월

        //모든 회차 검색(역순)
        for(int i=ocginfo.Count;i>0;i--)
        {
            //(주간)마지막 플레이 날짜와 7일 이상 차이나지 않을때
            if((lastdate.GetDate()-ocginfo[i].GetDate()).Days<7)
            {
                //주간 정보 추가
                w_ocginfo.Add(ocginfo[i]);
            }

            //(월간)마지막 플레이 날짜와 같은 년도와 월을 공유하는 날짜 
            if(lastyear==ocginfo[i].GetDate().Year.ToString()&&
                lastmonth== ocginfo[i].GetDate().Month.ToString())
            {
                //월간 정보 추가
                m_ocginfo.Add(ocginfo[i]);
            }
        }
    }

    //플레이어 기본 정보 대입
    public void SetBasicInfo(string _id,string _pw,string _nickname)
    {
        id = _id;
        pw = _pw;
        nickname = _nickname;
    }

    //전체/주간/월간 정보 정리
    public void SetAllInfo()
    {
        //전체
        highcorrect = GetHighCorrect(ocginfo);      //최고정답수
        lowincorrect = GetLowInCorrect(ocginfo);    //최저오답수
        avrgtime = GetAvrgTime(ocginfo);            //평균소요시간
        avrgcorrect = GetAvrgCorrect(ocginfo);      //평균정답수
        avrgincorrect = GetAvrgInCorrect(ocginfo);  //평균오답수

        //주간
        w_highcorrect = GetHighCorrect(w_ocginfo);      //최고정답수
        w_lowincorrect = GetLowInCorrect(w_ocginfo);    //최저오답수
        w_avrgtime = GetAvrgTime(w_ocginfo);            //평균소요시간
        w_avrgcorrect = GetAvrgCorrect(w_ocginfo);      //평균정답수
        w_avrgincorrect = GetAvrgInCorrect(w_ocginfo);  //평균오답수

        //월간
        m_highcorrect = GetHighCorrect(m_ocginfo);      //최고정답수
        m_lowincorrect = GetLowInCorrect(m_ocginfo);    //최저오답수
        m_avrgtime = GetAvrgTime(m_ocginfo);            //평균소요시간
        m_avrgcorrect = GetAvrgCorrect(m_ocginfo);      //평균정답수
        m_avrgincorrect = GetAvrgInCorrect(m_ocginfo);  //평균오답수
    }

    //모든 변수 기본 초기화
    public void SetCleanInfo()
    {
        //플레이어 기본 정보
        id = "";
        pw = "";
        nickname = "";
        //전체 회차 정보
        ocginfo.Clear();    //리스트 비우기
        highcorrect = 0;
        lowincorrect = 0;
        avrgtime = 0;
        avrgcorrect = 0;
        avrgincorrect = 0;
        //주간 회차 정보
        w_ocginfo.Clear();    //리스트 비우기
        w_highcorrect = 0;
        w_lowincorrect = 0;
        w_avrgtime = 0;
        w_avrgcorrect = 0;
        w_avrgincorrect = 0;
        //월간 회차 정보
        m_ocginfo.Clear();    //리스트 비우기
        m_highcorrect = 0;
        m_lowincorrect = 0;
        m_avrgtime = 0;
        m_avrgcorrect = 0;
        m_avrgincorrect = 0;
    }

    //최고 정답수 산출
    public int GetHighCorrect(List<OCGInfo> _ocginfo)
    {
        //임시 최고 정답 갯수에 첫번째 회차의 정답 횟수를 대입(비교용)
        int temphigh = _ocginfo[0].GetCorAnswer();

        //플레이어의 모든 회차를 검색
        for (int i=0;i< _ocginfo.Count;i++)
        {
            //임시 최고 정답 횟수보다 높을 경우
            if(temphigh< _ocginfo[i].GetCorAnswer())
            {
                temphigh = _ocginfo[i].GetCorAnswer();   //최고점 갱신
            }
        }

        return temphigh; //최고점값 반환
    }

    //최저 오답수 산출
    public int GetLowInCorrect(List<OCGInfo> _ocginfo)
    {
        //임시 최저 오답 갯수에 첫번째 회차의 오답 횟수를 대입(비교용)
        int templow = _ocginfo[0].GetInCorAnswer();

        //플레이어의 모든 회차를 검색
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            //임시 최고 정답 횟수보다 높을 경우
            if (templow> _ocginfo[i].GetInCorAnswer())
            {
                templow = _ocginfo[i].GetInCorAnswer();   //최저점 갱신
            }
        }

        return templow; //최저점값 반환
    }

    //평균 소요시간 산출
    public int GetAvrgTime(List<OCGInfo> _ocginfo)
    {
        int alltime = 0;    //모든 회차의 소요시간 변수 선언

        //플레이어의 모든 회차를 검색
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            alltime += _ocginfo[i].GetPlayTime();        //모든 회차 소요 시간 합산
        }

        return (int)(alltime / _ocginfo.Count);         //평균 소요시간 반환
    }

    //평균 정답수 산출
    public int GetAvrgCorrect(List<OCGInfo> _ocginfo)
    {
        int allcorrect = 0;                         //모든 회차의 정답수 변수 선언

        //플레이어의 모든 회차를 검색
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            allcorrect += _ocginfo[i].GetCorAnswer();        //모든 회차 정답수 합산
        }

        return (int)(allcorrect / _ocginfo.Count);         //평균 정답수 반환
    }

    //평균 오답수 산출
    public int GetAvrgInCorrect(List<OCGInfo> _ocginfo)
    {
        int allincorrect = 0;                         //모든 회차의 오답수 변수 선언

        //플레이어의 모든 회차를 검색
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            allincorrect += _ocginfo[i].GetInCorAnswer();        //모든 회차 오답수 합산
        }

        return (int)(allincorrect / _ocginfo.Count);         //평균 오답수 반환
    }

    //전체게임 기록 산출 함수
    public int GetHighCor()
    {
        return highcorrect;
    }
    public int GetLowInCor()
    {
        return lowincorrect;
    }
    public int GetAvrgT()
    {
        return avrgtime;
    }
    public int GetAvrgC()
    {
        return avrgcorrect;
    }
    public int GetAvrgInC()
    {
        return avrgincorrect;
    }
    //주간게임 기록 산출 함수
    public int GetWHighCor()
    {
        return w_highcorrect;
    }
    public int GetWLowInCor()
    {
        return w_lowincorrect;
    }
    public int GetWAvrgT()
    {
        return w_avrgtime;
    }
    public int GetWAvrgC()
    {
        return w_avrgcorrect;
    }
    public int GetWAvrgInC()
    {
        return w_avrgincorrect;
    }
    //월간게임 기록 산출 함수
    public int GetMHighCor()
    {
        return m_highcorrect;
    }
    public int GetMLowInCor()
    {
        return m_lowincorrect;
    }
    public int GetMAvrgT()
    {
        return m_avrgtime;
    }
    public int GetMAvrgC()
    {
        return m_avrgcorrect;
    }
    public int GetMAvrgInC()
    {
        return m_avrgincorrect;
    }
}
