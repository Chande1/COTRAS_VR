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
    [SerializeField] GameObject salt;
    [SerializeField] GameObject[] eggs;

    [Header("추가 상호작용 오브젝트")]
    [SerializeField] GameObject OnOff_btn;  //전원버튼
    [SerializeField] GameObject OnOff_txt;  //전원글자
    [SerializeField] GameObject NumMinus_btn;   //감소버튼
    [SerializeField] GameObject NumPlus_btn;    //증가버튼
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;

    [SerializeField] GameObject waterflow;
   

    [Header("오브젝트 상호작용 위치")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject Startpoint2;
    [SerializeField] GameObject watermidpos;
    [SerializeField] GameObject eggsinpos;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject saltinpos;

    [Header("이펙트")]
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
            case BoilingEgg.Start:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    flag = true;
                }
                else
                {
                    //오른손 작동시 다음으로
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)|| LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        ing = BoilingEgg.PotGrip;
                    }
                }
                break;
            case BoilingEgg.PotGrip:
                if (flag)
                {
                    uimanager.ShowNextUI(); //다음 UI
                    pot.GetComponent<BoxCollider>().enabled = true;
                    pot.GetComponent<OutLineObject>().OutLineOn();  //외각선 활성
                    circle1_eff.SetActive(true);        //이펙트 활성화
                    watermidpos.SetActive(true); 
                    flag = false;
                }
                else
                {
                    //냄비 싱크대 위
                    if (!stay && pot.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle1_eff.SetActive(false);        //이펙트 비활성화
                        uimanager.ShowResultUI(0);  //잘했습니다


                        //정답 이펙트
                        GameObject o_effect = GameObject.Instantiate(star_eff, watermidpos.transform.position, watermidpos.transform.rotation);
                        o_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                        Destroy(o_effect, 2f);                          //2초 후에 삭제

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
                        uimanager.ShowResultUI(0);  //잘했습니다

                        for (int i = 0; i < eggs.Length; i++)
                        {
                            eggs[i].GetComponent<Rigidbody>().isKinematic = false;
                            eggs[i].transform.parent = pot.transform;
                        }

                        //정답 이펙트
                        GameObject o_effect = GameObject.Instantiate(star_eff, eggsinpos.transform.position, eggsinpos.transform.rotation);
                        o_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                        Destroy(o_effect, 2f);                          //2초 후에 삭제

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
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                    //후라이팬이 인덕션 위에 위치 할때
                    if (stay && pot.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle2_eff.SetActive(false);        //이펙트 비활성화
                        //오차범위가 작으면
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 1)
                        {
                            uimanager.ShowResultUI(0);  //잘했습니다
                            score += 3;    //점수 추가

                            //정답 이펙트
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                            Destroy(o_effect, 2f);                          //2초 후에 삭제

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 2)
                        {
                            uimanager.ShowResultUI(1);  //중앙
                            score += 2;    //점수 추가

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 3)
                        {
                            uimanager.ShowResultUI(1);  //중앙
                            score += 1;    //점수 추가

                            Invoke("StayTime", staytime);
                            stay = false;
                        }
                    }
                }
                break;
            case BoilingEgg.OnFire:
                if (flag)
                {
                    //오브젝트 비활성화
                    firemidpos.SetActive(false);
                    //후라이팬 고정
                    pot.GetComponent<Interactable>().enabled = false;
                    pot.GetComponent<Rigidbody>().isKinematic = true;
                    pot.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                    pot.GetComponent<BoxCollider>().enabled = false;
                    //오브젝트 활성화
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
                    stay = true;
                    flag = false;
                }
                else
                {
                    //애니메이션이 활성화 되어있을때
                    if (stay && OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if (OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //애니메이션 비활성화
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            OnOff_btn.GetComponent<BoxCollider>().enabled = false;
                            //OnOff_btn.GetComponent<FingerTouchObject>().enabled = false;
                            uimanager.ShowResultUI(0);  //잘했습니다
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
                        Debug.Log("트리거 작동");
                        saltinpos.SetActive(false);
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                        //마이너스 버튼 눌렀을때
                        if (NumMinus_btn.GetComponent<FingerTouchObject>().GetNowAni())
                        {
                            if (NumMinus_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                            {
                                Debug.Log("마이너스!");
                                if (temperature > 0)
                                    temperature -= 1;

                                ShowInductionNum();
                                //애니메이션 비활성화
                                NumMinus_btn.GetComponent<FingerTouchObject>().OffAni();
                            }
                        }
                        //플러스 버튼 눌렀을때
                        if (NumPlus_btn.GetComponent<FingerTouchObject>().GetNowAni())
                        {
                            if (NumPlus_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                            {
                                Debug.Log("플러스!");
                                if (temperature < 5)
                                    temperature += 1;

                                ShowInductionNum();
                                //애니메이션 비활성화
                                NumPlus_btn.GetComponent<FingerTouchObject>().OffAni();
                            }
                        }
                        if (temperature >= 5)
                        {
                            //오브젝트 비활성화
                            NumMinus_btn.GetComponent<BoxCollider>().enabled = false;
                            NumPlus_btn.GetComponent<BoxCollider>().enabled = false;
                            //UI
                            uimanager.ShowResultUI(0);  //잘했습니다
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
                    //시간제한 활성화
                    TimeWindow.SetActive(true);
                    ticktok = true;
                    stay = true;
                    flag = true;
                }
                else
                {
                    ClockWork();    //시간초

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
                            uimanager.ShowResultUI(0);  //잘했습니다
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
                    //애니메이션이 활성화 되어있을때
                    if (stay && OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if (OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //인덕션 끄기
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
                            //애니메이션 비활성화
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            uimanager.ShowResultUI(0);  //잘했습니다
                            Invoke("StayTime", staytime);

                            stay = false;
                        }
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
