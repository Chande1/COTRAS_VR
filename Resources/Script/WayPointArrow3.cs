using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointArrow3 : MonoBehaviour
{
    [SerializeField] GameObject vrcamera;
    [SerializeField] GameObject Manager;
    [SerializeField] Text distance;
    [SerializeField] Vector3 ArrivePoint;

    private void Awake()
    {
        if(Manager.GetComponent<FindArriveManager>()&& Manager.GetComponent<FindArriveManager>().GetEndPos()!=null)
            ArrivePoint= Manager.GetComponent<FindArriveManager>().GetEndPos().position;
        if (Manager.GetComponent<FindObjectManager>()&& Manager.GetComponent<FindObjectManager>().GetEndPos() != null)
            ArrivePoint = Manager.GetComponent<FindObjectManager>().GetEndPos().position;
    }

    private void Update()
    {
        if (Manager.GetComponent<FindArriveManager>() && Manager.GetComponent<FindArriveManager>().GetEndPos() != null)
            ArrivePoint = Manager.GetComponent<FindArriveManager>().GetEndPos().position;
        if (Manager.GetComponent<FindObjectManager>() && Manager.GetComponent<FindObjectManager>().GetEndPos() != null)
            ArrivePoint = Manager.GetComponent<FindObjectManager>().GetEndPos().position;

        Vector3 target = new Vector3(ArrivePoint.x, transform.position.y, ArrivePoint.z);
        transform.LookAt(target);
        //transform.LookAt(ArrivePoint);
        distance.text = ((int)Vector3.Distance(ArrivePoint, vrcamera.transform.position)).ToString() + "m";
    }
}
