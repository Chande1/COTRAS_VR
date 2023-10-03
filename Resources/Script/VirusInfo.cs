using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VirusValue
{
    Red = 0,
    Green,
    Blue
}

public class Virus
{
    public int score;       //����    
    public int rate;        //�ҵ���
    public VirusValue value;   //����
}

public class VirusInfo : MonoBehaviour
{
    [Header("���̷��� ����")]
    public VirusValue vv;
    private GameObject player;

    [Header("���̷��� ��ü")]
    [SerializeField] GameObject vbody;
    [Header("���̷��� ��ƼŬ")]
    [SerializeField] ParticleSystem particle;

    [SerializeField] Material outline;

    SkinnedMeshRenderer renderers;
    List<Material> materialList = new List<Material>();

    Vector3 pos; //������ġ
    /*
    [Space(10f)]
    [Header("�� ������ ����")]
    [Header("����")]
    [SerializeField] public bool horizontal = false;   //����
    [SerializeField] float hspeed = 1f;      //���� ������ �ӵ�
    [SerializeField] float hdistance = 1f;  //���� ������ �Ÿ�
    [Space(10f)]
    [Header("����")]
    [SerializeField] public bool verticality = false;   //����
    [SerializeField] float vspeed = 1f;      //���� ������ �ӵ�
    [SerializeField] float vdistance = 1f;  //���� ������ �Ÿ�
    float tempspeed = 1f;
    */
    bool horizontal = false;   //����
    float hspeed = 1f;      //���� ������ �ӵ�
    float hdistance = 1f;  //���� ������ �Ÿ�
    
    bool verticality = true;   //����
    float vspeed = 1f; //���� ������ �ӵ�
    float vdistance = 0.5f;  //���� ������ �Ÿ�
    float tempspeed = 1f;

    private void Start()
    {
        player = GameObject.Find("Player");
        pos = transform.position;   //���� ��ġ ����
        vspeed= Random.Range(1f, 5f);   //������ �ӵ�
        outline = new Material(Shader.Find("Custom/OutLine"));

        if (horizontal)
            tempspeed = hspeed;
        else if (verticality)
            tempspeed = vspeed;
    }

    private void Update()
    {
        gameObject.transform.LookAt(player.transform);  //�÷��̾� �ٶ󺸱�
        Enemymoving();  //���� ������ ������ ����
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="RHand"||other.tag=="LHand")
        {
            //Debug.Log("�������� ���̷���: " + gameObject.name);
            GameObject.Find("VirusGameManager").GetComponent<VirusGameManager>().VirusCheck(gameObject.name, vv);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RHand" || other.tag == "LHand")
        {
            //Debug.Log("�������� ���̷���: " + gameObject.name);

            renderers = vbody.GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderers.sharedMaterials);
            materialList.Add(outline);

            renderers.materials = materialList.ToArray();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RHand" || other.tag == "LHand")
        {
            SkinnedMeshRenderer renderer = vbody.GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }
    }

    //���Ʒ� ������
    void Enemymoving()
    {
        Vector3 vec = pos;

        if (horizontal) //���� ���
        {
            hspeed = tempspeed;
            vec.z += hdistance * Mathf.Sin(Time.time * hspeed);
            transform.position = vec;
        }
        if (verticality)    //���� ���
        {
            vspeed = tempspeed;
            vec.y += vdistance * Mathf.Sin(Time.time * vspeed);
            transform.position = vec;
        }
        
    }

    public void DestoyVirus()
    {
        vbody.SetActive(false);
        particle.Play();
        Destroy(gameObject, 2f);
    }

    /*
    private void OnMouseOver()
    {
        Debug.Log("Ŭ������ ���̷���: " + gameObject.name);
        GameObject.Find("VirusGameManager").GetComponent<VirusGameManager>().VirusCheck(gameObject.name, vv);
    }*/
}



