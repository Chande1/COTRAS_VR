using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CookHamManager : MonoBehaviour
{
    enum CookingHam
    {
        Start = 0,
        MoveFryfan,
        WorkFire,
        InOil,
        PlusTemp,
        OpenPackage,
        HamIn,
        MakeHam,
        OffFire,
        MoveHam
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookingHam ing;
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
    [SerializeField] GameObject package_t;  //���� ��
    [SerializeField] GameObject package_b;  //���� �Ʒ�
    [SerializeField] GameObject oil;        //�⸧

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
    [SerializeField] GameObject[] ham;

    [Header("������Ʈ ��ȣ�ۿ� ��ġ")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject oilinpos;
    [SerializeField] GameObject hampos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject midpos;

    [Header("����Ʈ")]
    [SerializeField] GameObject circle1_eff;    //ū�� ȿ��
    [SerializeField] GameObject circle2_eff;    //������ ȿ��
    [SerializeField] GameObject star_eff;       //�� ȿ��
    [SerializeField] GameObject steam_eff;

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
            case CookingHam.Start:
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
                        ing = CookingHam.MoveFryfan;
                    }
                }
                break;
            case CookingHam.MoveFryfan:
                if (flag)
                {
                    uimanager.ShowNextUI(); //���� UI
                    fryingfan.GetComponent<BoxCollider>().enabled = true;
                    fryingfan.GetComponent<Rigidbody>().isKinematic = false;
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
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength()==1)
                        {
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            score+=3;    //���� �߰�

                            //���� ����Ʈ
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                            Destroy(o_effect, 2f);                          //2�� �Ŀ� ����

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 2)
                        {
                            uimanager.ShowResultUI(1);  //�߾�
                            score += 2;    //���� �߰�

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 3)
                        {
                            uimanager.ShowResultUI(1);  //�߾�
                            score += 1;    //���� �߰�

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }
                }
                break;
            case CookingHam.WorkFire:
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
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
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
            case CookingHam.InOil:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    oil.GetComponent<BoxCollider>().enabled = true;
                    oil.GetComponent<Rigidbody>().isKinematic = false;
                    oilinpos.SetActive(true);
                    stay = false;
                    flag = false;
                }
                else
                {
                    if (!stay && oilinpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("Ʈ���� �۵�");
                        //egg_oil.SetActive(true);    //�Ķ����� �⸧ �߰�
                        oilinpos.SetActive(false);
                        //fanfire.SetActive(false);   //�Ķ����� ��
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = true;
                    }
                }
                break;
            case CookingHam.PlusTemp:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    NumMinus_btn.SetActive(true);
                    NumPlus_btn.SetActive(true);
                    stay = true;
                    flag = true;
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
            case CookingHam.OpenPackage:
                if (flag)
                {
                    uimanager.ShowNowUI();

                    //����
                    package_b.GetComponent<Interactable>().enabled = true;
                    package_b.GetComponent<BoxCollider>().enabled = true;
                    package_b.GetComponent<Rigidbody>().useGravity = true;
                    package_b.GetComponent<Rigidbody>().isKinematic = false;

                    package_t.GetComponent<Interactable>().enabled = true;
                    package_t.GetComponent<BoxCollider>().enabled = true;
                    //package_t.GetComponent<Rigidbody>().useGravity = true;
                    //package_t.GetComponent<Rigidbody>().isKinematic = false;


                    stay = true;
                    flag = false;
                }
                else
                {
                    if (stay && package_t.GetComponent<GripState>().GetGripStateValue() == GripStateValue.Gripping)
                    {
                        package_t.GetComponent<Rigidbody>().useGravity = true;
                        //UI
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                   
                }
                break;
            case CookingHam.HamIn:
                if(!flag)
                {
                    uimanager.ShowNowUI();
                    hampos.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && hampos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        for (int i = 0; i < ham.Length; i++)
                        {
                            ham[i].SetActive(true);
                        }
                        
                        //UI
                        uimanager.ShowResultUI(0);  //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingHam.MakeHam:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    hampos.SetActive(false);
                    stay = false;
                    flag = false;
                }
                else
                {
                    if (!stay)
                    {
                        if(!steam_eff.activeSelf)
                        {
                            for (int i = 0; i < ham.Length; i++)
                            {
                                ham[i].GetComponent<Animator>().SetBool("H_C", true);
                            }
                            steam_eff.SetActive(true);   //�Ķ����� ��
                        }
                       
                        //���� �� ������
                        if (ham[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Ham_cook")&&
                            ham[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0f)
                        {
                            Debug.Log("���� ��� �;����ϴ�!");
                            uimanager.ShowResultUI(0);  //�� �;����ϴ�
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }

                    
                }
                break;
            case CookingHam.OffFire:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
                    OnOff_btn.GetComponent<BoxCollider>().enabled = true;
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
            case CookingHam.MoveHam:
                if (flag)
                {
                    //�ð����� Ȱ��ȭ
                    TimeWindow.SetActive(true);
                    uimanager.ShowNowUI();
                    turner.GetComponent<BoxCollider>().enabled = true;
                    turner.GetComponent<Rigidbody>().isKinematic = false;

                    circle2_eff.SetActive(true);
                    friedpos.SetActive(true);
                    midpos.SetActive(true);
                    ticktok = true;
                    stay = true;
                    flag = false;
                }
                else
                {
                    
                    //ClockWork();    //�ð���

                    if (stay && friedpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("���ÿ� ����!");
                        //midpos.GetComponent<AttachObject>().DetachObjects(friedpos);
                        midpos.GetComponent<AttachObject>().DetachObject();
                        friedpos.GetComponent<GoalInfoObject>().ResetGoalInfoObject();
                    }
                    else if(stay)
                    {
                        if (friedpos.GetComponent<GoalInfoObject>().GetCount() >= 8&&
                            Mathf.Round(limit_f)>0)
                        {
                            midpos.SetActive(false);
                            uimanager.ShowResultUI(0);  //���߽��ϴ�
                            ticktok = false;
                            Debug.Log("Ʈ���� �۵�");
                            circle2_eff.SetActive(false);
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
        if (ticktok&& limit_f>=0)
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

        for (int i = 0; i < ham.Length; i++)
        {
            ham[i].SetActive(false);
        }


        firemidpos.SetActive(false);
        oilinpos.SetActive(false);
        friedpos.SetActive(false);
        midpos.SetActive(false);
        hampos.SetActive(false);

        circle1_eff.SetActive(false);
        circle2_eff.SetActive(false);
        steam_eff.SetActive(false);

        package_t.GetComponent<BoxCollider>().enabled = false;
        package_t.GetComponent<Rigidbody>().isKinematic = true;

        package_b.GetComponent<BoxCollider>().enabled = false;
        package_b.GetComponent<Rigidbody>().isKinematic = true;

        oil.GetComponent<BoxCollider>().enabled = false;
        oil.GetComponent<Rigidbody>().isKinematic = true;

        fryingfan.GetComponent<BoxCollider>().enabled = false;
        fryingfan.GetComponent<Rigidbody>().isKinematic = true;

        turner.GetComponent<BoxCollider>().enabled = false;
        turner.GetComponent<Rigidbody>().isKinematic = true;
    }
}