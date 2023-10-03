using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandReckonManager : MonoBehaviour
{
    [Header("���� �г�")]
    [SerializeField] GameObject Desciption;
    [Header("���� ��� �г�")]
    [SerializeField] GameObject CurResult;
    [SerializeField] GameObject Currect;
    [SerializeField] GameObject InCurrect;
    [SerializeField] GameObject ReactTime; //rt_text
    [SerializeField] GameObject QuestionCount;  //qc_text
    [Header("���� ��� �г�")]
    [SerializeField] GameObject TotalResult;
    [Header("�ϴ� ��ư")]
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject QuitButton;
    [Header("��Ģ���� ������Ʈ (���Կ�)")]
    [SerializeField] GameObject Calc;
    [SerializeField] GameObject Numbers;
    [SerializeField] GameObject Hand_Right;
    [SerializeField] GameObject Hand_Left;
    [Header("��Ģ���� ������Ʈ (Ȯ�ο�)")]
    [SerializeField] GameObject[] calc;
    [SerializeField] GameObject[] number;
    [SerializeField] GameObject[] righthand;
    [SerializeField] GameObject[] lefthand;
    [Space(10)]
    [Header("�������� �ý���")]
    [ContextMenuItem("RandomQMake", "RandomQMake")] //��Ŭ ��ư ���� ���� ���� �Լ� ȣ��
    [SerializeField] string NowQText;   //���� ���� ǥ��
    [SerializeField] int calcobjectnum;       //��ȣ ��ȣ
    [SerializeField] int lefthandnum;   //�޼� ��ȣ
    [SerializeField] int righthandnum;  //������ ��ȣ
    [SerializeField] int resultnum;     //�����
    [Space(10)]
    [Header("���� ���߱� �ý���")]
    [SerializeField] int AllQcount; //���� ��
    [SerializeField] int Curcount;  //���� ��
    [SerializeField] int InCurcount;//���� ��
    [SerializeField] int Tempcount; //���� ����
    [Space(10)]
    [Header("����Ʈ (���Կ�)")]
    [SerializeField] GameObject[] CurEffect;  //���� ����Ʈ
    [SerializeField] GameObject EndEffect;  //�� ����Ʈ

    bool start; //���� ����
    bool qing;  //���� ������
    bool end;   //���� ��
    int myresult;   //���� ������ ����
    bool myanswer;  //������ �����ߴ���
    float temptime;  //�����ð� ����
    float avrgtime; //��չ����ð� ����
    bool starttime;
    private void Awake()
    {
        //��Ģ ���� ������Ʈ �ڽ� ������Ʈ ����
        SettingReckonObject();
        //���� ����
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
                //��ȣ ���� ��Ȱ��ȭ
                calc[calcobjectnum].SetActive(false);
                //�� ���� ��Ȱ��ȭ
                righthand[righthandnum - 1].SetActive(false);
                lefthand[lefthandnum - 1].SetActive(false);

                //�����϶�
                if (resultnum == myresult)
                {
                    Curcount += 1;  //���� �߰�
                    Tempcount += 1; //���� �߰�
                                    //���� �ð�
                    ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "��";
                    //���� Ƚ��
                    QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;
                    //�г� Ȱ��&��Ȱ��
                    CurResult.SetActive(true);
                    InCurrect.SetActive(false);
                    //����Ʈ
                    for(int i=0;i<CurEffect.Length;i++)
                    {
                        CurEffect[i].SetActive(true);
                        CurEffect[i].GetComponent<ParticleSystem>().Play();
                    }
                    
                }
                //�����϶�
                else
                {
                    InCurcount += 1;  //���� �߰�
                    Tempcount += 1; //���� �߰�
                                    //���� �ð�
                    ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "��";
                    //���� Ƚ��
                    QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;
                    //�г� Ȱ��&��Ȱ��
                    CurResult.SetActive(true);
                    Currect.SetActive(false);
                }

                //��ü ���� Ǯ�� ��!
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
        //���� ��!
        else
        {
            if(!TotalResult.activeSelf)
            {
                EndReset(); //���â ���
            }
        }
    }

    //���۹�ư �Լ�
    public void GameStart()
    {
        Debug.Log("Game Start!");
        if (TotalResult.activeSelf)
            ReStartSetting();
        start = true;
        starttime = true;
    }
    //���� ������Ʈ Ŭ���� ��� ����
    public void MyAnswer(int _num)
    {
        Debug.Log("My choice is " + _num);
        myresult = _num;
        myanswer = true;
    }
    //���� ��ư
    public void GameNext()
    {
        //���� ����
        QReset();
        qing = true;
    }
    //�׸��α� ��ư
    public void GameDone()
    {
        //��ȣ ���� ��Ȱ��ȭ
        calc[calcobjectnum].SetActive(false);
        //�� ���� ��Ȱ��ȭ
        righthand[righthandnum - 1].SetActive(false);
        lefthand[lefthandnum - 1].SetActive(false);

        end = true;
    }


    //���â�� ���� ����
    void EndReset()
    {
        //�г� Ȱ��&��Ȱ��
        Desciption.SetActive(false);
        CurResult.SetActive(false);
        Currect.SetActive(true);
        InCurrect.SetActive(true);
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(false);
        EndEffect.SetActive(true);
        EndEffect.GetComponent<ParticleSystem>().Play();


        TotalResult.SetActive(true);    //���â ���
        GameObject.Find("ccount").GetComponent<Text>().text = Curcount.ToString(); //�����
        GameObject.Find("incount").GetComponent<Text>().text = InCurcount.ToString(); //�����
        GameObject.Find("avrgrtime").GetComponent<Text>().text = Mathf.Round(avrgtime/(Curcount+InCurcount)).ToString(); //��չ����ð�
        GameObject.Find("accuracy").GetComponent<Text>().text = (Curcount * 10) + "%";  //��Ȯ��
        GameObject.Find("qcount").GetComponent<Text>().text = Tempcount + "/" + AllQcount;  //���� Ǯ�� ��
    }

    //���������� ���� ����
    void QReset()
    {
        //�ð� ����
        avrgtime += Mathf.Round(temptime);
        temptime = 0;
        //�г� Ȱ��&��Ȱ��
        CurResult.SetActive(false);
        Currect.SetActive(true);
        InCurrect.SetActive(true);
        //����Ʈ ��Ȱ��
        //����Ʈ
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }

    }

    void SettingReckonObject()
    {
        //���� ��ȣ
        calc = new GameObject[Calc.transform.childCount];
        for (int i = 0; i < Calc.transform.childCount; i++)
        {
            calc[i] = Calc.transform.GetChild(i).gameObject;
        }

        //����
        number = new GameObject[Numbers.transform.childCount];
        for(int j=0;j<Numbers.transform.childCount;j++)
        {
            number[j] = Numbers.transform.GetChild(j).gameObject;
        }

        //������
        righthand = new GameObject[Hand_Right.transform.childCount];
        for (int j = 0; j < Hand_Right.transform.childCount; j++)
        {
            righthand[j] = Hand_Right.transform.GetChild(j).gameObject;
        }

        //�޼�
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

        //Ÿ�̸ӿ�
        temptime = 0;
        avrgtime = 0;

        //���� ǥ�� ��
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //���â ��Ȱ��ȭ
        TotalResult.SetActive(false);

        //����Ʈ ��Ȱ��ȭ
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

        //Ÿ�̸ӿ�
        temptime = 0;
        avrgtime = 0;

        //���� ǥ�� ��
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //ĥ�� �г�
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

        //ĥ�� �г� ��Ȱ��ȭ
        //Desciption.SetActive(false);
        CurResult.SetActive(false);
        TotalResult.SetActive(false);
        //StartButton.SetActive(false);
        //NextButton.SetActive(false);
        //QuitButton.SetActive(false);

        //��Ģ���� ������Ʈ ��Ȱ��ȭ
        for (int i = 0; i < calc.Length; i++)
            calc[i].gameObject.SetActive(false);
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(false);
        for (int i = 0; i < righthand.Length; i++)
            righthand[i].gameObject.SetActive(false);
        for (int i = 0; i < lefthand.Length; i++)
            lefthand[i].gameObject.SetActive(false);

        //����Ʈ ��Ȱ��ȭ
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }
        EndEffect.SetActive(false);
    }

    //0~9���� ��µǴ� ��Ģ���� ���α׷�
    void RandomQMake()
    {
        //int calcnum = 2;
        int randnum = Random.Range(1, 10);   //��ȣ
        int rightnum = Random.Range(1, 5);  //������
        int leftnum = Random.Range(1, 5);   //�޼�
        int calcnum=0;
        int result = 0;                     //�����
        
        if (randnum > 5)
            calcnum = 0;
        else
            calcnum = 1;


        switch (calcnum)
        {
            //���ϱ� �� ��
            case 0:
                //10�̻��� ���� ������ �ȵǹǷ� �������� 5�϶� �޼��� 5 ����
                if (rightnum >= 5)
                {
                    leftnum = Random.Range(1, 4);
                }

                result = (leftnum + rightnum);
                NowQText = "(�޼�)" + leftnum + "+ (������)" + rightnum + "=" + result;
                break;
            //���� �� ��
            case 1:
                //�޼��� ���� �����պ��� ũ�ų� ���ƾ� ��
                if (rightnum > leftnum)
                {
                    leftnum = Random.Range(rightnum, 5);
                }
                result = (leftnum - rightnum);
                NowQText = "(�޼�)" + leftnum + "- (������)" + rightnum + "=" + result;
                break;
            default:
                break;
        }

        //�� �Է�
        calcobjectnum = calcnum;
        lefthandnum = leftnum;
        righthandnum = rightnum;
        resultnum = result;
    }
    //��Ģ ���� ���� ���
    void ShowHandReckon()
    {
        //��Ģ���� ������Ʈ ��Ȱ��ȭ
        for (int i = 0; i < calc.Length; i++)
            calc[i].gameObject.SetActive(false);
        for (int i = 0; i < righthand.Length; i++)
            righthand[i].gameObject.SetActive(false);
        for (int i = 0; i < lefthand.Length; i++)
            lefthand[i].gameObject.SetActive(false);

        //��ȣ ���� ȭ�鿡 ���
        calc[calcobjectnum].SetActive(true);
        //�� ���� ȭ�鿡 ���
        righthand[righthandnum-1].SetActive(true);
        lefthand[lefthandnum-1].SetActive(true);
        //���� ���� ���
        for (int i = 0; i < number.Length; i++)
            number[i].gameObject.SetActive(true);
    }
   
}
