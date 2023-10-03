using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComUIInfo : MonoBehaviour
{

    [SerializeField] GameObject C_gage;
    [SerializeField] GameObject C_rate;
    [SerializeField] GameObject C_score;
    [SerializeField] GameObject C_Sign;
    [SerializeField] Text C_signtext;
    [SerializeField] GameObject C_Restart;

    private void Update()
    {
        C_gage.GetComponent<Image>().fillAmount = GameObject.Find("gage").GetComponent<Image>().fillAmount;
        C_rate.GetComponent<Text>().text= GameObject.Find("rate").GetComponent<Text>().text;
        C_score.GetComponent<Text>().text = GameObject.Find("score").GetComponent<Text>().text;

        if (GameObject.Find("Restart"))
            C_Restart.SetActive(true);
        else
            C_Restart.SetActive(false);


        if (GameObject.Find("Sign"))
        {
            C_Sign.SetActive(true);
            C_signtext.GetComponent<Text>().text = GameObject.Find("signtext").GetComponent<Text>().text;
        }
        else
        {
            C_Sign.SetActive(false);
        }
    }
}
