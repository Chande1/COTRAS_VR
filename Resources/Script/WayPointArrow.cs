using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointArrow : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject Manager;
    [SerializeField] Image arrow;
    [SerializeField] Transform target;
    [SerializeField] Text distance;
    [SerializeField] Camera vrcamera;
    RectTransform pointerRectTransform;

    [Header("상시 체크")]
    [SerializeField] bool alltime;


    private void Update()
    {
        if (Manager.GetComponent<FindArriveManager>())
            target = Manager.GetComponent<FindArriveManager>().GetEndPos();
        if (Manager.GetComponent<FindObjectManager>())
            target = Manager.GetComponent<FindObjectManager>().GetEndPos();

        //float minX = arrow.GetPixelAdjustedRect().width / 2;
        //float minY = arrow.GetPixelAdjustedRect().height / 2;
        //float maxX = Screen.width - minX;
        //float maxY = Screen.height - minY;
        //float maxX = canvas.GetComponent<RectTransform>().sizeDelta.x - minX;
        //float maxY = canvas.GetComponent<RectTransform>().sizeDelta.y - minY;

        float minX = (canvas.GetComponent<RectTransform>().sizeDelta.x / 2) * (-1);
        float maxX = canvas.GetComponent<RectTransform>().sizeDelta.x - (minX * -1);
        float minY = (canvas.GetComponent<RectTransform>().sizeDelta.y / 2) * (-1);
        float maxY = canvas.GetComponent<RectTransform>().sizeDelta.y - (minY * -1);


        //Vector2 pos = vrcamera.WorldToScreenPoint(target.localPosition);
        Vector2 pos = vrcamera.WorldToScreenPoint(target.position);
        //Vector2 pos = vrcamera.ScreenToWorldPoint(target.position);
        pos.x -= (minX * -2.4f);
        pos.y -= (minY * -1.2f);

        //Debug.Log("targetpos: " + target.position + "/vrpos:" + pos);

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
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

        //arrow.transform.position = pos;
        arrow.transform.localPosition = pos;

        Vector2 targetpos = vrcamera.WorldToScreenPoint(target.position);
        float angle = Mathf.Atan2(targetpos.y - pos.y, targetpos.x - pos.x) * Mathf.Rad2Deg;
        pointerRectTransform = arrow.GetComponent<RectTransform>();
        //pointerRectTransform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (pos.x < 0)
        {
            if (pos.y < 0)
            {
                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle % 360);
            }
            else
            {
                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle % 360 - 180);
            }
        }
        else
        {
            if (pos.y > 0)
            {
                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle % 360 - 90);
            }
            else
            {
                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle % 360 + 90);
            }
        }


        distance.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";

        Debug.Log("targetpos: " + target.position + "/vrpos:" + pos);
        //Debug.Log("minxy:" + minX + " , " + minY + "/ maxxy:" + maxX + " , " + maxY + "/ mypos:" + pos);
    }
}


