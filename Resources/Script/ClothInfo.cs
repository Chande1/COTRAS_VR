using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClothInfo : MonoBehaviour
{
    [SerializeField] bool hit;               //�浹 ����
    [SerializeField] Vector3 temppos;    //���� ��ġ
    [SerializeField] Quaternion temprot;    //���� ����
    [SerializeField] Transform selecttrans;  //���õ� �Ƿ� ��ġ
    private float movespeed=0.1f;   //�̵� �ӵ�
    private bool zone;

    private void Awake()
    {
        zone = false;
        hit = false;
        temppos = gameObject.transform.position;
        temprot = gameObject.transform.rotation;

    }

    private void Update()
    {
        //�浹���ϋ�
        if(hit)
        {
            
            //�Ź��� �ƴҶ�
            if(!gameObject.CompareTag("Shoose"))
            {
                //�縻�϶�
                if(gameObject.CompareTag("Sock"))
                {

                    if (Math.Round(gameObject.transform.eulerAngles.y) != Math.Round(temprot.eulerAngles.y - 90) &&
                        Math.Round(gameObject.transform.eulerAngles.y) != Math.Round(temprot.eulerAngles.y - 90) +360)
                    {
                        gameObject.transform.Rotate(Vector3.down * movespeed * 100);
                        //Debug.Log("rotate.y: " + Math.Round(gameObject.transform.eulerAngles.y) + "(" + (Math.Round(temprot.eulerAngles.y + 90)) + ")");
                    }

                }
                else
                {
                    //ȸ��
                    if (Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x + 90) &&
                        Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x + 90) - 360)
                    {
                        gameObject.transform.Rotate(Vector3.right * movespeed * 100);
                        //Debug.Log("rotate.x: " + Math.Round(gameObject.transform.eulerAngles.x) + "(" + (Math.Round(temprot.eulerAngles.x + 90)) + ")");

                    }
                }
                
            }
            //���� ��ǥ�� �̵�
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, selecttrans.transform.position, movespeed);
        }
        else
        {
            //������Ʈ�� ��ġ�� �ʱ� ��ġ�� �ٸ��ٸ�
            if (gameObject.transform.position!=temppos)
            {
                //�ʱ� ��ǥ�� ����
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, temppos, movespeed);
            }
            else
            {
                //�� ���� ������ ����� ���
                if(zone&& gameObject.GetComponent<BoxCollider>()!=null)
                {
                    gameObject.transform.rotation = temprot;
                    gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                    
            }
            //������Ʈ�� ������ �ʱ� ������ �ٸ��ٸ�
            if(gameObject.CompareTag("Sock"))
            {
                if (Math.Round(gameObject.transform.eulerAngles.y) != Math.Round(temprot.eulerAngles.y)&&
                    Math.Round(gameObject.transform.eulerAngles.y) != Math.Round(temprot.eulerAngles.y) +360 && !zone)
                {
                    gameObject.transform.Rotate(Vector3.up * movespeed * 100);
                    Debug.Log("rotate.y: " + Math.Round(gameObject.transform.eulerAngles.y) + "(" + (Math.Round(temprot.eulerAngles.y)) + ")");
                }
            }
            else
            {
                if (Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x) &&
                    Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x) - 360 && !zone)
                {
                    gameObject.transform.Rotate(Vector3.left * movespeed * 100);
                }
            }
        }
    }


    public void HitCloth()
    {
        Debug.Log("Hit " + gameObject.name);
        hit = true;
        selecttrans = GameObject.Find("SelectObject").transform;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void MissCloth()
    {
        Debug.Log("Miss " + gameObject.name);
        hit = false;
        selecttrans = null;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void TempToZone(Transform _clothpos)
    {
        Debug.Log("Select " + gameObject.name);
        hit = false;
        selecttrans = null;
        //temppos = _zone.transform.position;
        //temppos = _hitpoint;
        temppos = _clothpos.position;
        temprot = _clothpos.rotation;
        zone = true;
    }
}
