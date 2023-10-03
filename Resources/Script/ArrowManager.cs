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

    [Header("게임 상태")]
    [SerializeField] GameState state;

    [Header("시작 지점")]
    [SerializeField] Transform startpos;

    [Header("플레이어")]
    [SerializeField] GameObject player;
    [SerializeField] Hand RHand;
    [SerializeField] Hand LHand;
    [SerializeField] GameObject vrpointer;

    [Header("점수")]
    [SerializeField]Text score;
    [SerializeField] int temp_score;

    [Header("시간제한")]
    [SerializeField] Text time;
    [SerializeField] float limit_time;
    [SerializeField] float temp_time;

    [Header("안내창")]
    [SerializeField] GameObject start_panel;

    [Header("결과창")]
    [SerializeField] GameObject result_panel;
    [SerializeField] Text result_time;
    [SerializeField] Text result_score;

    bool start;

    // Start is called before the first frame update
    void Awake()
    {
        player.transform.position = startpos.transform.position;  //시작 위치로 플레이어 이동
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
                    //트리거 작동시
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
                        Debug.Log("화살쏘기 끝!");
                        ResultInfo();
                        result_panel.SetActive(true);
                    }
                    else
                    {
                        ClockWork();    //시간작동
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

    //시계 작동
    void ClockWork()
    {
        temp_time -= Time.deltaTime;
        time.text = Mathf.Round(temp_time).ToString();
        //float temp = 1000 / limit_time;
        //clock.fillAmount += (Time.deltaTime * temp * 0.001f);

        //시간초가 0이 되면
        if (Mathf.Round(temp_time) == 0)
        {
            ResultInfo();
            result_panel.SetActive(true);   
        }
    }

    //결과창 표시
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
