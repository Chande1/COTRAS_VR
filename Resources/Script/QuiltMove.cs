using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuiltMove : MonoBehaviour
{
    [SerializeField] bool isLeft;   //���� ������
    [SerializeField] bool isStay;   //�� ���� ����
    [SerializeField] bool isFirst;  //ó�� �����ϴ� ����
    [SerializeField] GameObject FirstObject;  //ó���� �ƴ� ��� ó�� ������Ʈ ���
    
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

    //�ܺ� �̺� ���� Ȯ�ο�
    public bool QuiltMoveNow()
    {
        if (isStay)
            return true;
        else
            return false;
    }

    //���� �ʱ�ȭ
    public void ResetQuilt()
    {
        isStay = false;
    }
}
