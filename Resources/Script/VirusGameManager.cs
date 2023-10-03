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

    //��Һ� �ҵ��� ����
    private int ER; //����
    private int KR; //�ξ�
    private int LR; //�Ž�
    private int MR; //�ȹ�
    private int WR; //ȭ���
    //��Һ� ���̷��� ������
    [Header("�ξ� ���̷���")]
    [SerializeField] private GameObject KV;
    [Header("�Ž� ���̷���")]
    [SerializeField] private GameObject LV;
    [Header("�ȹ� ���̷���")]
    [SerializeField] private GameObject MV;
    [Header("ȭ��� ���̷���")]
    [SerializeField] private GameObject WV;

    //���� ���̷��� ����
    private Virus RV=new Virus();
    private Virus GV=new Virus();
    private Virus BV=new Virus();
    //�Ǻ��ϴ� ���̷��� ����
    private Virus v = new Virus();
    //[Header("������ ������")]
    //[SerializeField] float gagecount;//������ ������
    [Header("���̷��� ���˿���")]
    [SerializeField] private string vobject;    //���˵� ���̷��� ������Ʈ �̸�
    [SerializeField] private bool vcheck;            //���̷��� üũ
    //���
    [SerializeField] private PlaceCube nplace;  //���� ���

    private GameObject Player;  //�÷��̾�
    //�÷��̾� �ȳ���
    private GameObject Sign;
    private GameObject signtext;
    //�÷��̾� ĵ����
    private GameObject Basic_UI;
    private GameObject Result_UI;

    //�÷��̾� ǥ����
    private GameObject psign_S; //���� ǥ����
    private GameObject psign_K; //�ξ�
    private GameObject psign_L; //�Ž�
    private GameObject psign_W; //ȭ���
    private GameObject psign_M; //�ȹ�
    private GameObject psign_done;  //��

    //������
    private GameObject Sprayicon;   //�ҵ��� ������
    private GameObject GBlinkicon;  //������ ��¦�� ������

    //����� ��ư
    private GameObject restart;
    private bool controlcheck;

    //���� �ִϸ��̼�
    private GameObject rf_effect;

    //��Ʈ�ѷ� ����
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;

    //�̵�
    GameObject Startpoint;
    GameObject WBox;

    //���̷��� ���� ����
    [SerializeField] AudioSource VirusSound;

    private void Start()
    {
        Player.transform.position = Startpoint.transform.position;  //���� ��ġ�� �÷��̾� �̵�
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
        /*��� ����*/
        nplace = PlaceCube.Entrance;    //ó���� ����
        /*���̷��� ���� ����*/
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
                    //�÷��̾� �̵� Ȱ��ȭ
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
                    //�÷��̾� �̵� Ȱ��ȭ
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
                    //�÷��̾� �̵� Ȱ��ȭ
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
                    //�÷��̾� �̵� Ȱ��ȭ
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

            //�����̽��ٷ� ����
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
        
        /*���̷����� �������� ��*/
        if (vcheck&&controlcheck)
        {
            //�ҵ��������� ������ ��
            if(GameObject.Find("gage").GetComponent<Image>().fillAmount<0.08f)
            {
                ShowSign("�ҵ� �������� �����մϴ�.\n" +
                    "���� ��ư�� ���� �������� �����ϼ���.");
                GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 0);  //������ ��¦�� �ִϸ��̼� ���
                Sprayicon.GetComponent<Animator>().SetBool("S_Refill", true); //�ҵ��� �ִϸ��̼� ���
                
                vcheck = false;
                controlcheck = false;
            }
            else
            {
                if(GameObject.Find("gage").GetComponent<Image>().fillAmount < 0.45f)
                    GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 1);  //������ ��¦�� �ִϸ��̼� ���
                else if (GameObject.Find("gage").GetComponent<Image>().fillAmount < 0.7f)
                    GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 2);  //������ ��¦�� �ִϸ��̼� ���

                //Debug.Log(vobject + "(" + v.value + ") is dead! �ҵ��� " + v.rate + "�� �ҵ����� " + v.score + "��");
                
                GameObject.Find("gage").GetComponent<Image>().fillAmount -= v.rate * 0.01f;      //������ ����
                v.rate += int.Parse(GameObject.Find("rate").GetComponent<Text>().text);         //�ҵ��� ���
                GameObject.Find("rate").GetComponent<Text>().text = v.rate.ToString();          //�ҵ���ǥ��
                v.score += int.Parse(GameObject.Find("score").GetComponent<Text>().text);       //���� ���
                GameObject.Find("score").GetComponent<Text>().text = v.score.ToString();        //����ǥ��
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
                            Debug.Log(nplace + " �ҵ� ��!");

                            /*ShowSign("�ξ� �ҵ��� �Ϸ�Ǿ����ϴ�!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI ����
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("L1"));    //���� �̵� ��ǥ ����
                            psign_done.SetActive(true);                                                     //������ ������ �̵� UI Ȱ��ȭ
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI ����
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("L1"));    //���� �̵� ��ǥ ����
                            psign_done.SetActive(true);
                            Result_UI.SetActive(true);

                            //�÷��̾� �̵� ��Ȱ��ȭ
                            if(tpsmode)
                                Player.GetComponent<TPSPlayerController>().enabled = false;
                            else
                                Player.GetComponent<PlayerController>().enabled = false;
                            
                            //������ ���̷��� ����
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
                            Debug.Log(nplace + " �ҵ� ��!");
                            /*ShowSign("�Ž� �ҵ��� �Ϸ�Ǿ����ϴ�!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("W1"));
                            psign_done.SetActive(true);
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI ����
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("W1"));    //���� �̵� ��ǥ ����
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
                            Debug.Log(nplace + " �ҵ� ��!");
                            /*ShowSign("ȭ��� �ҵ��� �Ϸ�Ǿ����ϴ�!");
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("M1"));
                            psign_done.SetActive(true);
                            */

                            Basic_UI.SetActive(false);
                            int sa_s = (KR + LR + WR + MR) / 4;
                            Result_UI.GetComponent<ResultUI>().ResetUI(sa_s, v.score, KR, LR, WR, MR);
                            psign_done.GetComponent<FadeMoveUI>().ResetFMUI();                              //UI ����
                            psign_done.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("M1"));    //���� �̵� ��ǥ ����
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
                            Debug.Log(nplace + " �ҵ� ��!");
                            //ShowSign("�ȹ� �ҵ��� �Ϸ�Ǿ����ϴ�!");
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
            //Debug.Log("�Ÿ� �˸�");
            if (!Sign.activeSelf&&nplace!=PlaceCube.Entrance)
                ShowSign("�� �� ������ ���� �ҵ����ּ���.");
        }

        //��� ������ �ҵ��� 85 �̻��϶�
        if(KR>=85&&LR>=85&&MR>=85&&WR>=85)
        {
            //�÷��̾� ������ ����
            if (tpsmode)
            {
                ///Player.GetComponent<TPSPlayerController>().enabled = false;
            }
            else
            {
                Player.GetComponent<PlayerController>().enabled = false;
                WBox.SetActive(false);
            }

            /*�ȳ�â
            ShowEndSign("�����ο� 3D �����Ʒ�\n" +
                "�Ʒ��� �Ϸ�Ǿ����ϴ�.\n�����ϼ̽��ϴ�.\n" +
                "�ξ� �ҵ���: " + KR + "(%)\t�Ž� �ҵ���: " + LR + "(%)\n" +
                "�ȹ� �ҵ���: " + MR + "(%)\tȭ��� �ҵ���: " + WR + "(%)\n" +
                "�ҵ� ����: " + v.score + "(��)");
            */
            restart.SetActive(true); //����� ��ư Ȱ��ȭ
        }
        controlcheck = false;

    }
    


    //�÷��̾� ������ Ȱ��ȭ
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

    //�ҵ��� ������ ����
    public void RefillGage()
    {
        Debug.Log("RefillGage!");
        Sprayicon.GetComponent<Animator>().SetBool("S_Refill", false); //�ҵ��� �ִϸ��̼� ��Ȱ��ȭ
        GBlinkicon.GetComponent<Animator>().SetInteger("GB_Speed", 3);  //������ ��¦�� �ִϸ��̼� ���
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
        //�����ϴ� ��� �Ǻ�
        switch (_place)
        {
            case PlaceCube.Entrance:
                GameObject.Find("rate").GetComponent<Text>().text = ER.ToString();          //�ҵ���ǥ��
                break;
            case PlaceCube.Kitchen:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = KR.ToString();          //�ҵ���ǥ��
                break;
            case PlaceCube.LivingRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = LR.ToString();          //�ҵ���ǥ��
                break;
            case PlaceCube.MainRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = MR.ToString();          //�ҵ���ǥ��
                break;
            case PlaceCube.WashRoom:
                if (!Basic_UI.activeSelf)
                {
                    Result_UI.SetActive(false);
                    Basic_UI.SetActive(true);
                }
                GameObject.Find("rate").GetComponent<Text>().text = WR.ToString();          //�ҵ���ǥ��
                break;
            default:
                break;
        }


        nplace = _place;//���� ��� ����
    }

    public void VirusCheck(string _obj,VirusValue _value)
    {
        vobject = _obj; //�������� ������Ʈ ����

        //�������� ���̷��� �Ǻ�
        switch(_value)
        {
            case VirusValue.Red:
                v.value = VirusValue.Red;
                v.rate = RV.rate;
                v.score = RV.score;
                vcheck = true;  //����Ȯ��
                break;
            case VirusValue.Green:
                v.value = VirusValue.Green;
                v.rate = GV.rate;
                v.score = GV.score;
                vcheck = true;  //����Ȯ��
                break;
            case VirusValue.Blue:
                v.value = VirusValue.Blue ;
                v.rate = BV.rate;
                v.score = BV.score;
                vcheck = true;  //����Ȯ��
                break;
            default:
                break;
        }

        
    }


    private void ShowSign(string _text)
    {
        Sign.SetActive(true);
        signtext.GetComponent<Text>().text = _text;
        Invoke("StopSign", 3f);   //3�� �Ŀ� ��Ȱ��ȭ
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

        //���̷��� ��Ȱ��ȭ
        KV = GameObject.Find("K_Virus");
        LV = GameObject.Find("L_Virus");
        WV = GameObject.Find("W_Virus");
        MV = GameObject.Find("M_Virus");
        KV.SetActive(false);
        LV.SetActive(false);
        WV.SetActive(false);
        MV.SetActive(false);

        

        //ǥ���� ��Ȱ��ȭ
        psign_S = GameObject.Find("UI_Start");
        psign_K = GameObject.Find("UI_K");
        psign_L = GameObject.Find("UI_L");
        psign_W = GameObject.Find("UI_W");
        psign_M = GameObject.Find("UI_M");
        psign_done = GameObject.Find("UI_Done");
        

        //������
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

        //UIĵ����
        Basic_UI = GameObject.Find("UICanvas");
        Result_UI = GameObject.Find("ResultCanvas");
        Result_UI.SetActive(false);
    }


    private void VirusSetting()
    {
        /*���̷��� ���� ����*/
        //����
        RV.value = VirusValue.Red;
        RV.rate = int.Parse(GameObject.Find("R_r").GetComponent<Text>().text);
        RV.score = int.Parse(GameObject.Find("R_s").GetComponent<Text>().text);
        //�ʷ�
        GV.value = VirusValue.Green;
        GV.rate = int.Parse(GameObject.Find("G_r").GetComponent<Text>().text);
        GV.score = int.Parse(GameObject.Find("G_s").GetComponent<Text>().text);
        //�Ķ�
        BV.value = VirusValue.Blue;
        BV.rate = int.Parse(GameObject.Find("B_r").GetComponent<Text>().text);
        BV.score = int.Parse(GameObject.Find("B_s").GetComponent<Text>().text);
    }
}
