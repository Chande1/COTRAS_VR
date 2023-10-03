using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CookRiceManager : MonoBehaviour
{
    enum CookingRice
    {
        Start = 0,
        WashRice,
        FillWater,
        CloseWater,
        MovePot,
        ClosePot,
        PotWark
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookingRice ing;
    [SerializeField] float staytime;    //���ð�
    [SerializeField] int score;         //����
    [SerializeField] int temperature;   //�µ�

    [Header("�÷��̾�")]
    [SerializeField] GameObject player;
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    [Header("�ð�����")]
    [SerializeField] GameObject TimeWindow;
    [SerializeField] Image clock;   //�ð� �̹���
    [SerializeField] Text time;     //�ð�ǥ��
    [SerializeField] int limit;     //���ѽð�


    [Header("�� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject pot;

    [Header("�߰� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject potonoff;  //������ư
    [SerializeField] GameObject pottop;
    [SerializeField] GameObject water;
    [SerializeField] GameObject rice;
    [SerializeField] GameObject pot2;


    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject waterflowRpos;
    [SerializeField] GameObject waterflowLpos;
    [SerializeField] GameObject potinpos;

    [Header("����Ʈ")]
    [SerializeField] GameObject waterdrop;
    [SerializeField] GameObject circle1_eff;
    [SerializeField] GameObject star_eff;
    [SerializeField] GameObject steam_eff1;

    bool flag;
    bool stay;
    bool ticktok;
    float limit_f;  //���� �ð�


    private void Awake()
    {
        player.transform.position = Startpoint.transform.position;  //���� ��ġ�� �÷��̾� �̵�
        player.transform.eulerAngles = Startpoint.transform.eulerAngles;
        StartSetting();
    }


    void Update()
    {
        switch (ing)
        {
            case CookingRice.Start:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    flag = true;
                }
                else
                {
                    //������ �۵��� ��������
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) || LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        ing = CookingRice.WashRice;
                    }
                }
                break;
            case CookingRice.WashRice:
                if (flag)
                {
                    uimanager.ShowNextUI();
                    flag = false;
                }
                else
                {
                    //������ �۵��� ��������
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) || LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        ing = CookingRice.FillWater;
                    }
                }
                break;
            case CookingRice.FillWater:
                if(!flag)
                {
                    uimanager.ShowNextUI();
                    waterflowLpos.GetComponent<BoxCollider>().enabled = true;
                    waterflowRpos.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && waterdrop.activeInHierarchy)
                    {
                        water.SetActive(true);
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);

                        waterflowLpos.GetComponent<BoxCollider>().enabled = false;
                        waterflowRpos.GetComponent<BoxCollider>().enabled = false;

                        stay = false;
                    }
                }
                break;
            case CookingRice.CloseWater:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    waterflowLpos.GetComponent<BoxCollider>().enabled = true;
                    waterflowRpos.GetComponent<BoxCollider>().enabled = true;
                    waterflowLpos.GetComponent<FingerTouchObject>().ResetSetting();
                    waterflowRpos.GetComponent<FingerTouchObject>().ResetSetting();

                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay && !waterdrop.activeInHierarchy)
                    {
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);

                        waterflowLpos.GetComponent<BoxCollider>().enabled = false;
                        waterflowRpos.GetComponent<BoxCollider>().enabled = false;
                        stay = false;
                    }
                }
                break;
            case CookingRice.MovePot:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    pot.GetComponent<Rigidbody>().isKinematic = false;
                    pot.GetComponent<BoxCollider>().enabled = true;
                    potinpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if(stay&&potinpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        if(GameObject.Find("vr_glove_right_model_slim(Clone)"))
                            GameObject.Find("vr_glove_right_model_slim(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>().skeletonBlend = 1;
                        if (GameObject.Find("vr_glove_left_model_slim(Clone)"))
                            GameObject.Find("vr_glove_left_model_slim(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>().skeletonBlend = 1;
                        pot.SetActive(false);
                        pot2.SetActive(true);

                        potinpos.SetActive(false);
                        
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingRice.ClosePot:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    pottop.GetComponent<BoxCollider>().enabled = true;
                    pottop.GetComponent<OutLineObject>().OutLineOn();  //�ܰ��� Ȱ��
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay && !pottop.GetComponent<OutLineObject>().OutLineWork())
                    {
                        pottop.GetComponent<Animator>().SetBool("pt_close", true);
                        pottop.GetComponent<BoxCollider>().enabled = false;
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingRice.PotWark:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    potonoff.GetComponent<BoxCollider>().enabled = true;
                    potonoff.GetComponent<OutLineObject>().OutLineOn();  //�ܰ��� Ȱ��
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && !potonoff.GetComponent<OutLineObject>().OutLineWork())
                    {
                        steam_eff1.SetActive(true);
                        potonoff.GetComponent<BoxCollider>().enabled = false;
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            default:
                break;
        }

        //�ð� ���� �帣��
        if (ticktok)
        {
            ClockWork();
        }
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
            Debug.Log("�ð��ʰ�!");
        }
    }

    void StayTime()
    {
        Debug.Log("���� �ܰ�:" + (ing + 1));
        ing++;
    }

    void StartSetting()
    {
        flag = false;
        stay = false;
        ticktok = false;
        score = 0;
        temperature = 0;
        limit_f = limit;

        uimanager = GetComponent<SimpleUIManager>();
        //ing = CookEggIng.Start;
        TimeWindow.SetActive(false);
        //egg_oil.SetActive(false);

        //potonoff.SetActive(false);
        pot2.SetActive(false);

        waterdrop.SetActive(false);
        //rice.SetActive(false);
        water.SetActive(false);
        circle1_eff.SetActive(false);
        steam_eff1.SetActive(false);

        waterflowLpos.GetComponent<BoxCollider>().enabled = false;
        waterflowRpos.GetComponent<BoxCollider>().enabled = false;

        pot.GetComponent<Rigidbody>().isKinematic = true;
        pot.GetComponent<BoxCollider>().enabled = false;

        pottop.GetComponent<BoxCollider>().enabled = false;
        potonoff.GetComponent<BoxCollider>().enabled = false;
    }
}
