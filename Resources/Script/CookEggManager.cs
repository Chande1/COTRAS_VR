using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CookEggManager : MonoBehaviour
{
    enum CookEggIng
    {
        Start = 0,
        MoveFryfan,
        WorkFire,
        InOil,
        PlusTemp,
        BrokenEgg,
        MakeFried,
        MidFried,
        SaltFried,
        BurnFried,
        ReFried,
        MoveFried,
        PutTurner,
        OffFire
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookEggIng ing;
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
    [SerializeField] GameObject egg_1;      //�ް�(�ȱ�����)
    [SerializeField] GameObject egg_2;      //�ް�(������)
    [SerializeField] GameObject oil;        //�⸧
    [SerializeField] GameObject salt;       //�ұ�



    [Header("�߰� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject fanfire;
    [SerializeField] GameObject egg_oil;    //�Ķ����� �⸧
    [SerializeField] GameObject fried_1;    //�Ķ���1
    [SerializeField] GameObject fried_2;
    [SerializeField] GameObject fried_3;
    [SerializeField] GameObject fried_4;
    [SerializeField] GameObject holoegg;    //�ް� �ڸ�
    [SerializeField] GameObject OnOff_btn;  //������ư
    [SerializeField] GameObject OnOff_txt;  //��������
    [SerializeField] GameObject NumMinus_btn;   //���ҹ�ư
    [SerializeField] GameObject NumPlus_btn;    //������ư
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;

    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject oilinpos;
    [SerializeField] GameObject eggpos;
    [SerializeField] GameObject saltpos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject turnerpos;
    [SerializeField] GameObject midpos;

    [Header("����Ʈ")]
    [SerializeField] GameObject circle1_eff;    //ū�� ȿ��
    [SerializeField] GameObject circle2_eff;    //������ ȿ��
    [SerializeField] GameObject star_eff;       //�� ȿ��

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
            case CookEggIng.Start:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    flag = true;
                }
                else
                {
                    //������ �۵��� ��������
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        ing = CookEggIng.MoveFryfan;
                    }
                }
                break;
            case CookEggIng.MoveFryfan:
                if (flag)
                {
                    uimanager.ShowNextUI(); //���� UI
                    fryingfan.GetComponent<Rigidbody>().isKinematic = false;
                    fryingfan.GetComponent<BoxCollider>().enabled = true;
                    fryingfan.GetComponent<OutLineObject>().OutLineOn();  //�ܰ��� Ȱ��
                    circle1_eff.SetActive(true);        //����Ʈ Ȱ��ȭ
                    firemidpos.SetActive(true); //�Ķ����� ���� ����
                    flag = false;
                }
                else
                {
                    //�Ķ������� �δ��� ���� ��ġ �Ҷ�
                    if (!stay && fryingfan.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle1_eff.SetActive(false);        //����Ʈ ��Ȱ��ȭ
                        //���������� ������
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMargin())
                        {
                            Debug.Log("��������!");

                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            score++;    //���� �߰�
                            
                            //���� ����Ʈ
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                            Destroy(o_effect, 2f);                          //2�� �Ŀ� ����
                            
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else
                        {
                            Debug.Log("��������!");

                            uimanager.ShowNextUI();  //�߾ӿ� �����ּ���~
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }
                }
                break;
            case CookEggIng.WorkFire:
                if (!flag)
                {
                    //������Ʈ ��Ȱ��ȭ
                    firemidpos.SetActive(false);
                    //�Ķ����� ����
                    fryingfan.GetComponent<Interactable>().enabled = false;
                    fryingfan.GetComponent<Rigidbody>().isKinematic = true;
                    fryingfan.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                    fryingfan.GetComponent<BoxCollider>().enabled = false;
                    //������Ʈ Ȱ��ȭ
                    uimanager.ShowNextUI();
                    OnOff_btn.SetActive(true);
                    flag = true;
                }
                else
                {
                    
                    //�ִϸ��̼��� Ȱ��ȭ �Ǿ�������
                    if (stay&&OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if(OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //�ִϸ��̼� ��Ȱ��ȭ
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            OnOff_btn.GetComponent<BoxCollider>().enabled = false;
                            //OnOff_btn.GetComponent<FingerTouchObject>().enabled = false;
                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                break;
            case CookEggIng.InOil:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    oil.GetComponent<Rigidbody>().isKinematic = false;
                    oil.GetComponent<BoxCollider>().enabled = true;
                    oilinpos.SetActive(true);
                    stay = false;
                    flag = false;
                }
                else
                {
                    if(!stay&& oilinpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        //egg_oil.SetActive(true);    //�Ķ����� �⸧ �߰�
                        oilinpos.SetActive(false);
                        //fanfire.SetActive(false);   //�Ķ����� ��
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = true ;
                    }
                }
                break;
            case CookEggIng.PlusTemp:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    NumMinus_btn.SetActive(true);
                    NumPlus_btn.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if(stay)
                    {
                        //���̳ʽ� ��ư ��������
                        if(NumMinus_btn.GetComponent<FingerTouchObject>().GetNowAni())
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
                        if(temperature>=5)
                        {
                            //������Ʈ ��Ȱ��ȭ
                            NumMinus_btn.GetComponent<BoxCollider>().enabled = false;
                            NumPlus_btn.GetComponent<BoxCollider>().enabled = false;
                            //UI
                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookEggIng.BrokenEgg:
                if(flag)
                {
                    uimanager.ShowNumUI(7, 7);
                    egg_1.GetComponent<Rigidbody>().isKinematic = false;
                    egg_1.GetComponent<BoxCollider>().enabled = true;
                    holoegg.SetActive(true);    //��� ���� ��ġ ǥ��
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay&&holoegg.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        egg_2.SetActive(true);                              //���� �ް� ���̰�
                        egg_1.GetComponent<MeshRenderer>().enabled = false; //���� �ް� �Ⱥ��̰�
                        holoegg.SetActive(false);                           //���� �ް� ��ġ ��Ȱ��ȭ
                                                                            //UI
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookEggIng.MakeFried:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    eggpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if(stay&&eggpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        //������ �Ķ��� Ȱ��ȭ
                        fried_1.SetActive(true);
                        uimanager.ShowNextUI();
                        //�ð����� Ȱ��ȭ
                        TimeWindow.SetActive(true);
                        ticktok = true;
                        //��������
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookEggIng.MidFried:
                if(flag)//�ұ���
                {
                    uimanager.ShowNowUI();
                    salt.GetComponent<Rigidbody>().isKinematic = false;
                    salt.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    Invoke("StayTime", staytime);
                    flag = false;
                }
                else//�ұ���
                {
                    if(stay)
                    {
                        uimanager.ShowNowUI();
                        stay = false;
                    }
                    
                }
                break;
            case CookEggIng.SaltFried:
                if(!flag)
                {
                    uimanager.ShowNextUI();
                    saltpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && saltpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        eggdone = true;
                        stay = false;
                    }
                }
                break;
            case CookEggIng.BurnFried:
                if (flag)
                {
                    uimanager.ShowNextUI();
                    Invoke("StayTime", staytime);
                    flag = false;
                }
                break;
            case CookEggIng.ReFried:
                if(!flag)
                {
                    uimanager.ShowNextUI();
                    Invoke("CookEggTime", staytime);                    //BrokenEgg�� �̵�
                    flag = true;
                }
                break;
            case CookEggIng.MoveFried:
                if(flag)
                {
                    //�ð����� ��Ȱ��ȭ
                    TimeWindow.SetActive(false);
                    uimanager.ShowNumUI(13,13);
                    turner.GetComponent<Rigidbody>().isKinematic = false;
                    turner.GetComponent<BoxCollider>().enabled = true;
                    circle2_eff.SetActive(true);
                    friedpos.SetActive(true);
                    midpos.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay && friedpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        midpos.GetComponent<AttachObject>().DetachObject();
                        midpos.SetActive(false);
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Debug.Log("Ʈ���� �۵�");
                        circle2_eff.SetActive(false);
                        friedpos.SetActive(false);
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookEggIng.PutTurner:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    turnerpos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && turnerpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookEggIng.OffFire:
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
                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            //Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                
                break;
            default:
                break;
        }

        //�ð� ���� �帣��
        if(ticktok)
        {
            ClockWork();

            if(Mathf.Round(limit_f) == 20&&fried_1.activeSelf)
            {
                fried_1.SetActive(false);
                fried_2.transform.position = fried_1.transform.position;
                fried_2.transform.rotation = fried_1.transform.rotation;
                fried_2.SetActive(true);
            }
            else if(Mathf.Round(limit_f) == 10&& fried_2.activeSelf)
            {
                fried_2.SetActive(false);
                fried_3.transform.position = fried_2.transform.position;
                fried_3.transform.rotation = fried_2.transform.rotation;
                fried_3.SetActive(true);
                if(eggdone)
                {
                    ing=CookEggIng.MoveFried;
                    ticktok = false;
                }
            }
            else if (Mathf.Round(limit_f) == 0 && fried_3.activeSelf)
            {
                fried_3.SetActive(false);
                fried_4.transform.position = fried_3.transform.position;
                fried_4.transform.rotation = fried_3.transform.rotation;
                fried_4.SetActive(true);
                ticktok = false;
                TimeWindow.SetActive(false);
            }
        }
    }


    void ShowInductionNum()
    {
        if(Num_1.activeSelf)
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
            ing = CookEggIng.BurnFried;
        }
    }

    void StayTime()
    {
        Debug.Log("���� �ܰ�:" + (ing + 1));
        ing++;
    }

    void CookEggTime()
    {
        holoegg.GetComponent<GoalInfoObject>().ResetGoalInfoObject();   //����ް� ���� �ʱ�ȭ
        eggpos.GetComponent<GoalInfoObject>().ResetGoalInfoObject();   //�Ķ��� ���� �ʱ�ȭ
        egg_1.GetComponent<OutLineObject>().ResetPostion(); //�ް� ���� �ڸ��� �̵�
        salt.GetComponent<OutLineObject>().ResetPostion(); //�ұ� ���� �ڸ��� �̵�
        turner.GetComponent<OutLineObject>().ResetPostion(); //������ ���� �ڸ��� �̵�

        fried_4.SetActive(false);                           //ź �Ķ��� �Ⱥ��̰�
        egg_2.SetActive(false);                             //���� �ް� �Ⱥ��̰�
        egg_1.GetComponent<MeshRenderer>().enabled = true;  //���� �ް� ���̰�
        limit_f = limit;                                    //���ѽð� �ʱ�ȭ
        ing = CookEggIng.BrokenEgg;
        flag = true;
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


        fried_1.SetActive(false);
        fried_2.SetActive(false);
        fried_3.SetActive(false);
        fried_4.SetActive(false);
        
        holoegg.SetActive(false);
        //egg_oil.SetActive(false);
        egg_2.SetActive(false);
       
        OnOff_btn.SetActive(false);
        OnOff_txt.SetActive(false);
        NumMinus_btn.SetActive(false);
        NumPlus_btn.SetActive(false);
        Num_1.SetActive(false);
        Num_2.SetActive(false);
        Num_3.SetActive(false);
        Num_4.SetActive(false);
        Num_5.SetActive(false);
        
        firemidpos.SetActive(false);
        oilinpos.SetActive(false);
        eggpos.SetActive(false);
        saltpos.SetActive(false);
        friedpos.SetActive(false);
        turnerpos.SetActive(false);
        midpos.SetActive(false);

        circle1_eff.SetActive(false);
        circle2_eff.SetActive(false);

        oil.GetComponent<Rigidbody>().isKinematic = true;
        oil.GetComponent<BoxCollider>().enabled = false;

        salt.GetComponent<Rigidbody>().isKinematic = true;
        salt.GetComponent<BoxCollider>().enabled = false;

        egg_1.GetComponent<Rigidbody>().isKinematic = true;
        egg_1.GetComponent<BoxCollider>().enabled = false;

        turner.GetComponent<Rigidbody>().isKinematic = true;
        turner.GetComponent<BoxCollider>().enabled = false;

        fryingfan.GetComponent<Rigidbody>().isKinematic = true;
        fryingfan.GetComponent<BoxCollider>().enabled = false;
    }
}