using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.AI;

public class FadeMoveUI : MonoBehaviour
{
    //[SerializeField] Image MyImage;
    //[SerializeField] float BlinkSpeed;
    [SerializeField] bool TPSmode;
    [SerializeField] bool checkbutton;
    [SerializeField] GameObject nextplace;
    [SerializeField] float inoutTime = 1f;  //페이드 인/아웃 시간
    [SerializeField] float fadeTime = 2f;   //페이드 인 대기 시간
    [SerializeField] float stopTime = 3f;   //페이드 아웃 대기 시간
    
    Color mycolor;
    [SerializeField]bool flag;
    GameObject player;
    bool fade;
    bool movedone;
    [SerializeField]GameObject fadeobj;
    [SerializeField] GameObject fadetext;
    Image fadeimg;

    void Start()
    {
        player = GameObject.Find("Player");
        if (TPSmode)
        {
            if (fadeobj != null && fadeobj.activeSelf)
                fadeobj.SetActive(false);
        }

        //mycolor = MyImage.color;
        if(checkbutton)
        {
            flag = true;
            fade = false;
            movedone = false;
        }
        else
        {
            flag = false;
            fade = false;
            movedone = false;
        }
    }

    void Update()
    {
        if(!flag)
        {
            if(TPSmode)
            {
                fadeobj.SetActive(true);
                Invoke("TPS_FTB", stopTime);
                flag = true;
                Debug.Log("startfade");
            }
            else
            {
                if (gameObject.GetComponent<Button>() != null)
                    gameObject.GetComponent<Button>().interactable = false;
                Invoke("StopTime", stopTime);
                flag = true;
            }
           
        }
        if(fade)
        {
            if (TPSmode)
            {
                //플레이어 위치
                player.GetComponent<NavMeshAgent>().Warp(nextplace.transform.position);  //시작 위치로 플레이어 이동
                player.transform.eulerAngles = nextplace.transform.eulerAngles;
                Debug.Log(nextplace + "로 이동 fading...");

                gameObject.GetComponent<Image>().enabled = false;
                if(fadetext)
                    fadetext.SetActive(false);   //텍스트

                Invoke("TPS_FFB", fadeTime);
                fade = false;
            }
            else
            {
                //플레이어 위치
                if(player.GetComponent<NavMeshAgent>()!=null)
                    player.GetComponent<NavMeshAgent>().Warp(nextplace.transform.position);  //시작 위치로 플레이어 이동
                else
                    player.transform.position = nextplace.transform.position;  //시작 위치로 플레이어 이동
                player.transform.eulerAngles = nextplace.transform.eulerAngles;
                Debug.Log(nextplace + "로 이동 fading...");

                Invoke("FadeFromBlack", inoutTime);
                fade = false;
            }
           
        }
        if(movedone)
        {
            if (TPSmode&&fadeobj.activeSelf)
            {
                Debug.Log("fadedone");
                fadeobj.SetActive(false);
                gameObject.GetComponent<Image>().enabled = true;
                if (fadetext)
                    fadetext.SetActive(true);    //텍스트
            }

            
            

            Debug.Log("fadedone");
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }

        /*이미지 깜박임(flag다른걸로 대체해야함)
        MyImage.color = mycolor;

        if (flag && mycolor.a > 0)
        {
            mycolor.a -= Time.deltaTime * BlinkSpeed;
            if (mycolor.a <= 0)
            {
                flag = false;
                //Debug.Log("1");
            }
        }
        else if (!flag && mycolor.a < 255f / 255f)
        {
            mycolor.a += Time.deltaTime * BlinkSpeed;
            if (mycolor.a >= 255 / 255f)
            {
                flag = true;
            }
        }
        */

    }

    public void CheckButton()
    {
        flag = false;
    }

    public void ResetFMUI()
    {
        if (checkbutton)
            flag = true;
        else
            flag = false;

        fade = false;
        movedone = false;
    }

    //페이드용(VR)
    private void FadeToBlack() //원래화면->검은 화면
    {
        SteamVR_Fade.View(Color.clear, 0f);
        SteamVR_Fade.View(Color.black, inoutTime);    //인아웃 시간 설정

        Invoke("CountTime", fadeTime);  //인아웃 대기 시간
        Debug.Log("fadeout");

    }
    private void FadeFromBlack()   //검은화면->원래화면
    {
        SteamVR_Fade.View(Color.black, 0f);
        SteamVR_Fade.View(Color.clear, inoutTime);    //인아웃 시간 설정
        movedone = true;

        if (gameObject.GetComponent<Button>() != null)
            gameObject.GetComponent<Button>().interactable = true;

        Debug.Log("fadein");
    }

    private void CountTime()
    {
        fade = true;
    }

    private void StopTime()
    {
        Debug.Log("stoptime");
        FadeToBlack();
    }

    //페이드용(TPS)
    private void TPS_FTB()
    {
        StartCoroutine(FadeToBlack_TPS(inoutTime));    //2초 동안 fadeout
        
    }

    private void TPS_FFB()
    {
        StartCoroutine(FadeFromBlack_TPS(inoutTime));  //2초동안 fadein
    }

    IEnumerator FadeToBlack_TPS(float _time)
    {
        Color color = fadeimg.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / _time;
            fadeimg.color = color;
            yield return null;
        }

        if (color.a > 1)
        {
            fade = true;
        }
    }
    IEnumerator FadeFromBlack_TPS(float _time)
    {
        Color color = fadeimg.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / _time;
            fadeimg.color = color;
            yield return null;
        }

        if (color.a<=1)
        {
            movedone = true;
        }
    }

    //이동할 위치
    public void InputNextPlace(GameObject _np)
    {
        Debug.Log("다음장소:" + _np);
        nextplace = _np;
    }

    //fade미리 받기
    public void InputFade(GameObject _fadeobj)
    {
        fadeobj = _fadeobj;
        fadeimg = _fadeobj.GetComponent<Image>();
    }
}
