using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClothInfo : MonoBehaviour
{
    [SerializeField] bool hit;               //충돌 감지
    [SerializeField] Vector3 temppos;    //원래 위치
    [SerializeField] Quaternion temprot;    //원래 각도
    [SerializeField] Transform selecttrans;  //선택된 의류 위치
    private float movespeed=0.1f;   //이동 속도
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
        //충돌중일떄
        if(hit)
        {
            
            //신발이 아닐때
            if(!gameObject.CompareTag("Shoose"))
            {
                //양말일때
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
                    //회전
                    if (Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x + 90) &&
                        Math.Round(gameObject.transform.eulerAngles.x) != Math.Round(temprot.eulerAngles.x + 90) - 360)
                    {
                        gameObject.transform.Rotate(Vector3.right * movespeed * 100);
                        //Debug.Log("rotate.x: " + Math.Round(gameObject.transform.eulerAngles.x) + "(" + (Math.Round(temprot.eulerAngles.x + 90)) + ")");

                    }
                }
                
            }
            //선택 좌표로 이동
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, selecttrans.transform.position, movespeed);
        }
        else
        {
            //오브젝트의 위치가 초기 위치와 다르다면
            if (gameObject.transform.position!=temppos)
            {
                //초기 좌표로 복구
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, temppos, movespeed);
            }
            else
            {
                //옷 정리 영역에 닿았을 경우
                if(zone&& gameObject.GetComponent<BoxCollider>()!=null)
                {
                    gameObject.transform.rotation = temprot;
                    gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                    
            }
            //오브젝트의 각도가 초기 각도와 다르다면
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
