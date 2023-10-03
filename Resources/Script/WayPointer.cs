using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointer : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image arrow;
    [SerializeField] Transform target;
    [SerializeField] Text distance;
    [SerializeField] Camera vrcamera;
    RectTransform pointerRectTransform;

    [Header("상시 체크")]
    [SerializeField] bool alltime;


    private void Update()
    {
        /*
        //Vector3 pos = vrcamera.WorldToViewportPoint(target.position);
        Vector2 pos = vrcamera.WorldToScreenPoint(target.position);
        
        arrow.GetComponent<RectTransform>().anchoredPosition = pos;

        Debug.Log("targetpos: " + pos + "/arrowlocalpos:" + arrow.GetComponent<RectTransform>().localPosition + "/arrowanchorpos:" + arrow.GetComponent<RectTransform>().anchoredPosition);



        distance.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
       */

        Debug.Log("canvas.x:" + canvas.GetComponent<RectTransform>().sizeDelta.x + "canvas.y"+ canvas.GetComponent<RectTransform>().sizeDelta.y);
        /*
        float minX = (canvas.GetComponent<RectTransform>().sizeDelta.x / 2) * (-1);
        float maxX = canvas.GetComponent<RectTransform>().sizeDelta.x - (minX * -1);
        float minY = (canvas.GetComponent<RectTransform>().sizeDelta.y / 2) * (-1);
        float maxY = canvas.GetComponent<RectTransform>().sizeDelta.y - (minY * -1);
        */
        float minX = 0;
        float maxX = canvas.GetComponent<RectTransform>().sizeDelta.x;
        float minY = 0;
        float maxY = canvas.GetComponent<RectTransform>().sizeDelta.y;

        Vector2 pos = vrcamera.WorldToScreenPoint(target.position);

        Debug.Log("targetpos: " + target.position + "/vrpos:" + pos);

        pos.x *= 1.2f;
        pos.y *= 0.3f;


        if (Vector3.Dot((target.position - vrcamera.transform.position).normalized, transform.forward) < 0)
        {
            if (pos.x < canvas.GetComponent<RectTransform>().sizeDelta.x / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        if (alltime)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        arrow.GetComponent<RectTransform>().anchoredPosition = pos;

        //arrow.transform.localPosition = pos;
        Debug.Log("targetpos: " + pos + "/arrowlocalpos:" + arrow.GetComponent<RectTransform>().localPosition + "/arrowanchorpos:" + arrow.GetComponent<RectTransform>().anchoredPosition);

        pointerRectTransform = arrow.GetComponent<RectTransform>();
        Vector3 toPosition = target.position;
        Vector3 fromPosition = vrcamera.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        //pointerRectTransform.eulerAngles = new Vector3(0, 0, angle);


        

        //Debug.Log("minxy:" + minX + " , " + minY + "/ maxxy:" + maxX + " , " + maxY + "/ mypos:" + pos);
        
    }
}
