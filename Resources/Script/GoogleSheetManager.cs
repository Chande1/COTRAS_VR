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

	//���̵�� ��� �Է� ���� Ȯ��
	bool SetIDPass()
	{
		name = UsernameInput.text.Trim();
		age = UserAgeInput.text.Trim();

		if (name == "" || age == "") return false;
		else return true;
	}

	public void SetMale()
    {
		gender = "����";
    }

	public void SetFeMale()
	{
		gender = "����";
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
			print("�̸� �Ǵ� ���̰� ����ֽ��ϴ�.");
			return;
		}
		if(gender=="")
        {
			print("������ �������� �ʾҽ��ϴ�.");
			return;
        }
		date = DateTime.Now.ToString("yyyy��MM��dd��dddd");

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
			print("�̸� �Ǵ� ���̰� ����ֽ��ϴ�.");
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
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // �ݵ�� using�� ����Ѵ�
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("���� ������ �����ϴ�.");
		}
	}


	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;

		GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "ERROR")
		{
			print(GD.order + "�� ������ �� �����ϴ�. ���� �޽��� : " + GD.msg);
			return;
		}

		print(GD.order + "�� �����߽��ϴ�. �޽��� : " + GD.msg);

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
