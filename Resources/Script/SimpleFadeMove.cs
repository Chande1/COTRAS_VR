using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.AI;

public class SimpleFadeMove : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] bool checkbutton;
    [SerializeField] GameObject nextplace;
    [SerializeField] float inoutTime = 1f;  //페이드 인/아웃 시간
    [SerializeField] float fadeTime = 2f;   //페이드 인 대기 시간
    [SerializeField] float stopTime = 3f;   //페이드 아웃 대기 시간

    [SerializeField] bool flag;
    bool fade;
    bool movedone;

    void Start()
    {
        //mycolor = MyImage.color;
        if (checkbutton)
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
        if (!flag)
        {
            Invoke("StopTime", stopTime);
            flag = true;
        }
        if (fade)
        {
           //플레이어 위치
           player.transform.position = nextplace.transform.position;  //시작 위치로 플레이어 이동
           player.transform.eulerAngles = nextplace.transform.eulerAngles;
           Debug.Log(nextplace + "로 이동 fading...");

           Invoke("FadeFromBlack", inoutTime);
           fade = false;
        }
        if (movedone)
        {
            gameObject.GetComponent<SimpleFadeMove>().enabled = false;
            //gameObject.SetActive(false);
            //Destroy(gameObject);
        }


    }

    //외부 페이드 시작용 함수
    public void CheckButton()
    {
        flag = false;
    }

    public bool MoveDone()
    {
        if (movedone)
            return true;
        else
            return false;
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

    //이동할 위치
    public void InputNextPlace(GameObject _np)
    {
        Debug.Log("다음장소:" + _np);
        nextplace = _np;
    }
}
