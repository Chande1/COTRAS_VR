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
    [SerializeField] GameObject fryingfan;  //후라이팬
    [SerializeField] GameObject turner;     //뒤짚개
    [SerializeField] GameObject egg_1;      //달걀(안깨진거)
    [SerializeField] GameObject egg_2;      //달걀(깨진거)
    [SerializeField] GameObject oil;        //기름
    [SerializeField] GameObject salt;       //소금



    [Header("추가 상호작용 오브젝트")]
    [SerializeField] GameObject fanfire;
    [SerializeField] GameObject egg_oil;    //후라이팬 기름
    [SerializeField] GameObject fried_1;    //후라이1
    [SerializeField] GameObject fried_2;
    [SerializeField] GameObject fried_3;
    [SerializeField] GameObject fried_4;
    [SerializeField] GameObject holoegg;    //달걀 자리
    [SerializeField] GameObject OnOff_btn;  //전원버튼
    [SerializeField] GameObject OnOff_txt;  //전원글자
    [SerializeField] GameObject NumMinus_btn;   //감소버튼
    [SerializeField] GameObject NumPlus_btn;    //증가버튼
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;

    [Header("오브젝트 상호작용 위치")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject oilinpos;
    [SerializeField] GameObject eggpos;
    [SerializeField] GameObject saltpos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject turnerpos;
    [SerializeField] GameObject midpos;

    [Header("이펙트")]
    [SerializeField] GameObject circle1_eff;    //큰원 효과
    [SerializeField] GameObject circle2_eff;    //작은원 효과
    [SerializeField] GameObject star_eff;       //별 효과

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
            case CookEggIng.Start:
                if (!flag)
                {
                    uimanager.ShowNowUI();
                    flag = true;
                }
                else
                {
                    //오른손 작동시 다음으로
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        ing = CookEggIng.MoveFryfan;
                    }
                }
                break;
            case CookEggIng.MoveFryfan:
                if (flag)
                {
                    uimanager.ShowNextUI(); //다음 UI
                    fryingfan.GetComponent<Rigidbody>().isKinematic = false;
                    fryingfan.GetComponent<BoxCollider>().enabled = true;
                    fryingfan.GetComponent<OutLineObject>().OutLineOn();  //외각선 활성
                    circle1_eff.SetActive(true);        //이펙트 활성화
                    firemidpos.SetActive(true); //후라이팬 골인 지점
                    flag = false;
                }
                else
                {
                    //후라이팬이 인덕션 위에 위치 할때
                    if (!stay && fryingfan.GetComponent<OutLineObject>().ObjectArriveGoal())
                    {
                        circle1_eff.SetActive(false);        //이펙트 비활성화
                        //오차범위가 작으면
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMargin())
                        {
                            Debug.Log("오차적음!");

                            uimanager.ShowNumUI(0, true); //잘했습니다
                            score++;    //점수 추가
                            
                            //정답 이펙트
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                            Destroy(o_effect, 2f);                          //2초 후에 삭제
                            
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else
                        {
                            Debug.Log("오차많음!");

                            uimanager.ShowNextUI();  //중앙에 놓아주세요~
                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }
                }
                break;
            case CookEggIng.WorkFire:
                if (!flag)
                {
                    //오브젝트 비활성화
                    firemidpos.SetActive(false);
                    //후라이팬 고정
                    fryingfan.GetComponent<Interactable>().enabled = false;
                    fryingfan.GetComponent<Rigidbody>().isKinematic = true;
                    fryingfan.GetComponent<SteamVR_Skeleton_Poser>().enabled = false;
                    fryingfan.GetComponent<BoxCollider>().enabled = false;
                    //오브젝트 활성화
                    uimanager.ShowNextUI();
                    OnOff_btn.SetActive(true);
                    flag = true;
                }
                else
                {
                    
                    //애니메이션이 활성화 되어있을때
                    if (stay&&OnOff_btn.GetComponent<FingerTouchObject>().GetNowAni())
                    {
                        if(OnOff_btn.GetComponent<FingerTouchObject>().AnimatorIsDone())
                        {
                            //애니메이션 비활성화
                            OnOff_btn.GetComponent<FingerTouchObject>().OffAni();
                            OnOff_btn.GetComponent<BoxCollider>().enabled = false;
                            //OnOff_btn.GetComponent<FingerTouchObject>().enabled = false;
                            uimanager.ShowNumUI(0, true); //잘했습니다
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
                        Debug.Log("트리거 작동");
                        //egg_oil.SetActive(true);    //후라이팬 기름 추가
                        oilinpos.SetActive(false);
                        //fanfire.SetActive(false);   //후라이팬 열
                        uimanager.ShowNumUI(0, true); //잘했습니다
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
                        //마이너스 버튼 눌렀을때
                        if(NumMinus_btn.GetComponent<FingerTouchObject>().GetNowAni())
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
                        if(temperature>=5)
                        {
                            //오브젝트 비활성화
                            NumMinus_btn.GetComponent<BoxCollider>().enabled = false;
                            NumPlus_btn.GetComponent<BoxCollider>().enabled = false;
                            //UI
                            uimanager.ShowNumUI(0, true); //잘했습니다
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
                    holoegg.SetActive(true);    //계란 깨는 위치 표시
                    stay = true;
                    flag = false;
                }
                else
                {
                    if(stay&&holoegg.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        egg_2.SetActive(true);                              //깨진 달걀 보이게
                        egg_1.GetComponent<MeshRenderer>().enabled = false; //원래 달걀 안보이게
                        holoegg.SetActive(false);                           //투명 달걀 위치 비활성화
                                                                            //UI
                        uimanager.ShowNumUI(0, true); //잘했습니다
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
                        //안익은 후라이 활성화
                        fried_1.SetActive(true);
                        uimanager.ShowNextUI();
                        //시간제한 활성화
                        TimeWindow.SetActive(true);
                        ticktok = true;
                        //다음으로
                        Invoke("StayTime", staytime);
                        stay = false;
                    }
                }
                break;
            case CookEggIng.MidFried:
                if(flag)//소금전
                {
                    uimanager.ShowNowUI();
                    salt.GetComponent<Rigidbody>().isKinematic = false;
                    salt.GetComponent<BoxCollider>().enabled = true;
                    stay = true;
                    Invoke("StayTime", staytime);
                    flag = false;
                }
                else//소금후
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
                        Debug.Log("트리거 작동");
                        uimanager.ShowNumUI(0, true); //잘했습니다
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
                    Invoke("CookEggTime", staytime);                    //BrokenEgg로 이동
                    flag = true;
                }
                break;
            case CookEggIng.MoveFried:
                if(flag)
                {
                    //시간제한 비활성화
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
                        uimanager.ShowNumUI(0, true); //잘했습니다
                        Debug.Log("트리거 작동");
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
                        Debug.Log("트리거 작동");
                        uimanager.ShowNumUI(0, true); //잘했습니다
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
                            uimanager.ShowNumUI(0, true); //잘했습니다
                            //Invoke("StayTime", staytime);

                            stay = false;
                        }
                    }
                }
                
                break;
            default:
                break;
        }

        //시간 제한 흐르기
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
            ing = CookEggIng.BurnFried;
        }
    }

    void StayTime()
    {
        Debug.Log("다음 단계:" + (ing + 1));
        ing++;
    }

    void CookEggTime()
    {
        holoegg.GetComponent<GoalInfoObject>().ResetGoalInfoObject();   //투명달걀 정보 초기화
        eggpos.GetComponent<GoalInfoObject>().ResetGoalInfoObject();   //후라이 정보 초기화
        egg_1.GetComponent<OutLineObject>().ResetPostion(); //달걀 원래 자리로 이동
        salt.GetComponent<OutLineObject>().ResetPostion(); //소금 원래 자리로 이동
        turner.GetComponent<OutLineObject>().ResetPostion(); //뒤집개 원래 자리로 이동

        fried_4.SetActive(false);                           //탄 후라이 안보이게
        egg_2.SetActive(false);                             //깨진 달걀 안보이게
        egg_1.GetComponent<MeshRenderer>().enabled = true;  //원래 달걀 보이게
        limit_f = limit;                                    //제한시간 초기화
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