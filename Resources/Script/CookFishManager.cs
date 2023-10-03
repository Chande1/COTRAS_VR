using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
public class CookFishManager : MonoBehaviour
{
    enum CookingFish
    {
        Start = 0,
        OilIn,
        OnFire,
        TempUp,
        MoveFish,
        SaltIn,
        Stay,
        OffFire,
        MoveFried
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookingFish ing;
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
    [SerializeField] GameObject fryingfan;  //�Ķ�����
    [SerializeField] GameObject turner;     //��¤��
    [SerializeField] GameObject oil;        //�⸧
    [SerializeField] GameObject salt;        //�ұ�

    [Header("�߰� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject fanfire;
    [SerializeField] GameObject egg_oil;    //�Ķ����� �⸧
    [SerializeField] GameObject OnOff_btn;  //������ư
    [SerializeField] GameObject OnOff_txt;  //��������
    [SerializeField] GameObject NumMinus_btn;   //���ҹ�ư
    [SerializeField] GameObject NumPlus_btn;    //������ư
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;
    [SerializeField] GameObject[] fish;

    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject oilinpos;
    [SerializeField] GameObject saltinpos;
    [SerializeField] GameObject fishpos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject midpos;

    [Header("����Ʈ")]
    [SerializeField] GameObject circle1_eff;    //ū�� ȿ��
    [SerializeField] GameObject star_eff;       //�� ȿ��
    [SerializeField] GameObject steam_eff1;
    [SerializeField] GameObject steam_eff2;


    bool flag;
    bool stay;
    bool ticktok;
    bool eggdone;

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
            case CookingFish.Start:
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
                        ing = CookingFish.OilIn;
                    }
                }
                break;
            case CookingFish.OilIn:
                if(flag)
                {
                    uimanager.ShowNextUI();
                    oilinpos.SetActive(true);
                    oil.GetComponent<Rigidbody>().isKinematic = false ;
                    oil.GetComponent<BoxCollider>().enabled = true;

                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay && oilinpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        oilinpos.SetActive(false);
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingFish.OnFire:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    //�ִϸ��̼��� Ȱ��ȭ �Ǿ�������
                    if (stay && OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if (OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //�ִϸ��̼� ��Ȱ��ȭ
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            OnOff_btn.GetComponent<BoxCollider>().enabled = false;
                            //OnOff_btn.GetComponent<FingerTouchObject>().enabled = false;
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                break;
            case CookingFish.TempUp:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    NumMinus_btn.SetActive(true);
                    NumPlus_btn.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay)
                    {
                        //���̳ʽ� ��ư ��������
                        if (NumMinus_btn.GetComponent<FingerTouchObject>().GetNowAni())
                        {
                            if (NumMinus_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                            {
                                Debug.Log("���̳ʽ�!");
                                if (temperature > 0)
                                    temperature -= 1;

                                ShowInductionNum();
                                //�ִϸ��̼� ��Ȱ��ȭ
                                NumMinus_btn.GetComponent<FingerTouchObject>().OffAni();
                            }
                        }
                        //�÷��� ��ư ��������
                        if (NumPlus_btn.GetComponent<FingerTouchObject>().GetNowAni())
                        {
                            if (NumPlus_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                            {
                                Debug.Log("�÷���!");
                                if (temperature < 5)
                                    temperature += 1;

                                ShowInductionNum();
                                //�ִϸ��̼� ��Ȱ��ȭ
                                NumPlus_btn.GetComponent<FingerTouchObject>().OffAni();
                            }
                        }
                        if (temperature >= 5)
                        {
                            //������Ʈ ��Ȱ��ȭ
                            NumMinus_btn.GetComponent<BoxCollider>().enabled = false;
                            NumPlus_btn.GetComponent<BoxCollider>().enabled = false;
                            //UI
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingFish.MoveFish:
                if(!flag)
                {
                    uimanager.ShowNowUI();

                    for(int i=0;i<fish.Length;i++)
                    {
                        fish[i].GetComponent<Rigidbody>().isKinematic = false;
                        fish[i].GetComponent<BoxCollider>().enabled = true;
                    }
                    fishpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && fishpos.GetComponent<GoalInfoObject>().GetCount() >= 2)
                    {
                        //UI
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);

                        fishpos.SetActive(false);
                        stay = false;
                    }
                }
                break;
            case CookingFish.SaltIn:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    saltinpos.SetActive(true);

                    salt.GetComponent<Rigidbody>().isKinematic = false;
                    salt.GetComponent<BoxCollider>().enabled = true;

                    stay = true;
                    flag = false ;
                }
                else
                {
                    if (stay && saltinpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        saltinpos.SetActive(false);
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingFish.Stay:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    //�ð����� Ȱ��ȭ
                    TimeWindow.SetActive(true);

                    for (int i = 0; i < fish.Length; i++)
                    {
                        fish[i].GetComponent<Animator>().SetBool("F_C", true);
                    }

                    ticktok = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    ClockWork();    //�ð���

                    if (stay)
                    {
                        if (!steam_eff1.activeInHierarchy && Mathf.Round(limit_f) <= 8)
                        {
                            steam_eff1.SetActive(true);
                        }
                        else if (!steam_eff2.activeInHierarchy && Mathf.Round(limit_f) <= 5)
                        {
                            steam_eff2.SetActive(true);
                        }

                        if (Mathf.Round(limit_f) <= 0)
                        {
                            ticktok = false;
                            TimeWindow.SetActive(false);
                            //UI
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingFish.OffFire:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
                    OnOff_btn.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    flag = false;
                }
                else
                {
                    //�ִϸ��̼��� Ȱ��ȭ �Ǿ�������
                    if (stay && OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if (OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //�δ��� ����
                            if (Num_1.activeSelf)
                                Num_1.SetActive(false);
                            else if (Num_2.activeSelf)
                                Num_2.SetActive(false);
                            else if (Num_3.activeSelf)
                                Num_3.SetActive(false);
                            else if (Num_4.activeSelf)
                                Num_4.SetActive(false);
                            else if (Num_5.activeSelf)
                                Num_5.SetActive(false);
                            //�ִϸ��̼� ��Ȱ��ȭ
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                break;
            case CookingFish.MoveFried:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    circle1_eff.SetActive(true);
                    friedpos.SetActive(true);
                    midpos.SetActive(true);

                    turner.GetComponent<Rigidbody>().isKinematic = false;
                    turner.GetComponent<BoxCollider>().enabled = true;

                    stay = true;
                    flag = true;
                }
                else
                {
                    ClockWork();    //�ð���

                    if (stay && friedpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("���ÿ� ����!");
                        midpos.GetComponent<AttachObject>().DetachObjects(friedpos);
                        friedpos.GetComponent<GoalInfoObject>().ResetGoalInfoObject();
                    }
                    else if (stay)
                    {
                        if (friedpos.GetComponent<GoalInfoObject>().GetCount() >= 2)
                        {
                            midpos.SetActive(false);
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Debug.Log("Ʈ���� �۵�");
                            circle1_eff.SetActive(false);
                            friedpos.SetActive(false);
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
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


    void ShowInductionNum()
    {
        if (Num_1.activeSelf)
            Num_1.SetActive(false);
        if (Num_2.activeSelf)
            Num_2.SetActive(false);
        if (Num_3.activeSelf)
            Num_3.SetActive(false);
        if (Num_4.activeSelf)
            Num_4.SetActive(false);
        if (Num_5.activeSelf)
            Num_5.SetActive(false);


        switch (temperature)
        {
            case 1:
                Num_1.SetActive(true);
                break;
            case 2:
                Num_2.SetActive(true);
                break;
            case 3:
                Num_3.SetActive(true);
                break;
            case 4:
                Num_4.SetActive(true);
                break;
            case 5:
                Num_5.SetActive(true);
                break;
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
        eggdone = false;
        score = 0;
        temperature = 0;
        limit_f = limit;

        uimanager = GetComponent<SimpleUIManager>();
        //ing = CookEggIng.Start;
        TimeWindow.SetActive(false);
        //egg_oil.SetActive(false);

        OnOff_btn.SetActive(false);
        OnOff_txt.SetActive(false);
        NumMinus_btn.SetActive(false);
        NumPlus_btn.SetActive(false);
        Num_1.SetActive(false);
        Num_2.SetActive(false);
        Num_3.SetActive(false);
        Num_4.SetActive(false);
        Num_5.SetActive(false);

        for (int i = 0; i < fish.Length; i++)
        {
            fish[i].GetComponent<Rigidbody>().isKinematic = true;
            fish[i].GetComponent<BoxCollider>().enabled = false;
        }

        oilinpos.SetActive(false);
        saltinpos.SetActive(false);
        friedpos.SetActive(false);
        midpos.SetActive(false);
        fishpos.SetActive(false);

        circle1_eff.SetActive(false);
        steam_eff1.SetActive(false);
        steam_eff2.SetActive(false);

        oil.GetComponent<Rigidbody>().isKinematic = true;
        oil.GetComponent<BoxCollider>().enabled = false;
        salt.GetComponent<Rigidbody>().isKinematic = true;
        salt.GetComponent<BoxCollider>().enabled = false;
        turner.GetComponent<Rigidbody>().isKinematic = true;
        turner.GetComponent<BoxCollider>().enabled = false;
       
    }
}
