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
    [SerializeField] float staytime;    //대기시간
    [SerializeField] int score;         //점수
    [SerializeField] int temperature;   //온도

    [Header("플레이어")]
    [SerializeField] GameObject player;
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    [Header("시간제한")]
    [SerializeField] GameObject TimeWindow;
    [SerializeField] Image clock;   //시계 이미지
    [SerializeField] Text time;     //시간표시
    [SerializeField] int limit;     //제한시간


    [Header("손 상호작용 오브젝트")]
    [SerializeField] GameObject pot;

    [Header("추가 상호작용 오브젝트")]
    [SerializeField] GameObject potonoff;  //전원버튼
    [SerializeField] GameObject pottop;
    [SerializeField] GameObject water;
    [SerializeField] GameObject rice;
    [SerializeField] GameObject pot2;


    [Header("오브젝트 상호작용 위치")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject waterflowRpos;
    [SerializeField] GameObject waterflowLpos;
    [SerializeField] GameObject potinpos;

    [Header("이펙트")]
    [SerializeField] GameObject waterdrop;
    [SerializeField] GameObject circle1_eff;
    [SerializeField] GameObject star_eff;
    [SerializeField] GameObject steam_eff1;

    bool flag;
    bool stay;
    bool ticktok;
    float limit_f;  //현재 시간


    private void Awake()
    {
        player.transform.position = Startpoint.transform.position;  //시작 위치로 플레이어 이동
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
                    //오른손 작동시 다음으로
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
                    //오른손 작동시 다음으로
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
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                        
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                    pottop.GetComponent<OutLineObject>().OutLineOn();  //외각선 활성
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay && !pottop.GetComponent<OutLineObject>().OutLineWork())
                    {
                        pottop.GetComponent<Animator>().SetBool("pt_close", true);
                        pottop.GetComponent<BoxCollider>().enabled = false;
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                    potonoff.GetComponent<OutLineObject>().OutLineOn();  //외각선 활성
                    stay = true;
                    flag = true;
                }
                else
                {
                    if (stay && !potonoff.GetComponent<OutLineObject>().OutLineWork())
                    {
                        steam_eff1.SetActive(true);
                        potonoff.GetComponent<BoxCollider>().enabled = false;
                        uimanager.ShowResultUI(0);  //잘했습니다
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            default:
                break;
        }

        //시간 제한 흐르기
        if (ticktok)
        {
            ClockWork();
        }
    }

    
    //시계 작동
    void ClockWork()
    {
        limit_f -= Time.deltaTime;
        time.text = Mathf.Round(limit_f).ToString();
        float temp = 1000 / limit;
        clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //시간초가 0이 되면
        if (Mathf.Round(limit_f) == 0)
        {
            Debug.Log("시간초과!");
        }
    }

    void StayTime()
    {
        Debug.Log("다음 단계:" + (ing + 1));
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
