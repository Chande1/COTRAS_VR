using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
   [Header("윈도우창 오브젝트")]
   [SerializeField] GameObject StartWind;
   [SerializeField] GameObject SelectWind;
   [SerializeField] GameObject RegisterWind;
   [SerializeField] GameObject RCheckWind;

    [Header("유저 데이터 리스트 프리팹")]
    [SerializeField] GameObject UserInfoPrefab;
    [Header("스크롤 컨텐츠 오브젝트")]
    [SerializeField] GameObject Contents;
    string userinfodata;
    bool flag;

    private void Awake()
    {
        flag = false;

        StartWind = GameObject.Find("StartWindow");
        SelectWind = GameObject.Find("SelectWindow");
        RegisterWind = GameObject.Find("RegisterWindow");
        RCheckWind = GameObject.Find("RegisterCheck_W");

        RCheckWind.SetActive(false);
        SelectWind.SetActive(false);
        RegisterWind.SetActive(false);
    }

    private void Update()
    {
        if(GetComponent<GoogleSheetManager>().GD.msg=="회원가입 완료")
        {
            RCheckWindOn();
        }
        if(!flag&&GetComponent<GoogleSheetManager>().GD.msg== "사용자 선택 리스트 불러오기 완료")
        {
            GetComponent<UserDataListInfo>().SortUserDataListInfo();    //데이터 나열 후 리스트에 정리
            //리스트 수만큼 반복
            for(int i=0;i< GetComponent<UserDataListInfo>().GetUserInfoListCount();i++)
            {
                //리스트 버튼 생성
                GameObject userinfo = Instantiate(UserInfoPrefab, transform.position, Quaternion.identity);
                userinfo.transform.SetParent(Contents.transform);
                userinfo.GetComponent<UserInfoListPrepab>().SetUserListInfo(GetComponent<UserDataListInfo>().GetUserInfoNum(i));    //유저 정보 넘겨주기
            }

            flag = true;
        }
    }

    public void RCheckWindOn()
    {
        RCheckWind.SetActive(true);
    }

    public void RCheckWindOff()
    {
        RCheckWind.SetActive(false);
    }

    public void SelectWindOn()
    {
        StartWind.SetActive(false);
        SelectWind.SetActive(true);
        RegisterWind.SetActive(false);

        flag = false;
        GetComponent<GoogleSheetManager>().GetUserListInfo();
    }

    public void StartWindOn()
    {
        if(RCheckWind.activeSelf)
        {
            RCheckWind.SetActive(false);
            GetComponent<GoogleSheetManager>().RegisterInfoReset();
            GetComponent<GoogleSheetManager>().GD.msg = "";
        }
        
        StartWind.SetActive(true);
        SelectWind.SetActive(false);
        RegisterWind.SetActive(false);
    }

    public void RegisterWindOn()
    {
        StartWind.SetActive(false);
        SelectWind.SetActive(false);
        RegisterWind.SetActive(true);
    }
}
