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
    [SerializeField] float inoutTime = 1f;  //���̵� ��/�ƿ� �ð�
    [SerializeField] float fadeTime = 2f;   //���̵� �� ��� �ð�
    [SerializeField] float stopTime = 3f;   //���̵� �ƿ� ��� �ð�

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
           //�÷��̾� ��ġ
           player.transform.position = nextplace.transform.position;  //���� ��ġ�� �÷��̾� �̵�
           player.transform.eulerAngles = nextplace.transform.eulerAngles;
           Debug.Log(nextplace + "�� �̵� fading...");

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

    //�ܺ� ���̵� ���ۿ� �Լ�
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

    //�̵��� ��ġ
    public void InputNextPlace(GameObject _np)
    {
        Debug.Log("�������:" + _np);
        nextplace = _np;
    }
}
