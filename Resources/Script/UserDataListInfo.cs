using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInfo
{
    string name;        //�̸�
    string age;         //����
    string gender;      //����
    string registerdate;//���Գ�¥

    public void SetUsername(string _name)
    {
        name = _name;
    }

    public void SetUserAge(string _age)
    {
        age = _age;
    }

    public void SetUserGender(string _gender)
    {
        gender = _gender;
    }

    public void SetRegDate(string _regdate)
    {
        registerdate = _regdate;
    }

    public void SetUserInfo(string _name,string _age,string _gender,string _rdate)
    {
        name = _name;
        age = _age;
        gender = _gender;
        registerdate = _rdate;
    }

    public string GetUserName()
    {
        return name;
    }

    public string GetUserAge()
    {
        return age;
    }

    public string GetUserGender()
    {
        return gender;
    }

    public string GetUserRegDate()
    {
        return registerdate;
    }
}

public class UserDataListInfo : MonoBehaviour
{
    string origin_info; //�������� ���� ���� ����Ʈ

    List<UserInfo> UserInfoList = new List<UserInfo>();

    public void SortUserDataListInfo()
    {
        //Ŭ������ �ʵ� ����
        int classinfocount = 4;
        //����ǥ ����
        int infocount = origin_info.Count(f => f == ',');
        //����� ��
        int usercount = (infocount + 1) / classinfocount;

        string[] info = origin_info.Split(',');
        int temp = 0;

        Debug.Log("Ŭ���� �ʵ� ����:" + classinfocount + "/ ����ǥ ����:" + infocount+"/����� ��:"+usercount+"/���� ��:"+info.Length);

        //����� ����ŭ �ݺ�
        for(int i=0;i<usercount;i++)
        {
            UserInfo tempInfo=new UserInfo();

            //�ʵ� ���� ��ŭ �ݺ�
            for(int j=0;j<classinfocount;j++)
            {
                switch(j)
                {
                    case 0:
                        tempInfo.SetUsername(info[temp]);
                        break;
                    case 1:
                        tempInfo.SetUserAge(info[temp]);
                        break;
                    case 2:
                        tempInfo.SetUserGender(info[temp]);
                        break;
                    case 3:
                        tempInfo.SetRegDate(info[temp]);
                        break;
                    default:
                        break;
                }
                temp++;
            }

            Debug.Log(tempInfo.GetUserName() + "�� ����Ʈ ����");
            UserInfoList.Add(tempInfo);
        }
    }

    public UserInfo GetUserInfoNum(int _num)
    {
        return UserInfoList[_num];
    }

    public int GetUserInfoListCount()
    {
        return UserInfoList.Count;
    }

    public void SetUserListInfo(string _info)
    {
        origin_info = _info;
    }
            
}
