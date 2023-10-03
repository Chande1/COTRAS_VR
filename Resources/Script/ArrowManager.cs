using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class ArrowManager : MonoBehaviour
{
    enum GameState
    {
        NoGame,
        StartGame,
        FreeGame
    }

    [Header("���� ����")]
    [SerializeField] GameState state;

    [Header("���� ����")]
    [SerializeField] Transform startpos;

    [Header("�÷��̾�")]
    [SerializeField] GameObject player;
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;
    [SerializeField] GameObject vrpointer;

    [Header("����")]
    [SerializeField]Text score;
    [SerializeField] int temp_score;

    [Header("�ð�����")]
    [SerializeField] Text time;
    [SerializeField] float limit_time;
    [SerializeField] float temp_time;

    [Header("�ȳ�â")]
    [SerializeField] GameObject start_panel;

    [Header("���â")]
    [SerializeField] GameObject result_panel;
    [SerializeField] Text result_time;
    [SerializeField] Text result_score;

    bool start;

    // Start is called before the first frame update
    void Awake()
    {
        player.transform.position = startpos.transform.position;  //���� ��ġ�� �÷��̾� �̵�
        player.transform.eulerAngles = startpos.transform.eulerAngles;
        vrpointer.SetActive(false);
        result_panel.SetActive(false);
        start = false;
        temp_time = limit_time;    
    }

    // Update is called once per frame
    void Update()
    {
       switch(state)
        {
            case GameState.NoGame:

                break;
            case GameState.FreeGame:

                break;
            case GameState.StartGame:
                if (!start)
                {
                    //Ʈ���� �۵���
                    if (RHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
                        LHand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        if (start_panel.activeSelf)
                            start_panel.SetActive(false);

                      

                        start = true;
                    }
                }
                else
                {
                    if(temp_score>=100)
                    {
                        Debug.Log("ȭ���� ��!");
                        ResultInfo();
                        result_panel.SetActive(true);
                    }
                    else
                    {
                        ClockWork();    //�ð��۵�
                    }
                }
                break;
            default:
                break;
        }

        if(result_panel.activeSelf)
        {
            vrpointer.SetActive(true);
        }
    }

    //�ð� �۵�
    void ClockWork()
    {
        temp_time -= Time.deltaTime;
        time.text = Mathf.Round(temp_time).ToString();
        //float temp = 1000 / limit_time;
        //clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //�ð��ʰ� 0�� �Ǹ�
        if (Mathf.Round(temp_time) == 0)
        {
            ResultInfo();
            result_panel.SetActive(true);   
        }
    }

    //���â ǥ��
    void ResultInfo()
    {
        result_time.text = Mathf.Round(temp_time).ToString();
        result_score.text = temp_score.ToString();
    }

    public void AddScore(int _addscore)
    {
        temp_score += _addscore;
        score.text = temp_score.ToString();
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
