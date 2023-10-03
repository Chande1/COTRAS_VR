using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using Valve.VR;

public class FingerTouchObject : MonoBehaviour
{
    Hand RHand;
    Hand LHand;

    [SerializeField] bool aning = false;
    [SerializeField] Animator animator;
    [SerializeField] string AniBoolName;
    [SerializeField] string AnimationName;
    [SerializeField] bool justplay;
    [SerializeField] bool withgrip;

    bool stay;

    private void Awake()
    {
        if(withgrip)
        {
            RHand = GameObject.Find("RightHand").GetComponent<Hand>();
            LHand = GameObject.Find("LeftHand").GetComponent<Hand>();
            stay = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="finger")
        {
            if(!aning&&!withgrip)
            {
                if(justplay)
                {
                    animator.SetBool(AniBoolName, true);
                }
                else
                {
                    if (AnimatorIsPlaying()) //이미 작동중이면 초기화하고 다시 작동
                        animator.SetBool(AniBoolName, false);
                    animator.SetBool(AniBoolName, true);
                    Debug.Log(gameObject.name + "애니메이션 작동");
                    aning = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!stay && other.tag == "RHand" || other.tag == "LHand")
        {
            Debug.Log("touch");
            if (RHand.GetComponent<Hand>().grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            LHand.GetComponent<Hand>().grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                Debug.Log("접촉중인 오브젝트: " + gameObject.name);
                if (justplay)
                {
                    if(animator.GetBool(AniBoolName))
                    {
                        animator.SetBool(AniBoolName, false);
                    }
                    else
                    {
                        animator.SetBool(AniBoolName, true);
                    }
                    
                }
                stay = true;    //접촉완료
            }
        }
    }

    public bool GetNowAni()
    {
        return aning;
    }

    public void OffAni()
    {
        Debug.Log(gameObject.name + "애니메이션 종료");
        animator.SetBool(AniBoolName, false);
        aning = false;
    }

    public bool AnimatorIsDone()
    {

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length + ">=" + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //return animator.GetCurrentAnimatorStateInfo(0).length >= animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        return animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName) &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    public bool AnimatorIsPlaying()
    {
        Debug.Log(AnimationName + ":" + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        return animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f&&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

    public void ResetSetting()
    {
        stay = false;
    }
}
