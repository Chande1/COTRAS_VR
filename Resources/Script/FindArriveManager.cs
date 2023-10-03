using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindArriveManager : MonoBehaviour
{
    [Header("�÷��̾�(���Կ�)")]
    [SerializeField] GameObject Player;         //�÷��̾�
    [SerializeField] GameObject vrcamera;       //ī�޶�
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    [Header("������ �迭(���Կ�)")]
    [SerializeField] GameObject ArrivePos;      //������ �迭 �θ� ������Ʈ
    [SerializeField] GameObject[] APoints;      //������ �迭
    [SerializeField] GameObject StartPos;       //���� ���� ����
    [SerializeField] GameObject EndPos;         //���� �� ����
    [SerializeField] GameObject TempStartPos;        //�ӽ� ������ ��ǥ
    [SerializeField] GameObject[] TempEndPos;
    [Header("������ ������Ʈ(���Կ�)")]
    [SerializeField] GameObject ArriveObject;
    [SerializeField] GameObject arriveobject;
    [Header("���� ī��Ʈ(���Կ�)")]
    [SerializeField] int roundcount;        //������
    [Header("������ ī��Ʈ(���Կ�)")]
    [SerializeField] int apointcount;       //������ ī��Ʈ
    [Header("����/������ ī��Ʈ(Ȯ�ο�)")]
    [SerializeField] int nowrcount;
    [SerializeField] int nowacount;
    [Header("UI(���Կ�)")]
    [SerializeField] GameObject UIBox;
    [SerializeField] GameObject arrow_ui;
    [SerializeField] GameObject count_ui;
    [SerializeField] GameObject ready_ui;
    [SerializeField] GameObject noarrow_ui;
    [SerializeField] GameObject count2_ui;
    [SerializeField] GameObject good_ui;
    [SerializeField] GameObject well_ui;
    [SerializeField] GameObject bad_ui;

    [Header("ȭ��ǥ(���Կ�)")]
    [SerializeField] WayPointArrow arrowscript;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject distance;
    [Header("3Dȭ��ǥ(���Կ�)")]
    [SerializeField] GameObject Arrow;
    [SerializeField] GameObject Distance;
    [Header("3Dȭ��ǥ ����ϱ�")]
    [SerializeField] bool ObjectArrow;

    [Header("�÷��̾� ������ ���� ����(Ȯ�ο�")]
    [SerializeField] bool imarrive;
    [Header("���������� �Ÿ�(Ȯ�ο�)")]
    [SerializeField] float arrivedistance;

    [Header("�Ÿ�����(���Կ�)")]
    [SerializeField] int gooddis;
    [SerializeField] int welldis;
    [SerializeField] int baddis;

    [Header("ȭ��ǥ ǥ��(Ȯ�ο�)")]
    [SerializeField] bool arrownow;

    [Header("�ð�����(���Կ�)")]
    [SerializeField] Text time;     //�ð�ǥ��
    [SerializeField] Text time2;     //�ð�ǥ��
    [SerializeField] Text time3;     //�ð�ǥ��
    [SerializeField] int limit;     //���ѽð�
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


        //���������� ����������
        if(!fail&&!imarrive&&arrivedistance<=1.5f)
        {
            CloseUI();
            UIBox.SetActive(true);
            ready_ui.SetActive(true);
            ing = false;
            imarrive = true;
            Debug.Log("�ð� �ȿ� �̵� �Ϸ�!");
        }
        if(imarrive)
        {
            if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)|| LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                //���� ������ ����
                Destroy(arriveobject);
                CloseUI();

                if (arrownow)
                {
                    nowacount += 1; //���� ������
                    Debug.Log(nowacount + "��° ȭ��ǥ ������");
                    //ȭ��ǥ �ִ� ������ �̵�
                    ArrowArrive();
                    
                }
                else
                {
                    nowacount += 1;  //���� ������
                    Debug.Log(nowacount + "��° ��ȭ��ǥ ������");
                    //ȭ��ǥ ���� ������ �̵�
                    NoArrowResetPoint();
                    
                }

                imarrive = false;
            }
            
        }
    }


    //���� ����
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

        //ui ����
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

    //�ð� �۵�
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Mathf.Round(limit_f).ToString()+"��";
        time2.text = Mathf.Round(limit_f).ToString() + "��";
        time3.text = Mathf.Round(limit_f).ToString() + "��";
        float temp = 1000 / limit;

        //�ð��ʰ� 0�� �Ǹ�
        if (Mathf.Round(limit_f) == 0)
        {
            fail = true;
            ing = false;
        }
    }


    //ȭ��ǥ�� �ִ� ������
    public void ArrowArrive()
    {
        //ó�� ������ ����
        if (nowacount==1)
        {
            RandomArrivePos();          //���� ������ ����
            TempEndPos = new GameObject[apointcount+1];   //������ ������ŭ ������ ������ ����(������� ����)
            TempStartPos = StartPos;    //ù ������ ����
            TempEndPos[0] = StartPos;
            TempEndPos[1] = EndPos;

            //ȭ��ǥ ǥ��
            arrownow = true;
        }
        //���ڸ����� ���� ������ ����
        else if(apointcount>=nowacount)
        {
            OtherArrivePos();                           //���ڸ����� ���� ������ ����
            TempEndPos[nowacount] = EndPos;         //���ο� ������ ������ �����Կ� ����
            Debug.Log(nowacount+"��° ������ ��ǥ ����");
            //ȭ��ǥ ǥ��
            arrownow = true;
        }
        else
        {
            Debug.Log("��� ȭ��ǥ ������ �̵� �Ϸ�!");
            nowacount = 1;  //�ʱ�ȭ
            //ȭ��ǥ ǥ��
            arrownow = false;
            Debug.Log(nowacount + "��° ��ȭ��ǥ ������");
            //ȭ��ǥ ���� ������ �̵�
            NoArrowResetPoint();
        }
        
        if(arrownow)
        {
            SettingArriveObject();  //�÷��̾�� ������ ��ġ�� ����

            //�ð��۵�
            limit_f = limit;
            fail = false;
            ing = true;

            //������ ���� ������Ʈ
            count_ui.GetComponent<Text>().text = nowacount.ToString();
            count2_ui.GetComponent<Text>().text = nowacount.ToString();
            //uiȰ��
            UIBox.SetActive(true);
            arrow_ui.SetActive(true);
            count_ui.SetActive(true);
            //ui��Ȱ��
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

    //ȭ��ǥ ���� �ٽ��ѹ� �������� �̵�
    public void NoArrowResetPoint()
    {
        if(apointcount <nowacount)
        {
            if(roundcount>nowrcount)
            {
                Debug.Log("��� ��ȭ��ǥ ������ �̵� �Ϸ�!");
                Debug.Log(nowrcount+"��° ���� �Ϸ�!");
                nowacount = 1;  //�ʱ�ȭ
                nowrcount += 1; //������ �߰�
                                //ȭ��ǥ ǥ��
                arrownow = true;
                CloseUI();
                ArrowArrive();
            }
            else
            {
                CloseUI();
                Debug.Log("��� ���� �Ϸ�!");
                arrownow = true;
                imarrive = false;
                fail = false;
                ing = false;
                SceneManager.LoadScene("FindArriveAndObject_Start");
            }
        }

        if (!arrownow)
        {
            SettingArriveObject();  //�÷��̾�� ������ ó�� ��ġ�� ����
            EndPos = TempEndPos[nowacount];

            //�ð��۵�
            limit_f = limit;
            fail = false;
            ing = true;

            //������ ���� ������Ʈ
            count_ui.GetComponent<Text>().text = nowacount.ToString();
            count2_ui.GetComponent<Text>().text = nowacount.ToString();
            //uiȰ��
            UIBox.SetActive(true);
            noarrow_ui.SetActive(true);
            count2_ui.SetActive(true);
            //ui��Ȱ��
            Invoke("CloseUI", 2f);

            //ȭ��ǥ ��Ȱ��
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

    [ContextMenu("�������� ������Ʈ ����")]
    private void SettingArrivePoint()
    {
        //�ڽ��� ������
        if(ArrivePos.transform.childCount!=0)
        {
            APoints = new GameObject[ArrivePos.transform.childCount];

            for(int i=0;i<ArrivePos.transform.childCount;i++)
            {
                APoints[i] = ArrivePos.transform.GetChild(i).gameObject;
            }
        }
    }

    [ContextMenu("������ ������� ������ ����")]
    private void RandomArrivePos()
    {
        int start = Random.Range(0, APoints.Length);  //������ ���� ��ǥ ���� ����
        int end = Random.Range(0, APoints.Length);    //������ ���� ��ǥ ���� ����

        //���� ���ڰ� ��ĥ��
        while(start==end)
        {
            end = Random.Range(0, APoints.Length);
        }

        StartPos = APoints[start];
        EndPos = APoints[end];
    }

    //���ڸ����� ���ο� �������� ã��
    private void OtherArrivePos()
    {
        int end = Random.Range(0, APoints.Length);    //������ ���� ��ǥ ���� ����
        EndPos = APoints[end];

        //���� ���ڰ� ��ĥ��
        while (TempEndPos[nowacount-1]==EndPos)
        {
            end = Random.Range(0, APoints.Length);    //������ ���� ��ǥ ���� ����
            EndPos = APoints[end];
        }

    }


    //�÷��̾� ��ġ�� ������ ������ ����
    void SettingArriveObject()
    {
        //�÷��̾�
        Player.transform.position = TempEndPos[nowacount-1].transform.position;  //���� ��ġ�� �÷��̾� �̵�
                                                                  //Player.transform.eulerAngles = StartPos.transform.eulerAngles;

        //������ ������Ʈ
        arriveobject = GameObject.Instantiate(ArriveObject, TempEndPos[nowacount].transform.position, ArriveObject.transform.rotation);
        arriveobject.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���

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
