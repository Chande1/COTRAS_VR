using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class CookRamanManager : MonoBehaviour
{
    enum CookingRamen
    {
        Start = 0,
        MovePot,
        WorkFire,
        BoilWater,
        OpenRamen,
        InRamen,
        InSoup,
        CloseTop,
        WellRamen,
        OffFire,
        PutPot
    }

    [SerializeField] SimpleUIManager uimanager;
    [SerializeField] CookingRamen ing;
    [SerializeField] float staytime;    //���ð�
    [SerializeField] int score;         //����
    [SerializeField] int temperature;   //�µ�

    [Header("�÷��̾�")]
    [SerializeField] GameObject player;
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;


    [Header("�� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject pot_b;      //���� �Ʒ�
    [SerializeField] GameObject pot_t;      //���� �Ѳ�
    [SerializeField] GameObject pot_t2;      //���� �Ѳ�
    [SerializeField] GameObject nuddle;     //��
    [SerializeField] GameObject soup_b;       //����
    [SerializeField] GameObject soup_t;       //����
    [SerializeField] GameObject trash;      //���� ������



    [Header("�߰� ��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject Ramen_end;  //�ϼ��� ���
    [SerializeField] GameObject water;          //��

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
    [SerializeField] GameObject SoupPos;
    [SerializeField] GameObject TopPos;

    [Header("����Ʈ")]
    [SerializeField] GameObject circle1_eff;    //ū�� ȿ��
    [SerializeField] GameObject circle2_eff;    //������ ȿ��
    [SerializeField] GameObject star_eff;       //�� ȿ��
    [SerializeField] GameObject waterboiled;    //����
    [SerializeField] GameObject watersteam;     //������

    bool flag;
    bool stay;
    bool objectcatch;

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
            case CookingRamen.Start:
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
                        ing = CookingRamen.MovePot;
                    }
                }
                break;
            case CookingRamen.MovePot:
                if (flag)
                {
                    uimanager.ShowNextUI(); //���� UI
                    pot_b.GetComponent<OutLineObject>().OutLineOn();  //�ܰ��� Ȱ��
                    circle1_eff.SetActive(true);        //����Ʈ Ȱ��ȭ
                    firemidpos.SetActive(true); //�Ķ����� ���� ����
                    flag = false;
                }
                else
                {
                    //�Ķ������� �δ��� ���� ��ġ �Ҷ�
                    if (!stay && pot_b.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle1_eff.SetActive(false);        //����Ʈ ��Ȱ��ȭ
                                                             //�Ķ����� ����
                        //pot_b.GetComponent<Interactable>().enabled = false;
                        //pot_b.GetComponent<Rigidbody>().useGravity = false;
                        //pot_b.GetComponent<Rigidbody>().isKinematic = true;
                        //pot_b.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                        //pot_b.GetComponent<BoxCollider>().enabled = false;

                        //���������� ������
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength()==1)
                        {
                            Debug.Log("����+3");

                            uimanager.ShowNumUI(0, false); //���߽��ϴ�
                            score += 3;

                            //���� ����Ʈ
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                            Destroy(o_effect, 2f);                          //2�� �Ŀ� ����

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if(firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 2)
                        {
                            Debug.Log("����+2");

                            score += 2;

                            uimanager.ShowNextUI();
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 3)
                        {
                            Debug.Log("����+1");

                            score += 1;

                            uimanager.ShowNextUI();
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }
                }
                break;
            case CookingRamen.WorkFire:
                if (!flag)
                {
                    //������Ʈ ��Ȱ��ȭ
                    firemidpos.SetActive(false);
                    pot_b.GetComponent<Interactable>().enabled = false;
                    //pot_b.GetComponent<Rigidbody>().useGravity = false;
                    pot_b.GetComponent<Rigidbody>().isKinematic = true;
                    pot_b.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                    pot_b.GetComponent<BoxCollider>().enabled = false;
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
                            NumMinus_btn.SetActive(true);
                            NumPlus_btn.SetActive(true);
                            stay = false;
                        }
                    }
                    else if(!stay&& temperature<5)
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
                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingRamen.BoilWater:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    Invoke("WaterEffect", 3f);
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(!stay)
                    {
                        Invoke("WaterEffect", 5f);
                        stay = true;
                    }
                }
                break;
            case CookingRamen.OpenRamen:
                if (!flag)
                {
                    uimanager.ShowNextUI();
                    //���� Ȱ��ȭ
                    trash.GetComponent<Interactable>().enabled = true;
                    trash.GetComponent<BoxCollider>().enabled = true;
                    trash.GetComponent<Rigidbody>().useGravity = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay&& trash.GetComponent<GripState>().GetGripStateValue()==GripStateValue.Gripping)
                    {
                        //�� Ȱ��ȭ
                        nuddle.GetComponent<Interactable>().enabled = true;
                        nuddle.GetComponent<BoxCollider>().enabled = true;
                        nuddle.GetComponent<Rigidbody>().useGravity = true;
                    }
                    else if(stay && trash.GetComponent<GripState>().GetGripStateValue() == GripStateValue.GripStop)
                    {
                        if (nuddle.GetComponent<GripState>().GetGripStateValue() == GripStateValue.Gripping)
                        {
                            //UI
                            uimanager.ShowNumUI(0, true); //���߽��ϴ�
                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case CookingRamen.InRamen:
                if (flag)
                {
                    uimanager.ShowNowUI();

                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay&&water.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        //UI
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingRamen.InSoup:
                if (!flag)
                {
                    uimanager.ShowNowUI();

                    //���� Ȱ��ȭ
                    soup_b.GetComponent<Interactable>().enabled = true;
                    soup_b.GetComponent<BoxCollider>().enabled = true;
                    soup_b.GetComponent<Rigidbody>().useGravity = true;

                    soup_t.GetComponent<Interactable>().enabled = true;
                    soup_t.GetComponent<BoxCollider>().enabled = true;

                    stay = true;
                    flag = true;
                }
                else
                {
                    if(stay&&soup_t.GetComponent<GripState>().GetGripStateValue() == GripStateValue.Gripping)
                    {

                        soup_t.GetComponent<Rigidbody>().useGravity = true;
                        SoupPos.SetActive(true);
                    }
                    if(stay&&SoupPos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        //UI
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                    break;
            case CookingRamen.CloseTop:
                if (flag)
                {
                    uimanager.ShowNowUI();
                    TopPos.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay&&TopPos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        if (GameObject.Find("vr_glove_right_model_slim(Clone)"))
                            GameObject.Find("vr_glove_right_model_slim(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>().skeletonBlend = 1;
                        if (GameObject.Find("vr_glove_left_model_slim(Clone)"))
                            GameObject.Find("vr_glove_left_model_slim(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>().skeletonBlend = 1;

                        pot_t.SetActive(false);
                        pot_t2.SetActive(true);

                        //pot_t.GetComponent<Rigidbody>().isKinematic = true;
                        //pot_t.GetComponent<Interactable>().enabled = false;
                        //pot_t.GetComponent<BoxCollider>().enabled = false;
                        //pot_t.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;

                        //pot_t.transform.position = TopPos.transform.position;
                        //pot_t.transform.parent = pot_b.transform;
                        //UI
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookingRamen.WellRamen:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    //pot_t.transform.parent = pot_b.transform;
                    //pot_t.GetComponent<BoxCollider>().enabled = false;
                    //pot_t.GetComponent<Rigidbody>().isKinematic = true;
                    //pot_t.GetComponent<Rigidbody>().useGravity = false;
                  
                    nuddle.SetActive(false);
                    Ramen_end.SetActive(true);
                    stay = true;
                    flag = true;
                }
                else
                {
                   if(stay)
                    {
                        //Invoke("StayTime", staytime);
                        Invoke("StayTime", 7f);
                        stay = false;
                    }
                }
                break;
            case CookingRamen.OffFire:
                if (flag)
                {
                    uimanager.ShowNextUI();
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
                            Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                break;
            case CookingRamen.PutPot:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    circle2_eff.SetActive(true);

                    pot_b.GetComponent<Interactable>().enabled = true;
                    pot_b.GetComponent<Rigidbody>().useGravity = true;
                    pot_b.GetComponent<Rigidbody>().isKinematic = false;
                    pot_b.GetComponent<SteamVR_Skeleton_Poser>().enabled = true;
                    pot_b.GetComponent<BoxCollider>().enabled = true;

                    stay = true;
                    flag = true;
                }
                else
                {
                    if(stay&&circle2_eff.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        pot_b.GetComponent<Rigidbody>().useGravity = true;
                        pot_b.GetComponent<Rigidbody>().isKinematic = false;
                        //UI
                        uimanager.ShowNumUI(0, true); //���߽��ϴ�
                        stay = false;
                    }
                }
                break;
            default:
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

    void WaterEffect()
    {
        if (!waterboiled.activeSelf)
        {
            waterboiled.SetActive(true);
            stay = false;
        }  
        else
        {
            watersteam.SetActive(true);
            Invoke("StayTime", staytime);
        }
    }

    void StayTime()
    {
        Debug.Log("���� �ܰ�:" + (ing + 1));
        ing++;
    }

    public void GripObject()
    {
        objectcatch = true;
    }


    void StartSetting()
    {
        flag = false;
        stay = false;
        objectcatch = false;
        score = 0;
        temperature = 0;

        uimanager = GetComponent<SimpleUIManager>();
        //ing = CookEggIng.Start;

        Ramen_end.SetActive(false);

        trash.GetComponent<Interactable>().enabled = false;
        trash.GetComponent<BoxCollider>().enabled = false;
        trash.GetComponent<Rigidbody>().useGravity = false;

        nuddle.GetComponent<Interactable>().enabled = false;
        nuddle.GetComponent<BoxCollider>().enabled = false;
        nuddle.GetComponent<Rigidbody>().useGravity = false;

        soup_b.GetComponent<Interactable>().enabled = false;
        soup_b.GetComponent<BoxCollider>().enabled = false;
        soup_b.GetComponent<Rigidbody>().useGravity = false;

        soup_t.GetComponent<Interactable>().enabled = false;
        soup_t.GetComponent<BoxCollider>().enabled = false;
        soup_t.GetComponent<Rigidbody>().useGravity = false;


        OnOff_btn.SetActive(false);
        OnOff_txt.SetActive(false);
        NumMinus_btn.SetActive(false);
        NumPlus_btn.SetActive(false);
        Num_1.SetActive(false);
        Num_2.SetActive(false);
        Num_3.SetActive(false);
        Num_4.SetActive(false);
        Num_5.SetActive(false);

        pot_t2.SetActive(false);


        firemidpos.SetActive(false);
        SoupPos.SetActive(false);
        TopPos.SetActive(false);

        circle1_eff.SetActive(false);
        circle2_eff.SetActive(false);
        waterboiled.SetActive(false);
        watersteam.SetActive(false);
    }
}
