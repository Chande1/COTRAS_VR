using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTale : MonoBehaviour
{
    [TextArea]
    [SerializeField] string memo;

    [Header("머리")]
    [SerializeField] RectTransform Head;
    [Header("방향")]
    [SerializeField] bool horizontal;
    [SerializeField] bool vertical;
    [Header("거리")]
    [SerializeField] float distance;
    RectTransform head;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(horizontal)
        {
            transform.localPosition = new Vector3(Head.localPosition.x+distance, Head.localPosition.y, Head.localPosition.z);
        }
        else if(vertical)
        {
            transform.localPosition = new Vector3(Head.localPosition.x, Head.localPosition.y + distance, Head.localPosition.z);
        }
    }
}
