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
    [SerializeField] GameObject package_t;  //봉지 위
    [SerializeField] GameObject package_b;  //봉지 아래
    [SerializeField] GameObject oil;        //기름

    [Header("추가 상호작용 오브젝트")]
    [SerializeField] GameObject fanfire;
    [SerializeField] GameObject egg_oil;    //후라이팬 기름
    [SerializeField] GameObject OnOff_btn;  //전원버튼
    [SerializeField] GameObject OnOff_txt;  //전원글자
    [SerializeField] GameObject NumMinus_btn;   //감소버튼
    [SerializeField] GameObject NumPlus_btn;    //증가버튼
    [SerializeField] GameObject Num_1;          //1
    [SerializeField] GameObject Num_2;
    [SerializeField] GameObject Num_3;
    [SerializeField] GameObject Num_4;
    [SerializeField] GameObject Num_5;
    [SerializeField] GameObject[] ham;

    [Header("오브젝트 상호작용 위치")]
    [SerializeField] GameObject Startpoint;
    [SerializeField] GameObject firemidpos;
    [SerializeField] GameObject oilinpos;
    [SerializeField] GameObject hampos;
    [SerializeField] GameObject friedpos;
    [SerializeField] GameObject midpos;

    [Header("이펙트")]
    [SerializeField] GameObject circle1_eff;    //큰원 효과
    [SerializeField] GameObject circle2_eff;    //작은원 효과
    [SerializeField] GameObject star_eff;       //별 효과
    [SerializeField] GameObject steam_eff;

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
            case CookingHam.Start:
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
                        ing = CookingHam.MoveFryfan;
                    }
                }
                break;
            case CookingHam.MoveFryfan:
                if (flag)
                {
                    uimanager.ShowNextUI(); //다음 UI
                    fryingfan.GetComponent<BoxCollider>().enabled = true;
                    fryingfan.GetComponent<Rigidbody>().isKinematic = false;
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
                        if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength()==1)
                        {
                            uimanager.ShowResultUI(0);  //잘했습니다
                            score+=3;    //점수 추가

                            //정답 이펙트
                            GameObject o_effect = GameObject.Instantiate(star_eff, firemidpos.transform.position, firemidpos.transform.rotation);
                            o_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                            Destroy(o_effect, 2f);                          //2초 후에 삭제

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 2)
                        {
                            uimanager.ShowResultUI(1);  //중앙
                            score += 2;    //점수 추가

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                        else if (firemidpos.GetComponent<GoalInfoObject>().GetMarginLength() == 3)
                        {
                            uimanager.ShowResultUI(1);  //중앙
                            score += 1;    //점수 추가

                            Invoke("StayTime", staytime);
                            stay = true;
                        }
                    }
                }
                break;
            case CookingHam.WorkFire:
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
                    uimanager.ShowNowUI();
                    OnOff_btn.SetActive(true);
                    flag = true;
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
                        Debug.Log("트리거 작동");
                        //egg_oil.SetActive(true);    //후라이팬 기름 추가
                        oilinpos.SetActive(false);
                        //fanfire.SetActive(false);   //후라이팬 열
                        uimanager.ShowResultUI(0);  //잘했습니다
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
            case CookingHam.OpenPackage:
                if (flag)
                {
                    uimanager.ShowNowUI();

                    //봉지
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
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                        uimanager.ShowResultUI(0);  //잘했습니다
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
                            steam_eff.SetActive(true);   //후라이팬 열
                        }
                       
                        //햄이 다 익으면
                        if (ham[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Ham_cook")&&
                            ham[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0f)
                        {
                            Debug.Log("햄이 모두 익었습니다!");
                            uimanager.ShowResultUI(0);  //잘 익었습니다
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
            case CookingHam.MoveHam:
                if (flag)
                {
                    //시간제한 활성화
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
                    
                    //ClockWork();    //시간초

                    if (stay && friedpos.GetComponent<GoalInfoObject>().GetTouch())
                    {
                        Debug.Log("접시와 접촉!");
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
                            uimanager.ShowResultUI(0);  //잘했습니다
                            ticktok = false;
                            Debug.Log("트리거 작동");
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

        //시간 제한 흐르기
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