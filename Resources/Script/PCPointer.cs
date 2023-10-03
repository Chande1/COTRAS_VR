using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PCPointer : MonoBehaviour
{
    //컨트롤러 정보
    [Header("카메라")]
    [SerializeField] Camera camera;
    [Tooltip("충돌 객체")]
    [SerializeField] RaycastHit hit;           // 충돌된 레이의 히트포인트
    [Tooltip("충돌 객체 임시 저장용")]
    [SerializeField] GameObject tempObj;            // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    [Header("선택된 의류 영역 객체")]
    [SerializeField] GameObject ZoneObj;            //의류 영역이 객체




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

            //좌클릭중
            if(Input.GetMouseButton(0))
            {
                if (hit.collider.gameObject.CompareTag("UCloth") || hit.collider.gameObject.CompareTag("DCloth") ||
                    hit.collider.gameObject.CompareTag("Sock") || hit.collider.gameObject.CompareTag("Shoose")) //신발을 제외한 의류일 경우
                {
                    Debug.Log("hit collider: " + hit.collider.gameObject.name);    //레이케스트와 충돌중인 오브젝트 이름 출력 

                    if (tempObj == null)    //임시 저장용 객체가 null인 경우(아직 버튼을 안눌렀거나 눌렀다 뗐던 경우
                    {
                        tempObj = hit.collider.gameObject;                                  //임시 객체에 현재 충돌한 객체 저장 
                        tempObj.GetComponent<ClothInfo>().HitCloth();                       //충돌중!
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
                    if (tempObj != null)    //임시 저장용 객체가 null이 아닌 경우(버튼을 눌렀던 경우)
                    {
                        tempObj = null;                                  //임시 객체 비우기                        
                    }*/
                }
            }
            else if(Input.GetMouseButtonUp(0))  //좌클릭 버튼이 떼어져 있을 때
            {
                if (tempObj != null)    //임시 저장용 객체가 null이 아닌 경우(버튼을 눌렀던 경우)
                {
                    if (ZoneObj)
                    {
                        //tempObj.GetComponent<ClothInfo>().TempToZone(hit.point);
                        tempObj.GetComponent<ClothInfo>().TempToZone(ZoneObj.GetComponent<ClothZone>().GetKeepClothPos()) ;
                    }
                    else if (tempObj.GetComponent<ClothInfo>() != null)
                    {
                        tempObj.GetComponent<ClothInfo>().MissCloth();  //충돌 취소!
                    }
                    tempObj = null;                                  //임시 객체 비우기
                }
            }

        }
    }
}
