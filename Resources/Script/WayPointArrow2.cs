using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointArrow2 : MonoBehaviour
{
    [SerializeField] GameObject Manager;
    [SerializeField]Camera vrcamera;
    [SerializeField] GameObject Arrow;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Text distance;
    [SerializeField] float Angle;
    RectTransform pointerRectTransform;

    private void Awake()
    {
        targetPosition = Manager.GetComponent<FindArriveManager>().GetEndPos().position;
        pointerRectTransform = Arrow.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = Manager.GetComponent<FindArriveManager>().GetEndPos().position;
        Vector3 fromPosition = vrcamera.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle= (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) % 360;
        Angle = angle;
        //pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        pointerRectTransform.eulerAngles = new Vector3(0, 0, angle);
        distance.text = ((int)Vector3.Distance(toPosition, transform.position)).ToString() + "m";
    }
}
