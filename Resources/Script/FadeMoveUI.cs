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
    [SerializeField] float inoutTime = 1f;  //���̵� ��/�ƿ� �ð�
    [SerializeField] float fadeTime = 2f;   //���̵� �� ��� �ð�
    [SerializeField] float stopTime = 3f;   //���̵� �ƿ� ��� �ð�
    
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
                //�÷��̾� ��ġ
                player.GetComponent<NavMeshAgent>().Warp(nextplace.transform.position);  //���� ��ġ�� �÷��̾� �̵�
                player.transform.eulerAngles = nextplace.transform.eulerAngles;
                Debug.Log(nextplace + "�� �̵� fading...");

                gameObject.GetComponent<Image>().enabled = false;
                if(fadetext)
                    fadetext.SetActive(false);   //�ؽ�Ʈ

                Invoke("TPS_FFB", fadeTime);
                fade = false;
            }
            else
            {
                //�÷��̾� ��ġ
                if(player.GetComponent<NavMeshAgent>()!=null)
                    player.GetComponent<NavMeshAgent>().Warp(nextplace.transform.position);  //���� ��ġ�� �÷��̾� �̵�
                else
                    player.transform.position = nextplace.transform.position;  //���� ��ġ�� �÷��̾� �̵�
                player.transform.eulerAngles = nextplace.transform.eulerAngles;
                Debug.Log(nextplace + "�� �̵� fading...");

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
                    fadetext.SetActive(true);    //�ؽ�Ʈ
            }

            
            

            Debug.Log("fadedone");
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }

        /*�̹��� ������(flag�ٸ��ɷ� ��ü�ؾ���)
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

    //���̵��(VR)
    private void FadeToBlack() //����ȭ��->���� ȭ��
    {
        SteamVR_Fade.View(Color.clear, 0f);
        SteamVR_Fade.View(Color.black, inoutTime);    //�ξƿ� �ð� ����

        Invoke("CountTime", fadeTime);  //�ξƿ� ��� �ð�
        Debug.Log("fadeout");

    }
    private void FadeFromBlack()   //����ȭ��->����ȭ��
    {
        SteamVR_Fade.View(Color.black, 0f);
        SteamVR_Fade.View(Color.clear, inoutTime);    //�ξƿ� �ð� ����
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

    //���̵��(TPS)
    private void TPS_FTB()
    {
        StartCoroutine(FadeToBlack_TPS(inoutTime));    //2�� ���� fadeout
        
    }

    private void TPS_FFB()
    {
        StartCoroutine(FadeFromBlack_TPS(inoutTime));  //2�ʵ��� fadein
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

    //�̵��� ��ġ
    public void InputNextPlace(GameObject _np)
    {
        Debug.Log("�������:" + _np);
        nextplace = _np;
    }

    //fade�̸� �ޱ�
    public void InputFade(GameObject _fadeobj)
    {
        fadeobj = _fadeobj;
        fadeimg = _fadeobj.GetComponent<Image>();
    }
}
