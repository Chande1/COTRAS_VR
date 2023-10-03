using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CookPumkinManager : MonoBehaviour
{
    enum CookingPumkin
    { 
        Start = 0,
        CutPumkin,
        EggInfo,
        BrokenEgg,
        MixEgg,
        EggInPumkin,
        OnFire,
        PlusTemp,
        MovePumkin,
        Stay,
        MoveFried
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookingPumkin ing;
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
    [SerializeField] GameObject spoon;
    //[SerializeField] GameObject oil;
    [SerializeField] GameObject paddle;
    [SerializeField] GameObject knife;
    [SerializeField] GameObject[] eggs;
    [SerializeField] GameObject[] pumkins;
    [SerializeField] GameObject[] cutlines;

    [Header("�߰� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject OnOff_btn;  //������ư
    [SerializeField] GameObject OnOff_txt;  //��������
    [SerializeField] GameObject NumMinus_btn;   //���ҹ�ư
    [SerializeField] GameObject NumPlus_btn;    //������ư
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;

    [SerializeField] GameObject lastpumkin1;
    [SerializeField] GameObject lastpumkin2;

    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject Startpoint2;
    [SerializeField] GameObject holoeggpos;
    [SerializeField] GameObject eggsinpos;
    [SerializeField] GameObject eggpumkinpos;
    //[SerializeField] GameObject oilinpos;
    [SerializeField] GameObject pumkininpos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject paddlemid;

    [Header("����Ʈ")]
    [SerializeField] GameObject circle1_eff;
    [SerializeField] GameObject circle2_eff;
    [SerializeField] GameObject steam_eff1;
    [SerializeField] GameObject steam_eff2;

    bool flag;
    bool stay;
    bool ticktok;
    bool eggdone;

    float limit_f;  //���� �ð�
    int tempcount = 0;

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
            case CookingPumkin.Start:
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
                        ing = CookingPumkin.CutPumkin;
                    }
                }
                break;
            case CookingPumkin.CutPumkin:
                if (flag)
                {
                    uimanager.ShowNextUI();

                    knife.GetComponent<Rigidbody>().isKinematic = false;
                    knife.GetComponent<BoxCollider>().enabled = true;

                    cutlines[0].SetActive(true);


                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay)
                    {
                        if (tempcount < cutlines.Length)
                        {
                            if (cutlines[tempcount].GetComponent<CutLine>().GetCutting())
                            {
                                if(tempcount==0)
                                {
                                    lastpumkin1.SetActive(false);
                                    lastpumkin2.SetActive(true);
                                }
                                else
                                {
                                    pumkins[tempcount-1].GetComponent<Rigidbody>().isKinematic = false;
                                    pumkins[tempcount-1].GetComponent<BoxCollider>().enabled = true;
                                }
                                cutlines[tempcount].SetActive(false);

                                if (tempcount + 1 < cutlines.Length)
                                    cutlines[tempcount + 1].SetActive(true);
                                tempcount += 1;
                            }
                        }

                        if (tempcount >= cutlines.Length)
                        {
                            pumkins[tempcount-2].GetComponent<Rigidbody>().isKinematic = false;
                            pumkins[tempcount-2].GetComponent<BoxCollider>().enabled = true;
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            gameObject.GetComponent<SimpleFadeMove>().CheckButton();
                            stay = false;
                        }
                    }
                }
                break;
            case CookingPumkin.EggInfo:
                if (!flag)
                {
                    if (gameObject.GetComponent<SimpleFadeMove>().MoveDone())
                    {
                        uimanager.ShowNowUI();
                        flag = true;
                    }

                }
                else
                {
                    //������ �۵��� ��������
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) || LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        ing = CookingPumkin.BrokenEgg;
                    }
                }
                break;
            case CookingPumkin.BrokenEgg:
                if (flag)
                {
                    uimanager.ShowNextUI();
                    for (int i = 0; i < eggs.Length; i++)
                    {
                        eggs[i].GetComponent<Rigidbody>().isKinematic = false;
                        eggs[i].GetComponent<BoxCollider>().enabled = true;
                    }
                    holoeggpos.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay)
                    {
                        if (holoeggpos.GetComponent<GoalInfoObject>().GetTouch())
                        {
                            eggsinpos.SetActive(true);
                        }

                        if (eggsinpos.GetComponent<MixBowl>().GetBrokenDone())
                        {
                            holoeggpos.SetActive(false);
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingPumkin.MixEgg:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    spoon.GetComponent<Rigidbody>().isKinematic = false;
                    spoon.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && eggsinpos.GetComponent<MixBowl>().GetMixDone())
                    {
                        eggsinpos.GetComponent<BoxCollider>().enabled = false;
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingPumkin.EggInPumkin:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    eggpumkinpos.SetActive(true);

                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay && eggpumkinpos.GetComponent<GoalInfoObject>().GetCount() >= 4)
                    {
                        eggpumkinpos.SetActive(false);
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingPumkin.OnFire:
                if (!flag)
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
            case CookingPumkin.PlusTemp:
                if (flag)
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
            case CookingPumkin.MovePumkin:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    pumkininpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && pumkininpos.GetComponent<GoalInfoObject>().GetCount() >= 4)
                    {
                        pumkininpos.SetActive(false);
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingPumkin.Stay:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    ticktok = true;
                    TimeWindow.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    ClockWork();    //�ð���

                    if (stay)
                    {
                        if (!steam_eff1.activeInHierarchy && Mathf.Round(limit_f) <= 8)
                        {
                            for (int i = 0; i < pumkins.Length; i++)
                            {
                                pumkins[i].GetComponent<Animator>().SetBool("P_C2", true);
                            }
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
                            uimanager.ShowResultUI(0);
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingPumkin.MoveFried:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    circle1_eff.SetActive(true);
                    friedpos.SetActive(true);
                    paddle.GetComponent<Rigidbody>().isKinematic = false;
                    paddle.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && friedpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("���ÿ� ����!");
                        paddlemid.GetComponent<AttachObject>().DetachObjects(friedpos);
                        friedpos.GetComponent<GoalInfoObject>().ResetGoalInfoObject();
                    }
                    else if (stay)
                    {
                        if (friedpos.GetComponent<GoalInfoObject>().GetCount() >= 4)
                        {
                            paddlemid.SetActive(false);
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

        lastpumkin2.SetActive(false);

        holoeggpos.SetActive(false);
        eggsinpos.SetActive(false);
        eggpumkinpos.SetActive(false);
        //oilinpos.SetActive(false);
        friedpos.SetActive(false);
        pumkininpos.SetActive(false);

        circle1_eff.SetActive(false);
        circle2_eff.SetActive(false);
        steam_eff1.SetActive(false);
        steam_eff2.SetActive(false);

        //oil.GetComponent<BoxCollider>().enabled = false;
        spoon.GetComponent<BoxCollider>().enabled = false;
        paddle.GetComponent<BoxCollider>().enabled = false;
        knife.GetComponent<BoxCollider>().enabled = false;

        //oil.GetComponent<Rigidbody>().isKinematic = true;
        spoon.GetComponent<Rigidbody>().isKinematic = true;
        paddle.GetComponent<Rigidbody>().isKinematic = true;
        knife.GetComponent<Rigidbody>().isKinematic = true;

        for (int i = 0; i < eggs.Length; i++)
        {
            eggs[i].GetComponent<Rigidbody>().isKinematic = true;
            eggs[i].GetComponent<BoxCollider>().enabled = false;
        }

        for (int j = 0; j < pumkins.Length; j++)
        {
            pumkins[j].GetComponent<Rigidbody>().isKinematic = true;
            pumkins[j].GetComponent<BoxCollider>().enabled = false;
        }

        for (int k = 0; k < cutlines.Length; k++)
        {
            cutlines[k].SetActive(false);
        }
    }
}