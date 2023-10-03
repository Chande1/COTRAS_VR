using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using Valve.VR;


[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class OutLineObject : MonoBehaviour
{
    [SerializeField] bool Skinned;
    [SerializeField] Material outline;
    [Header("터치 시 상호작용 여부")]
    [SerializeField] bool stay;
    [Header("목적이 있는 오브젝트")]
    [SerializeField] bool havegoal;
    [Header("리스폰 범위가 있는 오브젝트")]
    [SerializeField] bool haveoutside;
    [Header("외각선 작동 여부(확인용)")]
    [SerializeField] bool work;
    [Header("목적 도착 여부(확인용)")]
    [SerializeField] bool arrive;

    Hand RHand;
    Hand LHand;
    Vector3 temppos;  //처음위치
    SkinnedMeshRenderer skinnedrend;
    MeshRenderer meshrend;

    List<Material> materialList = new List<Material>();

    public void Awake()
    {
        arrive = false;
        temppos = gameObject.transform.position;
        outline = new Material(Shader.Find("Custom/OutLine"));
        RHand = GameObject.Find("RightHand").GetComponent<Hand>();
        LHand = GameObject.Find("LeftHand").GetComponent<Hand>();
    }

    public void OutLineOn()
    {
        Debug.Log(gameObject.name+"'s outline on.");

        if(Skinned)
        {
            skinnedrend = GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(skinnedrend.sharedMaterials);
            materialList.Add(outline);

            skinnedrend.materials = materialList.ToArray();
        }
        else
        {
            meshrend = GetComponent<MeshRenderer>();

            materialList.Clear();
            materialList.AddRange(meshrend.sharedMaterials);
            materialList.Add(outline);

            meshrend.materials = materialList.ToArray();
        }
        work = true;
    }

    public void OutLineOff()
    {
        Debug.Log(gameObject.name + "'s outline off.");

        if (Skinned)
        {
            SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }
        else
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }
        work = false;
    }

    public bool OutLineWork()
    {
        if (work)
            return true;
        else
            return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!stay&&other.tag == "RHand" || other.tag == "LHand")
        {
            //Debug.Log("touch");
            if (RHand.GetComponent<Hand>().grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand) ||
            LHand.GetComponent<Hand>().grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                //Debug.Log("접촉중인 오브젝트: " + gameObject.name);
                OutLineOff();   //외각선 종료
                work = false ;    //접촉완료
            }   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(havegoal&&other.tag=="goal")
        {
            arrive = true;
        }
        if(haveoutside&&other.tag=="outside")
        {
            Debug.Log(gameObject.name+"이 외각 범위로 이동했습니다!");
            gameObject.transform.position = temppos;
        }
    }

    public bool ObjectArriveGoal()
    {
        if (arrive)
            return true;
        else
            return false;
    }

    public void ResetPostion()
    {
        Debug.Log(gameObject.name + "이 원래 자리로 돌아갑니다!");
        gameObject.transform.position = temppos;
    }

    public void ResetObjectSetting()
    {
        arrive = false;
        work = false;
    }
}
