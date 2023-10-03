using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VirusGameManager : MonoBehaviour
{
    [SerializeField]
    bool tpsmode;

    //장소별 소독률 정보
    private int ER; //현관
    private int KR; //부엌
    private int LR; //거실
    private int MR; //안방
    private int WR; //화장실
    //장소별 바이러스 프리팹
    [Header("부엌 바이러스")]
    [SerializeField] private GameObject KV;
    [Header("거실 바이러스")]
    [SerializeField] private GameObject LV;
    [Header("안방 바이러스")]
    [SerializeField] private GameObject MV;
    [Header("화장실 바이러스")]
    [SerializeField] private GameObject WV;

    //색깔별 바이러스 정보
    private Virus RV=new Virus();
    private Virus GV=new Virus();
    private Virus BV=new Virus();
    //판별하는 바이러스 정보
    private Virus v = new Virus();
    //[Header("게이지 감소율")]
    //[SerializeField] float gagecount;//게이지 감소율
    [Header("바이러스 접촉여부")]
    [SerializeField] private string vobject;    //접촉된 바이러스 오브젝트 이름
    [SerializeField] private bool vcheck;            //바이러스 체크
    //장소
    [SerializeField] private PlaceCube nplace;  //현재 장소

    private GameObject Player;  //플레이어
    //플레이어 안내문
    private GameObject Sign;
    private GameObject signtext;
    //플레이어 캔버스
    private GameObject Basic_UI;
    private GameObject Result_UI;

    //플레이어 표지판
    private GameObject psign_S; //시작 표지판
    private GameObject psign_K; //부엌
    private GameObject psign_L; //거실
    private GameObject psign_W; //화장실
    private GameObject psign_M; //안방
    private GameObject psign_done;  //끝

    //아이콘
    private GameObject Sprayicon;   //소독제 아이콘
    private GameObject GBlinkicon;  //게이지 반짝이 아이콘

    //재시작 버튼
    private GameObject restart;
    private bool controlcheck;

    //리필 애니메이션
    private GameObject rf_effect;

    //컨트롤러 정보
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    //이동
    GameObject Startpoint;
    GameObject WBox;

    //바이러스 제거 사운드
    [SerializeField] AudioSource VirusSound;

    private void Start()
    {
        Player.transform.position = Startpoint.transform.position;  //시작 위치로 플레이어 이동
        Player.transform.eulerAngles = Startpoint.transform.eulerAngles;
    }

    void Awake()
    {
        ER = 0;
        KR = 0;
        LR = 0;
        MR = 0;
        WR = 0;
        v.rate = 0;
        v.score = 0;
        /*장소 세팅*/
        nplace = PlaceCube.Entrance;    //처음은 현관
        /*바이러스 정보 세팅*/
        vcheck = false;
        controlcheck = false;
        StartSetting();
        VirusSetting();
    }
    

    void Update()
    {
        
        switch (nplace)
        {
            case PlaceCube.Entrance:

                break;
            case PlaceCube.Kitchen:
                if (KV!=null&&!KV.activeSelf)
                {
                    //플레이어 이동 활성화
                    if (tpsmode)
                        Player.GetComponent<TPSPlayerController>().enabled = true;
                    else
                    {
                        Player.GetComponent<PlayerController>().enabled = true;
                        WBox.SetActive(true);
                    }
                        
                    KV.SetActive(true);
                }
                    

                break;
            case PlaceCube.LivingRoom:
                if (LV != null && !LV.activeSelf)
                {
                    //플레이어 이동 활성화
                    if (tpsmode)
                        Player.GetComponent<TPSPlayerController>().enabled = true;
                    else
                    {
                        Player.GetComponent<PlayerController>().enabled = true;
                        WBox.SetActive(true);
                    }
                    LV.SetActive(true);

                    
                }
                    

                break;
            case PlaceCube.WashRoom:
                if (WV != null && !WV.activeSelf)
                {
                    //플레이어 이동 활성화
                    if (tpsmode)
                        Player.GetComponent<TPSPlayerController>().enabled = true;
                    else
                    {
                        Player.GetComponent<PlayerController>().enabled = true;
                        WBox.SetActive(true);
                    }
                    WV.SetActive(true);
                }
                    

                break;
            case PlaceCube.MainRoom:
                if (MV != null && !MV.activeSelf)
                {
                    //플레이어 이동 활성화
                    if (tpsmode)
                        Player.GetComponent<TPSPlayerController>().enabled = true;
                    else
                    {
                        Player.GetComponent<PlayerController>().enabled = true;
                        WBox.SetActive(true);
                    }
                    MV.SetActive(true);
                }
                break;
            default:
                break;
        }
        
        if (tpsmode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("mouse press check");
                controlcheck = true;
            }

            //스페이스바로 리필
            if (Input.GetKeyDown(KeyCode.Space))
                RefillGage();

        }
        else
        {
            if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))

            {
                //Debug.Log("press check");
                controlcheck = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("mouse press check");
                controlcheck = true;
            }
        }
        
        /*바이러스가 접촉중일 때*/
        if (vcheck&&controlcheck)
        {
            //소독게이지가 부족할 때
            if(GameObject.Find("gage").GetComponent<Image>().fillAmount<0.08f)
            {
                ShowSign("소독 게이지가 부족합니다.\n" +
                    "리필 버튼을 눌러 게이지를 충전하세요.");
                GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 0);  //게이지 반짝이 애니메이션 재생
                Sprayicon.GetComponent<Animator>().SetBool("S_Refill", true); //소독제 애니메이션 재생
                
                vcheck = false;
                controlcheck = false;
            }
            else
            {
                if(GameObject.Find("gage").GetComponent<Image>().fillAmount < 0.45f)
                    GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 1);  //게이지 반짝이 애니메이션 재생
                else if (GameObject.Find("gage").GetComponent<Image>().fillAmount < 0.7f)
                    GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 2);  //게이지 반짝이 애니메이션 재생

                //Debug.Log(vobject + "(" + v.value + ") is dead! 소독률 " + v.rate + "↑ 소독점수 " + v.score + "↑");
                
                GameObject.Find("gage").GetComponent<Image>().fillAmount -= v.rate * 0.01f;      //게이지 감소
                v.rate += int.Parse(GameObject.Find("rate").GetComponent<Text>().text);         //소독률 계산
                GameObject.Find("rate").GetComponent<Text>().text = v.rate.ToString();          //소독률표기
                v.score += int.Parse(GameObject.Find("score").GetComponent<Text>().text);       //점수 계산
                GameObject.Find("score").GetComponent<Text>().text = v.score.ToString();        //점수표기
                //Destroy(GameObject.Find(vobject));
                GameObject.Find(vobject).GetComponent<VirusInfo>().DestoyVirus();
                VirusSound.Play();

                switch (nplace)
                {
                    case PlaceCube.Entrance:
                        ER = v.rate;
                        
                        vobject = "";
                        vcheck = false;
                        break;
                    case PlaceCube.Kitchen:
                        KR = v.rate;
                        if (KR >= 85)
                        {
                            Debug.Log(nplace + " 소독 끝!");

                            /*ShowSign("부엌 소독이 완료되었습니다!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI 리셋
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("L1"));    //다음 이동 좌표 설정
                            psign_done.SetActive(true);                                                     //위에서 설정된 이동 UI 활성화
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI 리셋
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("L1"));    //다음 이동 좌표 설정
                            psign_done.SetActive(true);
                            Result_UI.SetActive(true);

                            //플레이어 이동 비활성화
                            if(tpsmode)
                                Player.GetComponent<TPSPlayerController>().enabled = false;
                            else
                                Player.GetComponent<PlayerController>().enabled = false;
                            
                            //나머지 바이러스 삭제
                            KV.GetComponent<VGroupInfo>().DestroyVGroup();
                        }
                        //Destroy(GameObject.Find(vobject));

                        GameObject.Find(vobject).GetComponent<VirusInfo>().DestoyVirus();
                        vobject = "";
                        vcheck = false;
                        break;
                    case PlaceCube.LivingRoom:
                        LR = v.rate;
                        if (LR >= 85)
                        {
                            Debug.Log(nplace + " 소독 끝!");
                            /*ShowSign("거실 소독이 완료되었습니다!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("W1"));
                            psign_done.SetActive(true);
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI 리셋
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("W1"));    //다음 이동 좌표 설정
                            psign_done.SetActive(true);
                            Result_UI.SetActive(true);

                            if (tpsmode)
                                Player.GetComponent<TPSPlayerController>().enabled = false;
                            else
                                Player.GetComponent<PlayerController>().enabled = false;

                            LV.GetComponent<VGroupInfo>().DestroyVGroup();
                        }
                        //Destroy(GameObject.Find(vobject));
                        GameObject.Find(vobject).GetComponent<VirusInfo>().DestoyVirus();
                        vobject = "";
                        vcheck = false;
                        break;
                    case PlaceCube.WashRoom:
                        WR = v.rate;
                        if (WR >= 85)
                        {
                            Debug.Log(nplace + " 소독 끝!");
                            /*ShowSign("화장실 소독이 완료되었습니다!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("M1"));
                            psign_done.SetActive(true);
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI 리셋
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("M1"));    //다음 이동 좌표 설정
                            psign_done.SetActive(true);
                            Result_UI.SetActive(true);

                            if (tpsmode)
                                Player.GetComponent<TPSPlayerController>().enabled = false;
                            else
                                Player.GetComponent<PlayerController>().enabled = false;

                            WV.GetComponent<VGroupInfo>().DestroyVGroup();

                        }
                        //Destroy(GameObject.Find(vobject));
                        GameObject.Find(vobject).GetComponent<VirusInfo>().DestoyVirus();
                        vobject = "";
                        vcheck = false;
                        break;
                    case PlaceCube.MainRoom:
                        MR = v.rate;
                        if (MR >= 85)
                        {
                            Debug.Log(nplace + " 소독 끝!");
                            //ShowSign("안방 소독이 완료되었습니다!");
                            //Destroy(MV);

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            
                            Result_UI.SetActive(true);


                            if (tpsmode)
                                Player.GetComponent<TPSPlayerController>().enabled = false;
                            else
                                Player.GetComponent<PlayerController>().enabled = false;

                            MV.GetComponent<VGroupInfo>().DestroyVGroup();

                        }
                        //Destroy(GameObject.Find(vobject));
                        GameObject.Find(vobject).GetComponent<VirusInfo>().DestoyVirus();
                        vobject = "";
                        vcheck = false;
                        break;
                    default:
                        break;
                }
            }
        }
        else if(Basic_UI.activeSelf&&controlcheck&&!vcheck&& GameObject.Find("gage").GetComponent<Image>().fillAmount >= 0.08f&& !GameObject.Find(vobject))
        {
            //Debug.Log("거리 알림");
            if (!Sign.activeSelf&&nplace!=PlaceCube.Entrance)
                ShowSign("좀 더 가까이 가서 소독해주세요.");
        }

        //모든 구역이 소독률 85 이상일때
        if(KR>=85&&LR>=85&&MR>=85&&WR>=85)
        {
            //플레이어 움직임 제한
            if (tpsmode)
            {
                ///Player.GetComponent<TPSPlayerController>().enabled = false;
            }
            else
            {
                Player.GetComponent<PlayerController>().enabled = false;
                WBox.SetActive(false);
            }

            /*안내창
            ShowEndSign("전전두엽 3D 게임훈련\n" +
                "훈련이 완료되었습니다.\n수고하셨습니다.\n" +
                "부엌 소독률: " + KR + "(%)\t거실 소독률: " + LR + "(%)\n" +
                "안방 소독률: " + MR + "(%)\t화장실 소독률: " + WR + "(%)\n" +
                "소독 점수: " + v.score + "(점)");
            */
            restart.SetActive(true); //재시작 버튼 활성화
        }
        controlcheck = false;

    }
    


    //플레이어 움직임 활성화
    public void StartGame()
    {
        Debug.Log("Start Game!");
        if(tpsmode)
        {
            psign_S.SetActive(true);
        }
        else
        {
            psign_S.SetActive(true);
            //Player.GetComponent<PlayerController>().enabled = true;
            //WBox.SetActive(true);
        }
        GameObject.Find("StartWindow").SetActive(false);

    }

    //소독률 게이지 리필
    public void RefillGage()
    {
        Debug.Log("RefillGage!");
        Sprayicon.GetComponent<Animator>().SetBool("S_Refill", false); //소독제 애니메이션 비활성화
        GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 3);  //게이지 반짝이 애니메이션 재생
        if(tpsmode)
            rf_effect.GetComponent<Animator>().Play("Refill_Effect");

        GameObject.Find("gage").GetComponent<Image>().fillAmount = 1;
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaceCheck(PlaceCube _place)
    {
        //입장하는 장소 판별
        switch (_place)
        {
            case PlaceCube.Entrance:
                GameObject.Find("rate").GetComponent<Text>().text = ER.ToString();          //소독률표기
                break;
            case PlaceCube.Kitchen:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = KR.ToString();          //소독률표기
                break;
            case PlaceCube.LivingRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = LR.ToString();          //소독률표기
                break;
            case PlaceCube.MainRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = MR.ToString();          //소독률표기
                break;
            case PlaceCube.WashRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = WR.ToString();          //소독률표기
                break;
            default:
                break;
        }


        nplace = _place;//현재 장소 변경
    }

    public void VirusCheck(string _obj,VirusValue _value)
    {
        vobject = _obj; //접촉중인 오브젝트 정보

        //접촉중인 바이러스 판별
        switch(_value)
        {
            case VirusValue.Red:
                v.value = VirusValue.Red;
                v.rate = RV.rate;
                v.score = RV.score;
                vcheck = true;  //접촉확인
                break;
            case VirusValue.Green:
                v.value = VirusValue.Green;
                v.rate = GV.rate;
                v.score = GV.score;
                vcheck = true;  //접촉확인
                break;
            case VirusValue.Blue:
                v.value = VirusValue.Blue ;
                v.rate = BV.rate;
                v.score = BV.score;
                vcheck = true;  //접촉확인
                break;
            default:
                break;
        }

        
    }


    private void ShowSign(string _text)
    {
        Sign.SetActive(true);
        signtext.GetComponent<Text>().text = _text;
        Invoke("StopSign", 3f);   //3초 후에 비활성화
    }

    private void ShowEndSign(string _text)
    {
        Sign.SetActive(true);
        signtext.GetComponent<Text>().text = _text;
    }

    private void StopSign()
    {
        Sign.SetActive(false);
    }

    private void StartSetting()
    {
        Startpoint = GameObject.Find("Startpoint");
        restart = GameObject.Find("Restart");
        restart.SetActive(false);
        Sign = GameObject.Find("Sign");
        signtext = GameObject.Find("signtext");
        Sign.SetActive(false);
        Player = GameObject.Find("Player");
        if (tpsmode)
        {
            Player.GetComponent<TPSPlayerController>().enabled = false;
        }
        else
        {
            Player.GetComponent<PlayerController>().enabled = false;
            WBox = GameObject.Find("WarkBox");
            WBox.SetActive(false);
        }

        //바이러스 비활성화
        KV = GameObject.Find("K_Virus");
        LV = GameObject.Find("L_Virus");
        WV = GameObject.Find("W_Virus");
        MV = GameObject.Find("M_Virus");
        KV.SetActive(false);
        LV.SetActive(false);
        WV.SetActive(false);
        MV.SetActive(false);

        

        //표지판 비활성화
        psign_S = GameObject.Find("UI_Start");
        psign_K = GameObject.Find("UI_K");
        psign_L = GameObject.Find("UI_L");
        psign_W = GameObject.Find("UI_W");
        psign_M = GameObject.Find("UI_M");
        psign_done = GameObject.Find("UI_Done");
        

        //아이콘
        Sprayicon = GameObject.Find("UI_Spray");
        Sprayicon.GetComponent<Animator>().SetBool("S_Refill", false);
        GBlinkicon = GameObject.Find("UI_GBlink");
        GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 3);

        

        if(tpsmode)
        {
            psign_S.GetComponent<FadeMoveUI>().InputFade(GameObject.Find("Fade"));
            psign_done.GetComponent<FadeMoveUI>().InputFade(GameObject.Find("Fade"));

            GameObject.Find("Fade").SetActive(false);
            psign_done.SetActive(false);

            rf_effect = GameObject.Find("Refill_effect");
        }

        psign_S.SetActive(false);
        psign_K.SetActive(false);
        psign_L.SetActive(false);
        psign_W.SetActive(false);
        psign_M.SetActive(false);

        //UI캔버스
        Basic_UI = GameObject.Find("UICanvas");
        Result_UI = GameObject.Find("ResultCanvas");
        Result_UI.SetActive(false);
    }


    private void VirusSetting()
    {
        /*바이러스 정보 세팅*/
        //빨강
        RV.value = VirusValue.Red;
        RV.rate = int.Parse(GameObject.Find("R_r").GetComponent<Text>().text);
        RV.score = int.Parse(GameObject.Find("R_s").GetComponent<Text>().text);
        //초록
        GV.value = VirusValue.Green;
        GV.rate = int.Parse(GameObject.Find("G_r").GetComponent<Text>().text);
        GV.score = int.Parse(GameObject.Find("G_s").GetComponent<Text>().text);
        //파랑
        BV.value = VirusValue.Blue;
        BV.rate = int.Parse(GameObject.Find("B_r").GetComponent<Text>().text);
        BV.score = int.Parse(GameObject.Find("B_s").GetComponent<Text>().text);
    }
}
