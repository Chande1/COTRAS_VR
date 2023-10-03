using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;
}


public class GoogleSheetManager : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbztAvv3PbtxyaFSTBqc42tAiM5_Nt0NmtxaI7pYz8kPd_yGt5zp3sRWFKncYcubmMMUEA/exec";
    public GoogleData GD;
    public InputField UsernameInput, UserAgeInput, ValueInput;
	public Text userinfo;
    string name, age,gender,date;

	//아이디와 비번 입력 유무 확인
	bool SetIDPass()
	{
		name = UsernameInput.text.Trim();
		age = UserAgeInput.text.Trim();

		if (name == "" || age == "") return false;
		else return true;
	}

	public void SetMale()
    {
		gender = "남자";
    }

	public void SetFeMale()
	{
		gender = "여자";
	}

	public void RegisterInfoReset()
    {
		UsernameInput.text = "";
		UserAgeInput.text = "";
		name = "";
		age = "";
		gender = "";
		date = "";
    }

	public void Register()
	{
		if (!SetIDPass())
		{
			print("이름 또는 나이가 비어있습니다.");
			return;
		}
		if(gender=="")
        {
			print("성별을 선택하지 않았습니다.");
			return;
        }
		date = DateTime.Now.ToString("yyyy년MM월dd일dddd");

		WWWForm form = new WWWForm();
		form.AddField("order", "register");
		form.AddField("name", name);
		form.AddField("age", age);
		form.AddField("gender", gender);
		form.AddField("date", date);

		StartCoroutine(Post(form));
	}


	public void Login()
	{
		if (!SetIDPass())
		{
			print("이름 또는 나이가 비어있습니다.");
			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("order", "login");
		form.AddField("name", name);
		form.AddField("age", age);

		StartCoroutine(Post(form));
	}


	void OnApplicationQuit()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "logout");

		StartCoroutine(Post(form));
	}


	public void SetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "setValue");
		form.AddField("value", ValueInput.text);

		StartCoroutine(Post(form));
	}


	public void GetValue()
	{
		WWWForm form = new WWWForm();
		form.AddField("order", "getValue");

		StartCoroutine(Post(form));
	}

	public void GetUserListInfo()
    {
		WWWForm form = new WWWForm();
		form.AddField("order", "getUserListInfo");

		StartCoroutine(Post(form));
	}



	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("웹의 응답이 없습니다.");
		}
	}


	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "ERROR")
		{
			print(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
			return;
		}

		print(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

		if (GD.order == "getValue")
		{
			ValueInput.text = GD.value;
		}

		if(GD.order=="getUserListInfo")
        {
			//userinfo.text = GD.value;
			GetComponent<UserDataListInfo>().SetUserListInfo(GD.value);
        }

	}
}
