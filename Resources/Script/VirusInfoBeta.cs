using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public enum VirusValue
{
    Red=0,
    Green,
    Blue
}

public class Virus
{
    public int score;       //����    
    public int rate;        //�ҵ���
    public VirusValue value;   //����
}
*/

public class VirusInfoBeta : MonoBehaviour
{
    private Virus v=new Virus();    //���̷��� ���� ����

    [Header("���̷��� ����")]
    public VirusValue vv;

    private void Awake()
    {
        //������ ���̷��� ������ ���� ������ ������ �������� ���̷��� ���� �ʱ�ȭ
        switch(vv)
        {
            case VirusValue.Red:
                v.value = VirusValue.Red;
                v.score = int.Parse(GameObject.Find("R_s").GetComponent<Text>().text);
                v.rate = int.Parse(GameObject.Find("R_r").GetComponent<Text>().text);
                break;
            case VirusValue.Green:
                v.value = VirusValue.Green;
                v.score = int.Parse(GameObject.Find("G_s").GetComponent<Text>().text);
                v.rate = int.Parse(GameObject.Find("G_r").GetComponent<Text>().text);
                break;
            case VirusValue.Blue:
                v.value = VirusValue.Blue;
                v.score = int.Parse(GameObject.Find("B_s").GetComponent<Text>().text);
                v.rate = int.Parse(GameObject.Find("B_r").GetComponent<Text>().text);
                break;
            default:
                break;
        }
        Debug.Log("Virus setting done!"+"�ҵ���: " + v.rate + " �ҵ�����: " + v.score);
    }

    private void OnTriggerStay(Collider other)
    {
        if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) >0||
            OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0)
        {
            Debug.Log("I'm dead!"+other.name + "�ҵ��� " + v.rate + "�� �ҵ����� " + v.score + "��");

            v.rate += int.Parse(GameObject.Find("rate").GetComponent<Text>().text);         //�ҵ��� ���
            GameObject.Find("rate").GetComponent<Text>().text = v.rate.ToString();          //�ҵ���ǥ��
            v.score += int.Parse(GameObject.Find("score").GetComponent<Text>().text);       //���� ���
            GameObject.Find("score").GetComponent<Text>().text = v.score.ToString();        //����ǥ��
            GameObject.Find("gage").GetComponent<Image>().fillAmount -= 0.1f;               //������ ����

            Destroy(gameObject);    //������Ʈ ����
        }
    }
}
