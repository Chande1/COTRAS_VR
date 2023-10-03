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
    public int score;       //점수    
    public int rate;        //소독률
    public VirusValue value;   //종류
}

public class VirusInfo : MonoBehaviour
{
    [Header("바이러스 종류")]
    public VirusValue vv;
    private GameObject player;

    [Header("바이러스 몸체")]
    [SerializeField] GameObject vbody;
    [Header("바이러스 파티클")]
    [SerializeField] ParticleSystem particle;

    [SerializeField] Material outline;

    SkinnedMeshRenderer renderers;
    List<Material> materialList = new List<Material>();

    Vector3 pos; //시작위치
    /*
    [Space(10f)]
    [Header("적 움직임 설정")]
    [Header("수평")]
    [SerializeField] public bool horizontal = false;   //수평
    [SerializeField] float hspeed = 1f;      //수평 움직임 속도
    [SerializeField] float hdistance = 1f;  //수평 움직임 거리
    [Space(10f)]
    [Header("수직")]
    [SerializeField] public bool verticality = false;   //수직
    [SerializeField] float vspeed = 1f;      //수평 움직임 속도
    [SerializeField] float vdistance = 1f;  //수평 움직임 거리
    float tempspeed = 1f;
    */
    bool horizontal = false;   //수평
    float hspeed = 1f;      //수평 움직임 속도
    float hdistance = 1f;  //수평 움직임 거리
    
    bool verticality = true;   //수직
    float vspeed = 1f; //수평 움직임 속도
    float vdistance = 0.5f;  //수평 움직임 거리
    float tempspeed = 1f;

    private void Start()
    {
        player = GameObject.Find("Player");
        pos = transform.position;   //시작 위치 저장
        vspeed= Random.Range(1f, 5f);   //랜덤한 속도
        outline = new Material(Shader.Find("Custom/OutLine"));

        if (horizontal)
            tempspeed = hspeed;
        else if (verticality)
            tempspeed = vspeed;
    }

    private void Update()
    {
        gameObject.transform.LookAt(player.transform);  //플레이어 바라보기
        Enemymoving();  //적의 움직임 설정값 적용
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="RHand"||other.tag=="LHand")
        {
            //Debug.Log("접촉중인 바이러스: " + gameObject.name);
            GameObject.Find("VirusGameManager").GetComponent<VirusGameManager>().VirusCheck(gameObject.name, vv);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RHand" || other.tag == "LHand")
        {
            //Debug.Log("접촉중인 바이러스: " + gameObject.name);

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

    //위아래 움직임
    void Enemymoving()
    {
        Vector3 vec = pos;

        if (horizontal) //수평 모드
        {
            hspeed = tempspeed;
            vec.z += hdistance * Mathf.Sin(Time.time * hspeed);
            transform.position = vec;
        }
        if (verticality)    //수직 모드
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
        Debug.Log("클릭중인 바이러스: " + gameObject.name);
        GameObject.Find("VirusGameManager").GetComponent<VirusGameManager>().VirusCheck(gameObject.name, vv);
    }*/
}



