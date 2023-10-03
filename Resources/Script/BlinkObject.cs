using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/*BlinkObject사용법*/
//1. 해당 오브젝트 안에 BlinkObject()와 interactive() 스크립트를 넣어줍니다.
//2. 해당 오브젝트의 static을 언체크 합니다.
//3. 깜빡이지 않는 오브젝트는 스크립트를 통해 notblink를 체크해줍니다.(미리x)


public class BlinkObject : MonoBehaviour
{
    [SerializeField] bool flag;
    [SerializeField] bool outlineflag;
    [SerializeField] float blinkspeed;
    public bool notblink;

    // Start is called before the first frame update
    void Start()
    {
        flag = true;
        outlineflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            if (!notblink)
                Invoke("OutLineTurn", blinkspeed);
            else
                outlineflag = true;

            if (outlineflag)
            {
                gameObject.GetComponent<Interactable>().OutLineObjectOn(); //오브젝트 하이라이트
            }
            else
            {
                gameObject.GetComponent<Interactable>().OutLineObjectOff();
            }

            flag = false;
        }
    }

    public void EndBlink()
    {
        flag = false;

        GameObject.Find(gameObject.name).GetComponent<BlinkObject>().outlineflag = false;
        
        if (GameObject.Find(gameObject.name + "_Highlighter"))
        {
            notblink = false;
            Debug.Log(gameObject.name+ " Highlighter destroy!");
            Destroy(GameObject.Find(gameObject.name + "_Highlighter"));
            //Destroy(GameObject.Find(gameObject.name + "SkinnedHolder")); //해당 부모 하이라이트 오브젝트 삭제
        }
        /*
        if (GameObject.Find("Highlighter"))
        {
            Debug.Log("destroy!");
            Destroy(GameObject.Find("Highlighter"));
        }
        */

        gameObject.GetComponent<BlinkObject>().enabled = false;
    }

    protected void OutLineTurn()
    {
        if (outlineflag)
            outlineflag = false;
        else
            outlineflag = true;
        flag = true;
    }

    public void ReFlag()
    {
        flag=true;
    }

    public void ReBlink()
    {
        flag = true;
        outlineflag = false;
        notblink = false;
    }
}
