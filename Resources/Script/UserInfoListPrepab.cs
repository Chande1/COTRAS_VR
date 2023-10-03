using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoListPrepab : MonoBehaviour
{
    [SerializeField] Text user_num;
    [SerializeField] Text user_name;
    [SerializeField] Text user_gender;
    [SerializeField] Text user_age;
    [SerializeField] Text user_date;
    [SerializeField] UserInfo user_info;


    //유저 정보를 리스트 버튼에 출력
    public void SetUserListInfo(UserInfo _info)
    {
        user_info = _info;
        //user_num.text = _num.ToString();
        user_name.text = user_info.GetUserName();
        user_gender.text = user_info.GetUserGender();
        user_age.text = user_info.GetUserAge();
        user_date.text = user_info.GetUserRegDate();
    }
}
