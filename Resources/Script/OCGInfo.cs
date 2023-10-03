using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OCGInfo : ScriptableObject
{
    DateTime play_date = new DateTime();            //�÷��� ��¥
    int play_time = 0;                               //�ҿ�ð�
    int correct_answer = 0;                          //����
    int incorrect_answer = 0;                        //����

    public void SetOCGInfo(DateTime _date,int _time,int _cor,int _incor)
    {
        play_date = _date;
        play_time = _time;
        correct_answer = _cor;
        incorrect_answer = _incor;
    }

    public DateTime GetDate()
    {
        return play_date;
    }

    public int GetPlayTime()
    {
        return play_time;
    }

    public int GetCorAnswer()
    {
        return correct_answer;
    }

    public int GetInCorAnswer()
    {
        return incorrect_answer;
    }


}
