using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PCPointer : MonoBehaviour
{
    //��Ʈ�ѷ� ����
    [Header("ī�޶�")]
    [SerializeField] Camera camera;
    [Tooltip("�浹 ��ü")]
    [SerializeField] RaycastHit hit;           // �浹�� ������ ��Ʈ����Ʈ
    [Tooltip("�浹 ��ü �ӽ� �����")]
    [SerializeField] GameObject tempObj;            // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    [Header("���õ� �Ƿ� ���� ��ü")]
    [SerializeField] GameObject ZoneObj;            //�Ƿ� ������ ��ü




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Display.RelativeMouseAt(Input.mousePosition));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray = Camera.main.ViewportPointToRay(Display.RelativeMouseAt(Input.mousePosition));
        //Debug.Log(Camera.main.name);
        //Ray ray = gamecam.ViewportPointToRay(Display.RelativeMouseAt(Input.mousePosition));
        RaycastHit hit;
        

        /*
        int di = GetHoveredDisplay();
        Camera currentDisplayCamera = cameras[di];
        Ray ray = currentDisplayCamera.ScreenPointToRay(Input.mousePosition);
        */
        //Debug.DrawRay(Input.mousePosition, ray.direction,Color.red);
        /*Debug.DrawRay(transform.position, transform.forward*20, Color.red);
        if (Physics.Raycast(transform.position, transform.forward * 20, out hit))*/
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("hit object: "+hit.transform.gameObject);

            //��Ŭ����
            if(Input.GetMouseButton(0))
            {
                if (hit.collider.gameObject.CompareTag("UCloth") || hit.collider.gameObject.CompareTag("DCloth") ||
                    hit.collider.gameObject.CompareTag("Sock") || hit.collider.gameObject.CompareTag("Shoose")) //�Ź��� ������ �Ƿ��� ���
                {
                    Debug.Log("hit collider: " + hit.collider.gameObject.name);    //�����ɽ�Ʈ�� �浹���� ������Ʈ �̸� ��� 

                    if (tempObj == null)    //�ӽ� ����� ��ü�� null�� ���(���� ��ư�� �ȴ����ų� ������ �ô� ���
                    {
                        tempObj = hit.collider.gameObject;                                  //�ӽ� ��ü�� ���� �浹�� ��ü ���� 
                        tempObj.GetComponent<ClothInfo>().HitCloth();                       //�浹��!
                    }
                }
                else if (hit.collider.gameObject.CompareTag("CZone"))
                {
                    ZoneObj = hit.collider.gameObject;
                }
                
                else
                {
                    ZoneObj = null;
                    /*
                    if (tempObj != null)    //�ӽ� ����� ��ü�� null�� �ƴ� ���(��ư�� ������ ���)
                    {
                        tempObj = null;                                  //�ӽ� ��ü ����                        
                    }*/
                }
            }
            else if(Input.GetMouseButtonUp(0))  //��Ŭ�� ��ư�� ������ ���� ��
            {
                if (tempObj != null)    //�ӽ� ����� ��ü�� null�� �ƴ� ���(��ư�� ������ ���)
                {
                    if (ZoneObj)
                    {
                        //tempObj.GetComponent<ClothInfo>().TempToZone(hit.point);
                        tempObj.GetComponent<ClothInfo>().TempToZone(ZoneObj.GetComponent<ClothZone>().GetKeepClothPos()) ;
                    }
                    else if (tempObj.GetComponent<ClothInfo>() != null)
                    {
                        tempObj.GetComponent<ClothInfo>().MissCloth();  //�浹 ���!
                    }
                    tempObj = null;                                  //�ӽ� ��ü ����
                }
            }

        }
    }
}
