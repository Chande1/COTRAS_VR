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

    [Header("플레이어")]
    [SerializeField] GameObject player;
    [Header("캔버스")]
    [SerializeField] GameObject ResultCanvas;
    [SerializeField] GameObject LightCanvas;
    [SerializeField] GameObject HandCanvas;
    [Header("UI")]
    [SerializeField] GameObject ui_welldone;
    [SerializeField] GameObject ui_turnlight;
    [SerializeField] GameObject ui_workphone;
    [SerializeField] GameObject ui_foldquilt;
    [SerializeField] GameObject ui_workvacuum;
    [Header("상호작용 오브젝트")]
    [SerializeField] GameObject AllLight;
    [SerializeField] GameObject LightSwitch;
    [SerializeField] GameObject MultiTab;
    [SerializeField] GameObject Charger;
    [SerializeField] GameObject Phone_off;
    [SerializeField] GameObject Phone_on;
    [SerializeField] GameObject PlaceObject;    //충전기 리스폰 범위
    [SerializeField] GameObject QuiltMoveManager;   //이불 관리
    [SerializeField] GameObject DuestObject;    //먼지 관리


    [Header("진행상황")]
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
       

        //플레이어 위치 시작 위치로
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
                    //전등 켜기
                    LightCanvas.SetActive(false);
                    AllLight.SetActive(true);
                    //UI교체
                    ui_turnlight.SetActive(false);
                    //장소 이동
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
                    //외각선 활성
                    Charger.GetComponent<OutLineObject>().OutLineOn();
                    MultiTab.GetComponent<OutLineObject>().OutLineOn();
                    //오브젝트 활성
                    PlaceObject.SetActive(true);
                    //UI활성
                    ui_workphone.SetActive(true);
                    one = false;
                }
                if (!one && done && Charger.GetComponent<OutLineObject>().ObjectArriveGoal())
                {
                    //외각선 비활성
                    Charger.GetComponent<OutLineObject>().OutLineOff();
                    MultiTab.GetComponent<OutLineObject>().OutLineOff();
                    //오브젝트 비활성
                    Charger.SetActive(false);
                    Phone_off.SetActive(false);
                    PlaceObject.SetActive(false);
                    //오브젝트 활성
                    Phone_on.SetActive(true);
                    //UI교체
                    ui_workphone.SetActive(false);
                    //장소 이동
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
                    //UI활성
                    ui_foldquilt.SetActive(true);
                    HandCanvas.SetActive(true);
                    QuiltMoveManager.SetActive(true);
                    one = true;
                }
                if(one&&!done)
                {
                    //이불 정리 동작 확인
                    QuiltMoveManager.GetComponent<QuiltManager>().AllQuiltMoveNow();
                    //이불이 전부 정리됐는지 확인
                    if (QuiltMoveManager.GetComponent<QuiltManager>().QuiltClean())
                    {
                        //오브젝트 비활성화
                        QuiltMoveManager.SetActive(false);
                        //UI교체
                        ui_foldquilt.SetActive(false);
                        //장소 이동
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
                    //UI활성
                    ui_workvacuum.SetActive(true);
                    //오브젝트 활성화
                    DuestObject.SetActive(true);
                    DuestObject.GetComponent<DuestObject>().DuestStart();
                    one = false;
                }
                if(!one&&done)
                {
                    DuestObject.GetComponent<DuestObject>().DuestCleanNow();

                    if(DuestObject.GetComponent<DuestObject>().DuestClean())
                    {
                        //오브젝트 비활성화
                        DuestObject.SetActive(false);
                        //UI교체
                        ui_workvacuum.SetActive(false);

                        //이동 제한
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
