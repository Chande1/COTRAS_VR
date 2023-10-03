using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressButton : MonoBehaviour
{
    [SerializeField] Text MyText;
    [SerializeField] float BlinkSpeed;

    GameObject Center;
    Color mycolor;
    bool flag;

    void Start()
    {
        mycolor = MyText.color;
        flag = true;
        Center = GameObject.Find("CenterEyeAnchor");
        GameObject p= GameObject.Find("Player");
        gameObject.transform.position = new Vector3(p.transform.position.x, p.transform.position.y-0.2f, p.transform.position.z + 0.7f);

    }

    void Update()
    {
        /*글자 움직임*/
        Center = GameObject.Find("CenterEyeAnchor");

        gameObject.transform.parent = Center.transform;
        gameObject.transform.LookAt(Center.transform);
        

        /*글자 깜박임*/
        MyText.color = mycolor;

        if (flag&&mycolor.a>0)
        {
            mycolor.a -= Time.deltaTime * BlinkSpeed;
            if(mycolor.a <= 0)
            {
                flag = false;
                //Debug.Log("1");
            }
        }
        else if(!flag && mycolor.a < 255f/255f)
        {
            mycolor.a += Time.deltaTime * BlinkSpeed;
            if (mycolor.a >= 255/255f)
            {
                flag = true;
            }
        }
  
        if (OVRInput.GetDown(OVRInput.Button.Any)&& !OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)&& !OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SceneManager.LoadScene("menu01");
        }


    }
}
