using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BasicControlManager : MonoBehaviour
{
    [Header("��")]
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    [Header("�ؽ�Ʈ ������Ʈ")]
    [SerializeField] GameObject Title_1;
    [SerializeField] GameObject Hold_2;
    [SerializeField] GameObject Move_3;
    [SerializeField] GameObject Throw_4;
    [SerializeField] GameObject End_5;
    [SerializeField] GameObject Next_0;

    [Header("�׸� ������Ʈ")]
    [SerializeField] GameObject[] GripObject;

    [Header("��ǥ ����")]
    [SerializeField] GameObject GoalObject;


    int ingnum;
    bool flag;
    int successcount;

    void Awake()
    {
        StartSetting();
    }

    void Update()
    {
        //������ ����
        if(ingnum==0&& RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Title_1.SetActive(false);
            
            for (int i = 0; i < GripObject.Length; i++)
            {
                GripObject[i].SetActive(true);
            }

            ingnum++;
        }

        //�� ���� ����
        switch(ingnum)
        {
            case 1: //:3�� ���
                if(!flag)
                {
                    Hold_2.SetActive(true);
                    flag = true;
                }

                if(successcount>=GripObject.Length)
                {
                    ResetSuccessCount();
                    Hold_2.SetActive(false);
                    Next_0.SetActive(true);
                    ingnum++;
                }
                break;
            case 2: //:���� ���� �̵�
                if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
                        LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Next_0.SetActive(false);
                    flag = false;
                    ingnum++;
                }
                break;
            case 3: //:������ ����
                if(!flag)
                {
                    Move_3.SetActive(true);
                    GoalObject.SetActive(true);
                    flag = true;
                }

                if (successcount >= GripObject.Length)
                {
                    ResetSuccessCount();
                    Move_3.SetActive(false);
                    GoalObject.SetActive(false);
                    End_5.SetActive(true);
                    ingnum++;
                }
                break;
            case 4: //:����!

                break;
            default:
                break;
        }

    }

    void StartSetting()
    {
        ingnum = 0;
        flag = false;
        successcount = 0;

        Title_1 = GameObject.Find("1_Title");
        Hold_2 = GameObject.Find("2_Hold");
        Move_3 = GameObject.Find("3_Move");
        Throw_4 = GameObject.Find("4_Throw");
        End_5 = GameObject.Find("5_End");
        Next_0 = GameObject.Find("0_Next");
        GoalObject = GameObject.Find("GoalObject");

        //Title_1.SetActive(false);
        Hold_2.SetActive(false);
        Move_3.SetActive(false);
        Throw_4.SetActive(false);
        End_5.SetActive(false);
        Next_0.SetActive(false);
        GoalObject.SetActive(false);
        for (int i = 0; i < GripObject.Length; i++)
        {
            GripObject[i].SetActive(false);
        }
    }

    public void SuccessCountUp()
    {
        successcount += 1;
    }

    public void SuccessCountDown()
    {
        successcount -= 1;
    }

    public void ResetSuccessCount()
    {
        successcount= 0;
    }
}
