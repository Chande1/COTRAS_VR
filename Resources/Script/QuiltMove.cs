using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiltMove : MonoBehaviour
{
    [SerializeField] bool isLeft;   //왼쪽 오른쪽
    [SerializeField] bool isStay;   //손 접촉 여부
    [SerializeField] bool isFirst;  //처음 시작하는 방향
    [SerializeField] GameObject FirstObject;  //처음이 아닐 경우 처음 오브젝트 등록
    
    private void Awake()
    {
        isStay = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "RHand")
        {
            if (!isLeft)
            {
                if(isFirst)
                {
                    isStay = true;
                }
                else if(FirstObject.GetComponent<QuiltMove>().QuiltMoveNow())
                {
                    isStay = true;
                }
            }
                
        }
        else if(other.tag == "LHand")
        {
            if (isLeft)
            {
                if (isFirst)
                {
                    isStay = true;
                }
                else if (FirstObject.GetComponent<QuiltMove>().QuiltMoveNow())
                {
                    isStay = true;
                }
            }
        }
    }

    //외부 이불 상태 확인용
    public bool QuiltMoveNow()
    {
        if (isStay)
            return true;
        else
            return false;
    }

    //상태 초기화
    public void ResetQuilt()
    {
        isStay = false;
    }
}
