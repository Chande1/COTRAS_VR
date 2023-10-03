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
    [Header("��ġ �� ��ȣ�ۿ� ����")]
    [SerializeField] bool stay;
    [Header("������ �ִ� ������Ʈ")]
    [SerializeField] bool havegoal;
    [Header("������ ������ �ִ� ������Ʈ")]
    [SerializeField] bool haveoutside;
    [Header("�ܰ��� �۵� ����(Ȯ�ο�)")]
    [SerializeField] bool work;
    [Header("���� ���� ����(Ȯ�ο�)")]
    [SerializeField] bool arrive;

    Hand RHand;
    Hand LHand;
    Vector3 temppos;  //ó����ġ
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
                //Debug.Log("�������� ������Ʈ: " + gameObject.name);
                OutLineOff();   //�ܰ��� ����
                work = false ;    //���˿Ϸ�
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
            Debug.Log(gameObject.name+"�� �ܰ� ������ �̵��߽��ϴ�!");
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
        Debug.Log(gameObject.name + "�� ���� �ڸ��� ���ư��ϴ�!");
        gameObject.transform.position = temppos;
    }

    public void ResetObjectSetting()
    {
        arrive = false;
        work = false;
    }
}
