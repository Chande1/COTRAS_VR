using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LaserPoint : MonoBehaviour
{
    //컨트롤러 정보
    [SerializeField] Hand RHand;
    [Tooltip("레이저가 될 라인 랜더러")]
    [SerializeField] LineRenderer laser;           // 레이저
    [Tooltip("충돌 객체")]
    [SerializeField] RaycastHit hit;           // 충돌된 레이의 히트포인트
    [Tooltip("충돌 객체 임시 저장용")]
    [SerializeField] GameObject tempObj;            // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    [Header("선택된 의류 영역 객체")]
    [SerializeField] GameObject ZoneObj;            //의류 영역이 객체
    [Header("레이저 설정")]
    [Range(10, 100)]
    [Tooltip("(필수)레이저 거리")]
    [SerializeField] float raycastDistance;  // 레이저 포인터 감지 거리
    [Tooltip("(필수)트리거를 뗀 레이저 색깔")]
    [SerializeField] Color lasercolor1;             //레이저 색깔1
    [Tooltip("(필수)트리거를 누른 레이저 색깔")]
    [SerializeField] Color lasercolor2;             //레이저 색깔2
    [Tooltip("(필수)레이저 굵기")]
    [SerializeField] float laserThickness;         //레이저 굵기

    void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();                   //라인랜더러
        laser.positionCount = 0;                                           //레이저 시작과 끝점                                     
        laser.SetWidth(laserThickness, laserThickness);                     //레이저 굵기
        //laser.startColor = lasercolor1;
        //laser.endColor = lasercolor1;
    }

    // Update is called once per frame
    void Update()
    {
        laser.positionCount = 0;    //조건에 부합하지 않으면 조준선이 생기지 않도록

        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))  //레이캐스트 거리 내의 충돌 감지
        {
            if (RHand.grabPinchAction.GetState(SteamVR_Input_Sources.RightHand)) //오른손 트리거가 눌리는중이면
            {
                if (hit.collider.gameObject.CompareTag("UCloth")|| hit.collider.gameObject.CompareTag("DCloth")||
                    hit.collider.gameObject.CompareTag("Sock") || hit.collider.gameObject.CompareTag("Shoose")) //신발을 제외한 의류일 경우
                {
                    //Debug.Log(hit.collider.gameObject.name);    //레이케스트와 충돌중인 오브젝트 이름 출력 
                    laser.positionCount = 2;

                    if (tempObj == null)    //임시 저장용 객체가 null인 경우(아직 버튼을 안눌렀거나 눌렀다 뗐던 경우
                    {
                        tempObj = hit.collider.gameObject;                                  //임시 객체에 현재 충돌한 객체 저장 
                        tempObj.GetComponent<ClothInfo>().HitCloth();                       //충돌중!
                    }

                    laser.SetPosition(0, transform.position);                               //시작점
                    laser.SetPosition(1, hit.point);                                        //끝점을 충돌된 객체 좌표로 고정 
                }
                else if(hit.collider.gameObject.CompareTag("CZone"))
                {
                    ZoneObj = hit.collider.gameObject;

                    laser.positionCount = 2;
                    laser.SetPosition(0, transform.position);                                           //시작점
                    laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));   //설정한 최대 거리까지 표시
                }
                else if(hit.collider.gameObject.CompareTag("Button")) // 충돌 객체의 태그가 Button인 경우
                {
                    //Debug.Log("bt");
                    laser.positionCount = 2;
                    laser.SetColors(lasercolor2, lasercolor2);                  //빨간색

                    if (tempObj == null)    //임시 저장용 객체가 null인 경우(아직 버튼을 안눌렀거나 눌렀다 뗐던 경우
                    {
                        tempObj = hit.collider.gameObject;                                  //임시 객체에 현재 충돌한 객체 저장 

                        EventSystem.current.SetSelectedGameObject(hit.collider.gameObject);
                        PointerEventData pointerEventData = new PointerEventData(null);
                        pointerEventData.position = new Vector2(hit.point.x, hit.point.y);

                        //temp.OnSelect(pointerEventData);
                        hit.collider.gameObject.GetComponent<Button>().OnPointerClick(pointerEventData);
                        //hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();       //오브젝트에 등록된 onClick()이벤트 실행
                    }

                    laser.SetPosition(0, transform.position);                               //시작점
                    laser.SetPosition(1, hit.point);                                        //끝점을 충돌된 객체 좌표로 고정 
                }
                else
                {
                    ZoneObj = null;

                    /*
                    if (tempObj != null)    //임시 저장용 객체가 null이 아닌 경우(버튼을 눌렀던 경우)
                    {
                        if(tempObj.GetComponent<Button>()!=null)
                            tempObj.GetComponent<Button>().OnPointerExit(null);
                        tempObj = null;                                  //임시 객체 비우기                        
                    }*/
                    //Debug.Log("nbt:" + hit.transform.tag);
                    laser.positionCount = 2;
                    laser.SetPosition(0, transform.position);                                           //시작점
                    laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));   //설정한 최대 거리까지 표시
                }
            }
            else  //오른손 트리거 버튼이 떼어져 있을 때
            {
                if (tempObj != null)    //임시 저장용 객체가 null이 아닌 경우(버튼을 눌렀던 경우)
                {
                    if (ZoneObj)
                    {
                        tempObj.GetComponent<ClothInfo>().TempToZone(ZoneObj.GetComponent<ClothZone>().GetKeepClothPos());
                    }
                    else if(tempObj.GetComponent<ClothInfo>()!=null)
                    {
                        tempObj.GetComponent<ClothInfo>().MissCloth();  //충돌 취소!
                    }

                    if(tempObj.GetComponent<Button>()!=null)
                        tempObj.GetComponent<Button>().OnPointerExit(null);
                    tempObj = null;                                  //임시 객체 비우기
                                                                     
                }
                laser.positionCount = 2;
                laser.SetPosition(0, transform.position);                                           //시작점
                laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));   //설정한 최대 거리까지 표시
                //laser.startColor = lasercolor1;
                //laser.endColor = lasercolor1;
            }
        }

    }

}
