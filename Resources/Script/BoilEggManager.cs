using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class BoilEggManager : MonoBehaviour
{
    enum BoilingEgg
    {
        Start = 0,
        PotGrip,
        FillEgg,
        FillWater,
        MovePot,
        OnFire,
        SaltIn,
        TempUp,
        Stay,
        OffFire
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] BoilingEgg ing;
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
    [SerializeField] GameObject salt;
    [SerializeField] GameObject[] eggs;

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

    [SerializeField] GameObject waterflow;
   

    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject Startpoint2;
    [SerializeField] GameObject watermidpos;
    [SerializeField] GameObject eggsinpos;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject saltinpos;

    [Header("����Ʈ")]
    [SerializeField] GameObject water;
    [SerializeField] GameObject circle1_eff;    
    [SerializeField] GameObject circle2_eff;    
    [SerializeField] GameObject star_eff;       
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
            case BoilingEgg.Start:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    flag = true;
                }
                else
                {
                    //������ �۵��� ��������
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)|| LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        ing = BoilingEgg.PotGrip;
                    }
                }
                break;
            case BoilingEgg.PotGrip:
                if (flag)
                {
                    uimanager.ShowNextUI(); //���� UI
                    pot.GetComponent<BoxCollider>().enabled = true;
                    pot.GetComponent<OutLineObject>().OutLineOn();  //�ܰ��� Ȱ��
                    circle1_eff.SetActive(true);        //����Ʈ Ȱ��ȭ
                    watermidpos.SetActive(true); 
                    flag = false;
                }
                else
                {
                    //���� ��ũ�� ��
                    if (!stay && pot.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle1_eff.SetActive(false);        //����Ʈ ��Ȱ��ȭ
                        uimanager.ShowResultUI(0);  //���߽��ϴ�


                        //���� ����Ʈ
                        GameObject o_effect = GameObject.Instantiate(star_eff, watermidpos.transform.position, watermidpos.transform.rotation);
                        o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                        Destroy(o_effect, 2f);                          //2�� �Ŀ� ����

                        watermidpos.SetActive(false);
                        Invoke("StayTime", staytime);
                        stay = true;
                    }
                }
                break;
            case BoilingEgg.FillEgg:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    for (int i = 0; i < eggs.Length; i++)
                    {
                        eggs[i].GetComponent<Rigidbody>().isKinematic = false;
                        eggs[i].GetComponent<BoxCollider>().enabled = true;
                    }
                    eggsinpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay&&eggsinpos.GetComponent<GoalInfoObject>().GetCount() >= 6)
                    {
                        uimanager.ShowResultUI(0);  //���߽��ϴ�

                        for (int i = 0; i < eggs.Length; i++)
                        {
                            eggs[i].GetComponent<Rigidbody>().isKinematic = false;
                            eggs[i].transform.parent = pot.transform;
                        }

                        //���� ����Ʈ
                        GameObject o_effect = GameObject.Instantiate(star_eff, eggsinpos.transform.position, eggsinpos.transform.rotation);
                        o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                        Destroy(o_effect, 2f);                          //2�� �Ŀ� ����

                        eggsinpos.SetActive(false);
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case BoilingEgg.FillWater:
                if(flag)
                {
                    uimanager.ShowNowUI();
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay&&water.activeInHierarchy)
                    {
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        gameObject.GetComponent<SimpleFadeMove>().CheckButton();
                        stay = false;
                    }
                }
                break;
            case BoilingEgg.MovePot:
                if(!flag)
                {
                    if (gameObject.GetComponent<SimpleFadeMove>().MoveDone())
                    {
                        
                        uimanager.ShowNowUI();
                        waterflow.SetActive(true);
                        water.SetActive(false);
                        firemidpos.SetActive(true);
                        pot.GetComponent<OutLineObject>().ResetObjectSetting();
                        circle2_eff.SetActive(true);

                        stay = true;
                        flag = true;
                    }
                }
                else
                {
                    //�Ķ������� �δ��� ���� ��ġ �Ҷ�
                    if (stay && pot.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle2_eff.SetActive(false);        //����Ʈ ��Ȱ��ȭ
                        //���������� ������
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 1)
                        {
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            score += 3;    //���� �߰�

                            //���� ����Ʈ
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                            Destroy(o_effect, 2f);                          //2�� �Ŀ� ����

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 2)
                        {
                            uimanager.ShowResultUI(1);  //�߾�
                            score += 2;    //���� �߰�

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 3)
                        {
                            uimanager.ShowResultUI(1);  //�߾�
                            score += 1;    //���� �߰�

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case BoilingEgg.OnFire:
                if (flag)
                {
                    //������Ʈ ��Ȱ��ȭ
                    firemidpos.SetActive(false);
                    //�Ķ����� ����
                    pot.GetComponent<Interactable>().enabled = false;
                    pot.GetComponent<Rigidbody>().isKinematic = true;
                    pot.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                    pot.GetComponent<BoxCollider>().enabled = false;
                    //������Ʈ Ȱ��ȭ
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
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
            case BoilingEgg.SaltIn:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    saltinpos.SetActive(true);
                    salt.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    flag = true;
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
            case BoilingEgg.TempUp:
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
            case BoilingEgg.Stay:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    //�ð����� Ȱ��ȭ
                    TimeWindow.SetActive(true);
                    ticktok = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    ClockWork();    //�ð���

                    if (stay)
                    {
                        if(!steam_eff1.activeInHierarchy&& Mathf.Round(limit_f)<=8)
                        {
                            steam_eff1.SetActive(true);
                        }
                        else if (!steam_eff2.activeInHierarchy && Mathf.Round(limit_f) <=5)
                        {
                            steam_eff2.SetActive(true);
                        }

                        if(Mathf.Round(limit_f)<=0)
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
            case BoilingEgg.OffFire:
                if (flag)
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

        watermidpos.SetActive(false);
        eggsinpos.SetActive(false);
        firemidpos.SetActive(false);

        waterflow.SetActive(false);
        water.SetActive(false);
        circle1_eff.SetActive(false);
        circle2_eff.SetActive(false);    
        steam_eff1.SetActive(false);
        steam_eff2.SetActive(false);

        pot.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < eggs.Length; i++)
        {
            eggs[i].GetComponent<Rigidbody>().isKinematic = true;
            eggs[i].GetComponent<BoxCollider>().enabled = false;
        }
    }
}
