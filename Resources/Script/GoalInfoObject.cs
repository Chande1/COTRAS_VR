using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalInfoObject : MonoBehaviour
{
    [Header("트리거 오브젝트")]
    [SerializeField] GameObject triggerObject;
    [Header("오브젝트 중앙 좌표")]
    [SerializeField] GameObject ObjectMidPos;
    [SerializeField] bool touch = false;

    [Header("오차범위")]
    [SerializeField] float Margin;  //오차범위
    [SerializeField] bool mid=false;
    [SerializeField] int midlength=0;

    [Header("접촉 시간")]
    [SerializeField] bool havetouchtime;
    [SerializeField] bool touching;
    [SerializeField] float touchtime;
    [SerializeField] float temptime;

    [Header("애니메이션")]
    [SerializeField] bool haveani;
    [SerializeField] Animator animator;
    [SerializeField] string anibool;            //실행할 애니 부울

    [Header("접촉 갯수")]
    [SerializeField] string objecttag;
    [SerializeField] bool havecount;
    [SerializeField] bool notstay;
    [SerializeField] int objectcount;
    [SerializeField] GameObject[] tempcountobject;
    bool same;

    private void OnTriggerEnter(Collider other)
    {
        if(triggerObject!=null&&other.name==triggerObject.name)
        {
            Debug.Log("접촉중인 오브젝트:" + other.name);
            if (Margin > 0)
                GetMidToMidMargin();
            else
            {
                if(havetouchtime)
                {
                    touching = true;
                }
                else
                {
                    touch = true;
                }
                if(haveani)
                {
                    if(animator!=null)
                        animator.SetBool(anibool, true);
                    else
                    {
                        Debug.Log(other.name + "애니메이션 실행");
                        other.gameObject.GetComponent<Animator>().SetBool(anibool, true);
                    }
                }
            }
        }
        if(other.tag==objecttag)
        {
            if(havecount)
            {
                Debug.Log(other.gameObject.name+" 접촉");

                for (int i = 0;i <= objectcount;i++)
                {
                    if(tempcountobject[i]!=null&&tempcountobject[i].name==other.gameObject.name)
                    {
                        same = true;
                    }
                }

                if (!same)
                {
                    Debug.Log(other.gameObject.name + " 접촉중 등록");
                    tempcountobject[objectcount] = other.gameObject;
                    if(haveani)
                    {
                        if (animator != null)
                            animator.SetBool(anibool, true);
                        else
                        {
                            Debug.Log(other.name + "애니메이션 실행");
                            other.gameObject.GetComponent<Animator>().SetBool(anibool, true);
                        }
                    }
                    objectcount += 1;
                }
                else
                {
                    same = false;
                }
                
                touch = true;
                
            }
            else
            {
                touch = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(touching)
        {
            temptime += Time.deltaTime;

            if(temptime>=touchtime)
            {
                touch = true;
                temptime = 0f;
                touching = false;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (triggerObject != null && other.name == triggerObject.name)
        {
            if (havetouchtime)
            {
                temptime = 0f;
                touching = false;
            }
            if(haveani)
            {
                if(animator!=null)
                    animator.SetBool(anibool, false);
            }
        }
        if (other.tag == objecttag)
        {
            if (havecount&&!notstay)
            {
                //objectcount -= 1;
            }
            else
            {
                touch = false;
            }
        }
    }

    void GetMidToMidMargin()
    {
        if(Vector3.Distance(gameObject.transform.position,ObjectMidPos.transform.position)<=Margin)
        {
            Debug.Log(gameObject.name + "와 " + triggerObject.name + "사이의 거리가 가깝습니다: " 
                + Vector3.Distance(gameObject.transform.position, ObjectMidPos.transform.position));
            midlength = 1;
            mid = true;
        }
        else if(Vector3.Distance(gameObject.transform.position, ObjectMidPos.transform.position) >= Margin*2)
        {
            Debug.Log(gameObject.name + "와 " + triggerObject.name + "사이의 거리가 멉니다: "
                + Vector3.Distance(gameObject.transform.position, ObjectMidPos.transform.position));
            midlength = 2;
            mid = false;
        }
        else
        {
            Debug.Log(gameObject.name + "와 " + triggerObject.name + "사이의 거리가 중간입니다: "
               + Vector3.Distance(gameObject.transform.position, ObjectMidPos.transform.position));
            midlength = 3;
            mid = false;
        }
    }

    public int GetMarginLength()
    {
        if (midlength == 1)
            return 1;
        else if (midlength == 2)
            return 2;
        else if (midlength == 3)
            return 3;
        else
            return 0;
    }


    public bool GetTouch()
    {
        if (touch)
            return true;
        else
            return false;
    }

    public bool GetMargin()
    {
        if (mid)
            return true;
        else
            return false;
    }

    public void TouchTime()
    {
        touch = true;
    }

    public void ResetGoalInfoObject()
    {
        touch = false;
        mid = false;
        Debug.Log("초기화!");
    }

    public int GetCount()
    {
        return objectcount;
    }
}
