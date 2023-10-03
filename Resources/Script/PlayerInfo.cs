using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "PlayerInfo", fileName = "playerinfo")]
public class PlayerInfo : ScriptableObject
{
    //�⺻ ����
    string id="id";                                              //���̵�
    string pw = "pw";                                            //��й�ȣ
    string nickname = "nickname";                                //�г���

    //������������ ����
    List<OCGInfo> ocginfo = new List<OCGInfo>();                 //ȸ���� ���
    int highcorrect = 0;                                         //�ְ� �����
    int lowincorrect = 0;                                        //���� �����
    int avrgtime = 0;                                            //��� �ҿ�ð�
    int avrgcorrect = 0;                                         //��� �����
    int avrgincorrect = 0;                                       //��� ����� 

    List<OCGInfo> w_ocginfo = new List<OCGInfo>();                                            //�ְ� ���� ���
    int w_highcorrect = 0;                                         //�ְ� �����
    int w_lowincorrect = 0;                                        //���� �����
    int w_avrgtime = 0;                                            //��� �ҿ�ð�
    int w_avrgcorrect = 0;                                         //��� �����
    int w_avrgincorrect = 0;                                       //��� ����� 

    List<OCGInfo> m_ocginfo = new List<OCGInfo>();                                            //���� ���� ���
    int m_highcorrect = 0;                                         //�ְ� �����
    int m_lowincorrect = 0;                                        //���� �����
    int m_avrgtime = 0;                                            //��� �ҿ�ð�
    int m_avrgcorrect = 0;                                         //��� �����
    int m_avrgincorrect = 0;                                       //��� ����� 


    //������������ 1ȸ �÷��� ���� �߰�
    public void AddOCGInfo(DateTime _date, int _time, int _cor, int _incor)
    {
        OCGInfo addinfo=new OCGInfo();                      //1ȸ �÷��� �ӽ� ��ü
        addinfo.SetOCGInfo(_date, _time, _cor, _incor);     //�÷��� ��� ����
        ocginfo.Add(addinfo);                               //���ο� �÷��� ���� �߰�
    }

    //�ְ�&���� �÷��� ���� �߰�
    public void InputWMOCGInfo()
    {
        //���ԵǴ� ���� ��� ������� ����
        //�ְ�����:�ֱ� ����� ������ ���� 7�� (EX) 11.01~11.07[�ֱٱ�����]
        //��������:���� ���� ���������� ���� ���� (EX) 11.01~11.20[�ֱٱ�����]

        OCGInfo lastdate = ocginfo[ocginfo.Count];               //������ �÷��� ��¥:���� �ֽ� ���
        string lastyear = lastdate.GetDate().Year.ToString();    //������ �÷��� ��¥�� �⵵
        string lastmonth = lastdate.GetDate().Month.ToString();  //������ �÷��� ��¥�� ��

        //��� ȸ�� �˻�(����)
        for(int i=ocginfo.Count;i>0;i--)
        {
            //(�ְ�)������ �÷��� ��¥�� 7�� �̻� ���̳��� ������
            if((lastdate.GetDate()-ocginfo[i].GetDate()).Days<7)
            {
                //�ְ� ���� �߰�
                w_ocginfo.Add(ocginfo[i]);
            }

            //(����)������ �÷��� ��¥�� ���� �⵵�� ���� �����ϴ� ��¥ 
            if(lastyear==ocginfo[i].GetDate().Year.ToString()&&
                lastmonth== ocginfo[i].GetDate().Month.ToString())
            {
                //���� ���� �߰�
                m_ocginfo.Add(ocginfo[i]);
            }
        }
    }

    //�÷��̾� �⺻ ���� ����
    public void SetBasicInfo(string _id,string _pw,string _nickname)
    {
        id = _id;
        pw = _pw;
        nickname = _nickname;
    }

    //��ü/�ְ�/���� ���� ����
    public void SetAllInfo()
    {
        //��ü
        highcorrect = GetHighCorrect(ocginfo);      //�ְ������
        lowincorrect = GetLowInCorrect(ocginfo);    //���������
        avrgtime = GetAvrgTime(ocginfo);            //��ռҿ�ð�
        avrgcorrect = GetAvrgCorrect(ocginfo);      //��������
        avrgincorrect = GetAvrgInCorrect(ocginfo);  //��տ����

        //�ְ�
        w_highcorrect = GetHighCorrect(w_ocginfo);      //�ְ������
        w_lowincorrect = GetLowInCorrect(w_ocginfo);    //���������
        w_avrgtime = GetAvrgTime(w_ocginfo);            //��ռҿ�ð�
        w_avrgcorrect = GetAvrgCorrect(w_ocginfo);      //��������
        w_avrgincorrect = GetAvrgInCorrect(w_ocginfo);  //��տ����

        //����
        m_highcorrect = GetHighCorrect(m_ocginfo);      //�ְ������
        m_lowincorrect = GetLowInCorrect(m_ocginfo);    //���������
        m_avrgtime = GetAvrgTime(m_ocginfo);            //��ռҿ�ð�
        m_avrgcorrect = GetAvrgCorrect(m_ocginfo);      //��������
        m_avrgincorrect = GetAvrgInCorrect(m_ocginfo);  //��տ����
    }

