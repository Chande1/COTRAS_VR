using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothZone : MonoBehaviour
{
    enum CZone
    {
        UC_Zone=0,
        DC_Zone,
        SK_Zone,
        SH_Zone
    }

    [SerializeField] GameObject O_effect;
    [SerializeField] GameObject X_effect;
    [SerializeField] CZone czone;
    [SerializeField] string zonename;
    [SerializeField] int keepclothcount;
    [SerializeField] Transform[] clothspos;

    GameObject CW;  //점수판

    private void Awake()
    {
        CW = GameObject.Find("CountWindow");

        keepclothcount = 0;
        clothspos = new Transform[gameObject.transform.childCount];
        for(int i=0;i<gameObject.transform.childCount;i++)
        {
            clothspos[i] = gameObject.transform.GetChild(i).transform;
        }

        switch(czone)
        {
            case CZone.UC_Zone:
                zonename = "UCloth";
                break;
            case CZone.DC_Zone:
                zonename = "DCloth";
                break;
            case CZone.SK_Zone:
                zonename = "Sock";
                break;
            case CZone.SH_Zone:
                zonename = "Shoose";
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ClothInfo>())
        {
            //정답
            if (other.tag == zonename)
            {
                CW.GetComponent<CountWindow>().CorrectAnswer(); //정답 처리
                //Destroy(other.gameObject);//의류 오브젝트 삭제
                //정답 이펙트 생성
                GameObject c_effect=GameObject.Instantiate(O_effect, transform.position, transform.rotation);
                c_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                Destroy(c_effect, 2f);                          //2초 후에 삭제
                keepclothcount += 1;    //영역에 보관중인 옷 추가
                //콜라이더 삭제
                Destroy(other.gameObject.GetComponent<BoxCollider>());
            }
            //오답
            else
            {
                CW.GetComponent<CountWindow>().InCorrectAnswer(); //오답 처리
                Destroy(other.gameObject);//의류 오브젝트 삭제
                //정답 이펙트 생성
                GameObject c_effect = GameObject.Instantiate(X_effect, transform.position, transform.rotation);
                c_effect.GetComponent<ParticleSystem>().Play(); //파티클 재생
                Destroy(c_effect, 2f);                          //2초 후에 삭제
            }
        }
        
    }

    //현재 보관중인 옷의 좌표 
    public Transform GetKeepClothPos()
    {
        return clothspos[keepclothcount];
    }
}
