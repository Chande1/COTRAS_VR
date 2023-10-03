using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInfo
{
    string name;        //이름
    string age;         //나이
    string gender;      //성별
    string registerdate;//가입날짜

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
    string origin_info; //오리지널 유저 정보 리스트

    List<UserInfo> UserInfoList = new List<UserInfo>();

    public void SortUserDataListInfo()
    {
        //클래스에 필드 갯수
        int classinfocount = 4;
        //따옴표 갯수
        int infocount = origin_info.Count(f => f == ',');
        //사용자 수
        int usercount = (infocount + 1) / classinfocount;

        string[] info = origin_info.Split(',');
        int temp = 0;

        Debug.Log("클래스 필드 개수:" + classinfocount + "/ 따옴표 갯수:" + infocount+"/사용자 수:"+usercount+"/정보 수:"+info.Length);

        //사용자 수만큼 반복
        for(int i=0;i<usercount;i++)
        {
            UserInfo tempInfo=new UserInfo();

            //필드 갯수 만큼 반복
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

            Debug.Log(tempInfo.GetUserName() + "의 리스트 저장");
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