    //��� ���� �⺻ �ʱ�ȭ
    public void SetCleanInfo()
    {
        //�÷��̾� �⺻ ����
        id = "";
        pw = "";
        nickname = "";
        //��ü ȸ�� ����
        ocginfo.Clear();    //����Ʈ ����
        highcorrect = 0;
        lowincorrect = 0;
        avrgtime = 0;
        avrgcorrect = 0;
        avrgincorrect = 0;
        //�ְ� ȸ�� ����
        w_ocginfo.Clear();    //����Ʈ ����
        w_highcorrect = 0;
        w_lowincorrect = 0;
        w_avrgtime = 0;
        w_avrgcorrect = 0;
        w_avrgincorrect = 0;
        //���� ȸ�� ����
        m_ocginfo.Clear();    //����Ʈ ����
        m_highcorrect = 0;
        m_lowincorrect = 0;
        m_avrgtime = 0;
        m_avrgcorrect = 0;
        m_avrgincorrect = 0;
    }

    //�ְ� ����� ����
    public int GetHighCorrect(List<OCGInfo> _ocginfo)
    {
        //�ӽ� �ְ� ���� ������ ù��° ȸ���� ���� Ƚ���� ����(�񱳿�)
        int temphigh = _ocginfo[0].GetCorAnswer();

        //�÷��̾��� ��� ȸ���� �˻�
        for (int i=0;i< _ocginfo.Count;i++)
        {
            //�ӽ� �ְ� ���� Ƚ������ ���� ���
            if(temphigh< _ocginfo[i].GetCorAnswer())
            {
                temphigh = _ocginfo[i].GetCorAnswer();   //�ְ��� ����
            }
        }

        return temphigh; //�ְ����� ��ȯ
    }

    //���� ����� ����
    public int GetLowInCorrect(List<OCGInfo> _ocginfo)
    {
        //�ӽ� ���� ���� ������ ù��° ȸ���� ���� Ƚ���� ����(�񱳿�)
        int templow = _ocginfo[0].GetInCorAnswer();

        //�÷��̾��� ��� ȸ���� �˻�
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            //�ӽ� �ְ� ���� Ƚ������ ���� ���
            if (templow> _ocginfo[i].GetInCorAnswer())
            {
                templow = _ocginfo[i].GetInCorAnswer();   //������ ����
            }
        }

        return templow; //�������� ��ȯ
    }

    //��� �ҿ�ð� ����
    public int GetAvrgTime(List<OCGInfo> _ocginfo)
    {
        int alltime = 0;    //��� ȸ���� �ҿ�ð� ���� ����

        //�÷��̾��� ��� ȸ���� �˻�
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            alltime += _ocginfo[i].GetPlayTime();        //��� ȸ�� �ҿ� �ð� �ջ�
        }

        return (int)(alltime / _ocginfo.Count);         //��� �ҿ�ð� ��ȯ
    }

    //��� ����� ����
    public int GetAvrgCorrect(List<OCGInfo> _ocginfo)
    {
        int allcorrect = 0;                         //��� ȸ���� ����� ���� ����

        //�÷��̾��� ��� ȸ���� �˻�
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            allcorrect += _ocginfo[i].GetCorAnswer();        //��� ȸ�� ����� �ջ�
        }

        return (int)(allcorrect / _ocginfo.Count);         //��� ����� ��ȯ
    }

    //��� ����� ����
    public int GetAvrgInCorrect(List<OCGInfo> _ocginfo)
    {
        int allincorrect = 0;                         //��� ȸ���� ����� ���� ����

        //�÷��̾��� ��� ȸ���� �˻�
        for (int i = 0; i < _ocginfo.Count; i++)
        {
            allincorrect += _ocginfo[i].GetInCorAnswer();        //��� ȸ�� ����� �ջ�
        }

        return (int)(allincorrect / _ocginfo.Count);         //��� ����� ��ȯ
    }

    //��ü���� ��� ���� �Լ�
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
    //�ְ����� ��� ���� �Լ�
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
    //�������� ��� ���� �Լ�
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
