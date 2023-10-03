using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanRoomManager : MonoBehaviour
{
    enum IngNum
    {
        TurnOn=0,
        WorkPhone,
        FoldQuilt,
        WorkVacuum
    }

    [Header("�÷��̾�")]
    [SerializeField] GameObject player;
    [Header("ĵ����")]
    [SerializeField] GameObject ResultCanvas;
    [SerializeField] GameObject LightCanvas;
    [SerializeField] GameObject HandCanvas;
    [Header("UI")]
    [SerializeField] GameObject ui_welldone;
    [SerializeField] GameObject ui_turnlight;
    [SerializeField] GameObject ui_workphone;
    [SerializeField] GameObject ui_foldquilt;
    [SerializeField] GameObject ui_workvacuum;
    [Header("��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject AllLight;
    [SerializeField] GameObject LightSwitch;
    [SerializeField] GameObject MultiTab;
    [SerializeField] GameObject Charger;
    [SerializeField] GameObject Phone_off;
    [SerializeField] GameObject Phone_on;
    [SerializeField] GameObject PlaceObject;    //������ ������ ����
    [SerializeField] GameObject QuiltMoveManager;   //�̺� ����
    [SerializeField] GameObject DuestObject;    //���� ����


    [Header("�����Ȳ")]
    [SerializeField] IngNum ingnum;

    bool one;
    bool done;

    private void Awake()
    {
        one = true;
        done = false;
        ingnum = IngNum.TurnOn;

        player = GameObject.Find("Player");
        ResultCanvas = GameObject.Find("ResultCanvas");
        LightCanvas = GameObject.Find("LightCanvas");
        HandCanvas = GameObject.Find("HandCanvas");
        ui_welldone = GameObject.Find("UI_welldone");
        ui_turnlight = GameObject.Find("UI_TurnLight");
        ui_workphone = GameObject.Find("UI_WorkPhone");
        ui_foldquilt = GameObject.Find("UI_FoldQuilt");
        ui_workvacuum = GameObject.Find("UI_WorkVacuum");
        AllLight = GameObject.Find("AllLight");
        PlaceObject = GameObject.Find("PlaceObject");
        QuiltMoveManager = GameObject.Find("QuiltMove");
        DuestObject = GameObject.Find("DuestObject");

        HandCanvas.SetActive(false);
        ui_welldone.SetActive(false);
        ui_workphone.SetActive(false);
        ui_foldquilt.SetActive(false);
        ui_workvacuum.SetActive(false);
        AllLight.SetActive(false);
        Phone_on.SetActive(false);
        PlaceObject.SetActive(false);
        QuiltMoveManager.SetActive(false);
        DuestObject.SetActive(false);
       

        //�÷��̾� ��ġ ���� ��ġ��
        player.transform.position = GameObject.Find("StartPos").transform.position;
        player.transform.eulerAngles = GameObject.Find("StartPos").transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        switch(ingnum)
        {
            case IngNum.TurnOn:
                if(one&&!done)
                {
                    LightSwitch.GetComponent<OutLineObject>().OutLineOn();
                    one = false;
                }
                if(!one&& !LightSwitch.GetComponent<OutLineObject>().OutLineWork())
                {
                    //���� �ѱ�
                    LightCanvas.SetActive(false);
                    AllLight.SetActive(true);
                    //UI��ü
                    ui_turnlight.SetActive(false);
                    //��� �̵�
                    ui_welldone.GetComponent<FadeMoveUI>().ResetFMUI();
                    ui_welldone.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("Pos_1"));
                    ui_welldone.SetActive(true);
                    done = true;
                    one = true;
                }
                if (done&&!ui_welldone.activeSelf)
                {
                    ingnum++;
                }
                    
                break;
            case IngNum.WorkPhone:
                if (one && done)
                {
                    //�ܰ��� Ȱ��
                    Charger.GetComponent<OutLineObject>().OutLineOn();
                    MultiTab.GetComponent<OutLineObject>().OutLineOn();
                    //������Ʈ Ȱ��
                    PlaceObject.SetActive(true);
                    //UIȰ��
                    ui_workphone.SetActive(true);
                    one = false;
                }
                if (!one && done && Charger.GetComponent<OutLineObject>().ObjectArriveGoal())
                {
                    //�ܰ��� ��Ȱ��
                    Charger.GetComponent<OutLineObject>().OutLineOff();
                    MultiTab.GetComponent<OutLineObject>().OutLineOff();
                    //������Ʈ ��Ȱ��
                    Charger.SetActive(false);
                    Phone_off.SetActive(false);
                    PlaceObject.SetActive(false);
                    //������Ʈ Ȱ��
                    Phone_on.SetActive(true);
                    //UI��ü
                    ui_workphone.SetActive(false);
                    //��� �̵�
                    ui_welldone.GetComponent<FadeMoveUI>().ResetFMUI();
                    ui_welldone.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("Pos_2"));
                    ui_welldone.SetActive(true);

                    done = false;
                }
                if (!done && !ui_welldone.activeSelf)
                {
                    ingnum++;
                }
                break;
            case IngNum.FoldQuilt:
                if (!one && !done)
                {
                    //UIȰ��
                    ui_foldquilt.SetActive(true);
                    HandCanvas.SetActive(true);
                    QuiltMoveManager.SetActive(true);
                    one = true;
                }
                if(one&&!done)
                {
                    //�̺� ���� ���� Ȯ��
                    QuiltMoveManager.GetComponent<QuiltManager>().AllQuiltMoveNow();
                    //�̺��� ���� �����ƴ��� Ȯ��
                    if (QuiltMoveManager.GetComponent<QuiltManager>().QuiltClean())
                    {
                        //������Ʈ ��Ȱ��ȭ
                        QuiltMoveManager.SetActive(false);
                        //UI��ü
                        ui_foldquilt.SetActive(false);
                        //��� �̵�
                        ui_welldone.GetComponent<FadeMoveUI>().ResetFMUI();
                        ui_welldone.GetComponent<FadeMoveUI>().InputNextPlace(GameObject.Find("Pos_3"));
                        ui_welldone.SetActive(true);

                        done = true;
                    }
                }
                if (done && !ui_welldone.activeSelf)
                {
                    ingnum++;
                }
                break;
            case IngNum.WorkVacuum:
                if(one&&done)
                {
                    //UIȰ��
                    ui_workvacuum.SetActive(true);
                    //������Ʈ Ȱ��ȭ
                    DuestObject.SetActive(true);
                    DuestObject.GetComponent<DuestObject>().DuestStart();
                    one = false;
                }
                if(!one&&done)
                {
                    DuestObject.GetComponent<DuestObject>().DuestCleanNow();

                    if(DuestObject.GetComponent<DuestObject>().DuestClean())
                    {
                        //������Ʈ ��Ȱ��ȭ
                        DuestObject.SetActive(false);
                        //UI��ü
                        ui_workvacuum.SetActive(false);

                        //�̵� ����
                        ui_welldone.GetComponent<FadeMoveUI>().enabled = false;
                        ui_welldone.SetActive(true);
                        done = false;
                    }
                }
                break;
            default:
                break;
        }
    }
}
