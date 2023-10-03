using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using System;

public class OCGResult
{
    public string playdate;  //�÷��� ��¥
    public int playtime;   //�ҿ� �ð�
    public int correct;    //����
    public int incorrect;  //����           
}


public class ClothGameManager : MonoBehaviour
{
    [Header("PC���")]
    [SerializeField] bool pcmode;
    [SerializeField] PCPlayerController pcplayercam;
    [Header("������ ������Ʈ")]
    [SerializeField] Hand RHand;
    [SerializeField] GameObject s_win;   //����â
    [SerializeField] GameObject e_win;   //����â
    [SerializeField] GameObject w_win;   //�ְ�â
    [SerializeField] GameObject m_win;   //����â
    [SerializeField] GameObject pointer; //������
    [SerializeField] GameObject c_zone;  //����
    [SerializeField] GameObject c_point; //��

    [Header("(���Կ�)")]
    [SerializeField] Text R_date;
    [SerializeField] Text R_time;
    [SerializeField] Text R_correct;
    [SerializeField] Text R_incorrect;

    [Header("����/����")]
    [SerializeField] Text O_count;
    [SerializeField] Text X_count;
    [SerializeField] int ox_count;  //��ü ���� ��
    [Header("�ð�����")]
    [SerializeField] Image clock;   //�ð� �̹���
    [SerializeField] Text time;     //�ð�ǥ��
    [SerializeField] int limit;     //���ѽð�
    float limit_f;


    OCGResult result=new OCGResult();
    
    
    bool start; //���۹�ư
    bool ing;   //������
    bool end;   //�����ߴ�

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
            //pc���
            if(pcmode)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    pcplayercam.enabled = true;
                    StartGame(); //���ӽ���!
                    start = false;
                }
            }
            //vr���
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //������ Ʈ���Ű� ���������̸�
            {
                StartGame(); //���ӽ���!
                start = false;
            }
        }
        if(ing)
        {
            CheckGame();    //�������� Ȯ��
            ClockWork();    //�ð�����
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
            //pc���
            if(pcmode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    w_win.SetActive(false);
                }
            }
            //vr���
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //������ Ʈ���Ű� ���������̸�
            {
                Debug.Log("�ְ�â ����");
                w_win.SetActive(false);
            }
        }
        if (m_win.activeSelf)
        {
            //pc���
            if(pcmode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_win.SetActive(false);
                }
            }
            //vr���
            else if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)) //������ Ʈ���Ű� ���������̸�
            {
                Debug.Log("����â ����");
                m_win.SetActive(false);
            }
        }

    }

    //�ð� �۵�
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Math.Round(limit_f).ToString();
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime*temp*0.001f);

        //�ð��ʰ� 0�� �Ǹ�
        if(Math.Round(limit_f)==0)
        {
            ing = false;    //���� ����
            end = true;       
        }
    }

    //���� ���� Ȯ��
    void CheckGame()
    {
        int temp = int.Parse(O_count.text) + int.Parse(X_count.text);
        
        //�� �̻� �ǻ��� ������
        if (temp==ox_count)
        {
            ing = false;
            end = true;
        }
    }

    void MakeResult()
    {
        result.playdate = DateTime.Now.ToString("yyyy. MM. dd");   //���� ��¥ �Է�
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
        Debug.Log("�ְ�â ����");
        w_win.SetActive(true);
    }

    public void MonthButton()
    {
        Debug.Log("����â ����");
        m_win.SetActive(true);
    }


    //������ ����
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
