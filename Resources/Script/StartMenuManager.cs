using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
   [Header("������â ������Ʈ")]
   [SerializeField] GameObject StartWind;
   [SerializeField] GameObject SelectWind;
   [SerializeField] GameObject RegisterWind;
   [SerializeField] GameObject RCheckWind;

    [Header("���� ������ ����Ʈ ������")]
    [SerializeField] GameObject UserInfoPrefab;
    [Header("��ũ�� ������ ������Ʈ")]
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
        if(GetComponent<GoogleSheetManager>().GD.msg=="ȸ������ �Ϸ�")
        {
            RCheckWindOn();
        }
        if(!flag&&GetComponent<GoogleSheetManager>().GD.msg== "����� ���� ����Ʈ �ҷ����� �Ϸ�")
        {
            GetComponent<UserDataListInfo>().SortUserDataListInfo();    //������ ���� �� ����Ʈ�� ����
            //����Ʈ ����ŭ �ݺ�
            for(int i=0;i< GetComponent<UserDataListInfo>().GetUserInfoListCount();i++)
            {
                //����Ʈ ��ư ����
                GameObject userinfo = Instantiate(UserInfoPrefab, transform.position, Quaternion.identity);
                userinfo.transform.SetParent(Contents.transform);
                userinfo.GetComponent<UserInfoListPrepab>().SetUserListInfo(GetComponent<UserDataListInfo>().GetUserInfoNum(i));    //���� ���� �Ѱ��ֱ�
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
