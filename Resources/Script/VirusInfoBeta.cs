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
    public int score;       //점수    
    public int rate;        //소독률
    public VirusValue value;   //종류
}
*/

public class VirusInfoBeta : MonoBehaviour
{
    private Virus v=new Virus();    //바이러스 정보 생성

    [Header("바이러스 종류")]
    public VirusValue vv;

    private void Awake()
    {
        //선택한 바이러스 종류에 따라 점수판 정보를 바탕으로 바이러스 정보 초기화
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
        Debug.Log("Virus setting done!"+"소독률: " + v.rate + " 소독점수: " + v.score);
    }

    private void OnTriggerStay(Collider other)
    {
        if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) >0||
            OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0)
        {
            Debug.Log("I'm dead!"+other.name + "소독률 " + v.rate + "↑ 소독점수 " + v.score + "↑");

            v.rate += int.Parse(GameObject.Find("rate").GetComponent<Text>().text);         //소독률 계산
            GameObject.Find("rate").GetComponent<Text>().text = v.rate.ToString();          //소독률표기
            v.score += int.Parse(GameObject.Find("score").GetComponent<Text>().text);       //점수 계산
            GameObject.Find("score").GetComponent<Text>().text = v.score.ToString();        //점수표기
            GameObject.Find("gage").GetComponent<Image>().fillAmount -= 0.1f;               //게이지 감소

            Destroy(gameObject);    //오브젝트 삭제
        }
    }
}
