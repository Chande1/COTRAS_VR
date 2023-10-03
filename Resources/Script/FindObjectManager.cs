using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FindObjectManager : MonoBehaviour
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
    [SerializeField] GameObject TempPos;        //�ӽ� ������ ��ǥ
    [Header("������ ������Ʈ(���Կ�)")]
    [SerializeField] GameObject ArriveObject;
    [SerializeField] GameObject arriveobject;
    [Header("���帶ũ ī��Ʈ(���Կ�)")]
    [SerializeField] int landcount;        //������
    [Header("���� ī��Ʈ(���Կ�)")]
    [SerializeField] int qcount;       //������ ī��Ʈ
    [Header("����/������ ī��Ʈ(Ȯ�ο�)")]
    [SerializeField] int nowlcount;
    [SerializeField] int nowqcount;
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

    [Header("���帶ũ ������Ʈ(���Կ�)")]
    [SerializeField] GameObject LandMark;    //���帶ũ �θ�
    [SerializeField] GameObject[] LMarks;    //���帶ũ �迭


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

        if (ing)
        {
            ClockWork();
        }
        else if (fail)
        {
            if (arrivedistance > baddis)
            {
                CloseUI();
                UIBox.SetActive(true);
                bad_ui.SetActive(true);
                imarrive = true;
            }
            else if (arrivedistance < baddis && arrivedistance > welldis)
            {
                CloseUI();
                UIBox.SetActive(true);
                well_ui.SetActive(true);
                imarrive = true;
            }
            else if (arrivedistance < welldis)
            {
                CloseUI();
                UIBox.SetActive(true);
                good_ui.SetActive(true);
                imarrive = true;
            }
        }


        //���������� ����������
        if (!fail && !imarrive && arrivedistance <= 1.5f)
        {
            CloseUI();
            UIBox.SetActive(true);
            ready_ui.SetActive(true);
            ing = false;
            imarrive = true;
            Debug.Log("�ð� �ȿ� �̵� �Ϸ�!");
        }
        if (imarrive)
        {
            if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) || LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                //���� ������ ����
                Destroy(arriveobject);
                CloseUI();

                if (arrownow)
                {
                    nowlcount += 1; //���� ������
                    Debug.Log(nowlcount + "��° ȭ��ǥ ������");
                    //ȭ��ǥ �ִ� ������ �̵�
                    ArrowArrive();

                }
                else
                {
                    Debug.Log(nowlcount + "��° ��ȭ��ǥ ������");
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

        if (GameObject.Find("stageinfo"))
        {
            qcount = StageInfo.stageinfo.findAandOArriveCount;
            landcount = StageInfo.stageinfo.findAandORoundCount;
        }

        //���帶ũ Ȱ��&��Ȱ��
        for (int i = 0; i < LMarks.Length; i++)
        {
            if(i<landcount)
            {
                LMarks[i].SetActive(true);
            }
            else
            {
                LMarks[i].SetActive(false);
            }
        }

        nowlcount = 1;
        nowqcount = 1;
        count_ui.GetComponent<Text>().text = nowlcount.ToString();
        count2_ui.GetComponent<Text>().text = nowlcount.ToString();


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
        time.text = Mathf.Round(limit_f).ToString() + "��";
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
        if(qcount>=nowlcount)
        {
            RandomArrivePos();          //���� ������ ����
            arrownow = true;            //ȭ��ǥ ǥ��
            Debug.Log(nowlcount + "��° ȭ��ǥ ������ ��ǥ ����");
        }
        else
        {
            CloseUI();
            Debug.Log("��� ���� �Ϸ�!");
            arrownow = false;
            imarrive = false;
            fail = false;
            ing = false;
            SceneManager.LoadScene("FindArriveAndObject_Start");
        }

        if (arrownow)
        {
            SettingArriveObject();  //�÷��̾�� ������ ��ġ�� ����

            //�ð��۵�
            limit_f = limit;
            fail = false;
            ing = true;

            //������ ���� ������Ʈ
            count_ui.GetComponent<Text>().text = nowlcount.ToString();
            count2_ui.GetComponent<Text>().text = nowlcount.ToString();
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

            arrownow = false;       //��ȭ��ǥ�� ����
        }
    }

    //ȭ��ǥ ���� �ٽ��ѹ� �������� �̵�
    public void NoArrowResetPoint()
    {
        OtherStartPos();             //���� �������� �ٸ� ����� ����
        Debug.Log(nowlcount + "��° ��Ȱ��ǥ ������ ��ǥ ����");

        if (!arrownow)
        {
            SettingArriveObject();  //�÷��̾�� ������ ó�� ��ġ�� ����

            //�ð��۵�
            limit_f = limit;
            fail = false;
            ing = true;

            //������ ���� ������Ʈ
            count_ui.GetComponent<Text>().text = nowlcount.ToString();
            count2_ui.GetComponent<Text>().text = nowlcount.ToString();
            //uiȰ��
            UIBox.SetActive(true);
            noarrow_ui.SetActive(true);
            count2_ui.SetActive(true);
            //ui��Ȱ��
            Invoke("CloseUI", 2f);

           
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

            //ȭ��ǥ Ȱ��
            arrownow = true;
        }
    }


    [ContextMenu("���帶ũ ������Ʈ ����")]
    private void SettingLandMark()
    {
        //�ڽ��� ������
        if (LandMark.transform.childCount != 0)
        {
            LMarks = new GameObject[LandMark.transform.childCount];

            for (int i = 0; i < LandMark.transform.childCount; i++)
            {
                LMarks[i] = LandMark.transform.GetChild(i).gameObject;
            }
        }
    }



    [ContextMenu("�������� ������Ʈ ����")]
    private void SettingArrivePoint()
    {
        //�ڽ��� ������
        if (ArrivePos.transform.childCount != 0)
        {
            APoints = new GameObject[ArrivePos.transform.childCount];

            for (int i = 0; i < ArrivePos.transform.childCount; i++)
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
        while (start == end)
        {
            end = Random.Range(0, APoints.Length);
        }

        StartPos = APoints[start];
        EndPos = APoints[end];
    }

    //���ο� ������� ã��(�������� �״��)
    private void OtherStartPos()
    {
        int start = Random.Range(0, APoints.Length);    //������ ���� ��ǥ ���� ����
        StartPos = APoints[start];

        //���� ���ڰ� ��ĥ��
        while (StartPos==EndPos)
        {
            start = Random.Range(0, APoints.Length);    //������ ���� ��ǥ ���� ����
            StartPos = APoints[start];
        }

    }


    //�÷��̾� ��ġ�� ������ ������ ����
    void SettingArriveObject()
    {
        //�÷��̾�
        Player.transform.position = StartPos.transform.position;  //���� ��ġ�� �÷��̾� �̵�
        //Player.transform.eulerAngles = StartPos.transform.eulerAngles;

        //������ ������Ʈ
        arriveobject = GameObject.Instantiate(ArriveObject, EndPos.transform.position, ArriveObject.transform.rotation);
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
