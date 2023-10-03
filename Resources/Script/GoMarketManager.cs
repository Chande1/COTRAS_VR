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

    [Header("�����Ȳ")]
    [SerializeField] GMProgress ing;
    [Header("�÷��̾�")]
    [SerializeField] GameObject Player;
    [Header("������")]
    [SerializeField] GameObject vrPointer;
    [Header("ĵ����")]
    [SerializeField] GameObject StartWindow;    //����ȭ��
    [SerializeField] GameObject GameWindow;     //����ȭ��
    [SerializeField] GameObject MainPnl;        //���ӽ���ȭ��
    [SerializeField] GameObject CurResultPnl;   //�߰����â
    [SerializeField] GameObject TotalResultPnl; //�������â
    [SerializeField] GameObject Currect;
    [SerializeField] GameObject InCurrect;
    [SerializeField] GameObject ReactTime;      //r_time
    [SerializeField] GameObject QuestionCount;   //qcount
    [Header("��ư")]
    [SerializeField] GameObject Answer;
    [Header("���� ������ ������Ʈ")]
    [SerializeField] GameObject[] Items;
    [SerializeField] GameObject[] Shows;
    [Header("���� ������Ʈ (���Կ�)")]
    [SerializeField] GameObject[] QuestionItem;
    [Header("���� ������ ���� (���Կ�)")]
    [SerializeField] int ShowItemCount;
    [Header("�̵���ǥ")]
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject EndPos;
    [Header("�ð�����")]
    [SerializeField] Image clock;   //�ð� �̹���
    [SerializeField] Text time;     //�ð�ǥ��
    [SerializeField] int limit;     //���ѽð�
    [Header("���� ���߱� �ý���")]
    [SerializeField] int AllQcount; //��� ���� ��
    [SerializeField] int Curcount;  //���� ��
    [SerializeField] int InCurcount;//���� ��
    [SerializeField] int Tempcount; //���� ����
    [Header("������ ������Ʈ")]
    [SerializeField] GameObject QuestionObject;
    [Space(10)]
    [Header("����Ʈ (���Կ�)")]
    [SerializeField] GameObject[] CurEffect;  //���� ����Ʈ
    [SerializeField] GameObject EndEffect;  //�� ����Ʈ


    float limit_f;  //���� �ð�
    bool myanswer;  //�� ���� ����
    bool myresult;  //�� ����
    bool currectanswer; //����

    bool makeq;     //���� ����

    float temptime;  //�����ð� ����
    float avrgtime; //��չ����ð� ����
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
                    //�÷��̾� �̵� ����
                    Player.GetComponent<PlayerController>().enabled = true;
                    //GameObject.Find("WarkBox").SetActive(true);
                    vrPointer.SetActive(false);  //������ Ȱ��ȭ
                    StartWindow.SetActive(false);
                    ing = GMProgress.Moving;
                }
                break;
            case GMProgress.Moving:
                ClockWork();
                break;
            case GMProgress.MovingEnd:
                //�÷��̾� ��ġ ����
                Player.transform.position = EndPos.transform.position;
                Player.transform.eulerAngles = EndPos.transform.eulerAngles;
                //�÷��̾� �̵� ����
                Player.GetComponent<PlayerController>().enabled = false;
                GameObject.Find("WarkBox").SetActive(false);
                GameObject.Find("TimeWindow").SetActive(false); //�ո� Ÿ�̸� ��Ȱ��
                GameWindow.SetActive(true);
                vrPointer.SetActive(true);  //������ Ȱ��ȭ
                starttime = true;
                makeq = true;

                ing = GMProgress.GameStart;
                break;
            case GMProgress.GameStart:
                
                break;
            case GMProgress.Gaming:

                if(makeq)
                {
                    //���� ����
                    MakeQuestion();
                    myanswer = false;
                    makeq = false;
                }

                //���� �����ϸ�!
                if(myanswer)
                {
                    MainPnl.SetActive(false);

                    QuestionResult();
                    Destroy(QuestionObject.transform.GetChild(0).gameObject);   //�ڽ����� �����Ǿ� �ִ� ���� ������Ʈ ����
                    
                    //�����϶�
                    if(myresult==currectanswer)
                    {
                        Curcount += 1;  //���� �߰�
                        Tempcount += 1; //���� �߰�
                                        //���� �ð�
                        ReactTime.GetComponent<Text>().text = Mathf.Round(temptime).ToString() + "��";
                        //���� Ƚ��
                        QuestionCount.GetComponent<Text>().text = Tempcount + "/" + AllQcount;

                        InCurrect.SetActive(false);
                        CurResultPnl.SetActive(true);

                        //����Ʈ
                        for (int i = 0; i < CurEffect.Length; i++)
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

                        Currect.SetActive(false);
                        CurResultPnl.SetActive(true);
                    }
                    //��ü ���� Ǯ�� ��!
                    if (Tempcount == AllQcount)
                    {
                        ing = GMProgress.GameEnd;
                    }
                    myanswer = false;
                }
                //���� ���߱� ��!
                else if (starttime && !myanswer && !CurResultPnl.activeSelf)
                {
                    temptime += Time.deltaTime;
                }

                break;
            case GMProgress.GameEnd:
                //�г� Ȱ��&��Ȱ��
                MainPnl.SetActive(false);
                CurResultPnl.SetActive(false);
                Currect.SetActive(true);
                InCurrect.SetActive(true);
                Answer.SetActive(false);

                if(QuestionObject.transform.childCount!=0)
                    Destroy(QuestionObject.transform.GetChild(0).gameObject);   //�ڽ����� �����Ǿ� �ִ� ���� ������Ʈ ����

                EndEffect.SetActive(true);
                EndEffect.GetComponent<ParticleSystem>().Play();


                TotalResultPnl.SetActive(true);    //���â ���
                GameObject.Find("ccount").GetComponent<Text>().text = Curcount.ToString(); //�����
                GameObject.Find("incount").GetComponent<Text>().text = InCurcount.ToString(); //�����
                GameObject.Find("avrgrtime").GetComponent<Text>().text = Mathf.Round(avrgtime / (Curcount + InCurcount)).ToString(); //��չ����ð�
                GameObject.Find("accuracy").GetComponent<Text>().text = (Curcount * 10) + "%";  //��Ȯ��
                GameObject.Find("qcount").GetComponent<Text>().text = Tempcount + "/" + AllQcount;  //���� Ǯ�� ��
                break;
        }
    }

    public void StartWind()
    {
        start = true;
    }

    //���۹�ư �Լ�
    public void GameStart()
    {
        Debug.Log("Game Start!");
        if (TotalResultPnl.activeSelf)
            ReStartSetting();
        Answer.SetActive(true); //���� ��ư Ȱ��ȭ
        ing = GMProgress.Gaming;
    }
    //���� ��ư
    public void GameNext()
    {
        //���� ����
        QReset();
        //���� ����
        makeq = true;
    }
    //�׸��α� ��ư
    public void GameDone()
    {
        ing = GMProgress.GameEnd;
    }

    //���� ������ ����
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

        //���� ���� Ÿ�̸�
        limit_f = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;

        //���� ������ ��� ����
        myanswer = false;
        myresult = false;
        currectanswer = false;

        //���� �ð� 
        temptime = 0;  //�����ð� ����
        avrgtime = 0; //��չ����ð� ����
        starttime = false;

        //���� ����
        makeq = false;

        //���� ǥ�� ��
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        //���â ��Ȱ��ȭ
        TotalResultPnl.SetActive(false);

        //�÷��̾� ��ġ ����
        Player.transform.position = StartPos.transform.position;
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        //������ ��Ȱ��ȭ
        vrPointer.SetActive(false);
        Player.GetComponent<PlayerController>().enabled = true;
        GameObject.Find("WarkBox").SetActive(true);
        GameObject.Find("TimeWindow").SetActive(true); //�ո� Ÿ�̸� Ȱ��
        GameWindow.SetActive(false);
        StartWindow.SetActive(true);

        //����Ʈ ��Ȱ��
        //����Ʈ
        EndEffect.SetActive(false);
        */
    }

    void QReset()
    {
        //�ð� ����
        avrgtime += Mathf.Round(temptime);
        temptime = 0;

        myanswer = false;           //���� �ʱ�ȭ

        //������Ʈ Ȱ��/��Ȱ��ȭ
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
            Debug.Log("������ ���� ������Ʈ�� �����ϴ�.");
        }
    }

    //���� ������Ʈ ����
    void MakeQuestion()
    {
        int randnum = Random.Range(0, QuestionItem.Length);

        GameObject tempitem=Instantiate(QuestionItem[randnum], QuestionObject.transform.position, Quaternion.identity);
        tempitem.transform.SetParent(QuestionObject.transform);
        tempitem.name = tempitem.name.Replace("(Clone)", "");

        Debug.Log("���� ���� �Ϸ�!");
    }


    //�ð� �۵�
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Mathf.Round(limit_f).ToString();
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //�ð��ʰ� 0�� �Ǹ�
        if (Mathf.Round(limit_f) == 0)
        {
            ing = GMProgress.MovingEnd;
        }
    }

    //���� ������ Ȱ��ȭ
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
                    Debug.Log(i + "��° ��ϵ� ������ : " + Items[randnum].name);
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

    //��� ������ ��Ȱ��ȭ
    void HideAllItem()
    {
        for(int i=0;i<Items.Length;i++)
        {
            Items[i].SetActive(false);
        }
    }

    void StartSetting()
    {
        //���� ���� Ÿ�̸�
        limit_f = limit;
        time.text = limit.ToString();
        clock.fillAmount = 0;

        //���� ������ ��� ����
        myanswer = false;
        myresult = false;
        currectanswer = false;

        //���� �ð� 
        temptime=0;  //�����ð� ����
        avrgtime=0; //��չ����ð� ����
        starttime = false ;

        //���� ����
        makeq = false;

        //���� ǥ�� ��
        Curcount = 0;
        InCurcount = 0;
        Tempcount = 0;

        start = false;

        Items = GameObject.FindGameObjectsWithTag("Item");  //�ش� �±� ������Ʈ ����

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

        //������Ʈ ��Ȱ��ȭ
        CurResultPnl.SetActive(false);
        TotalResultPnl.SetActive(false);
        GameWindow.SetActive(false);
        Answer.SetActive(false);

        //�÷��̾� ��ġ ����
        Player.transform.position = StartPos.transform.position;
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        //�÷��̾� �̵� ����
        Player.GetComponent<PlayerController>().enabled = false;
        //GameObject.Find("WarkBox").SetActive(false);
        //������ ��Ȱ��ȭ
        //vrPointer.SetActive(false);
        //����Ʈ ��Ȱ��
        //����Ʈ
        EndEffect.SetActive(false);
        for (int i = 0; i < CurEffect.Length; i++)
        {
            CurEffect[i].SetActive(false);
        }
    }
}
