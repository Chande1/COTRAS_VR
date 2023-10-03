using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPoint : MonoBehaviour
{
    [Header("의상 스폰 포인트 오브젝트")]
    [Tooltip("필수 입력값")]
    [SerializeField] GameObject cspoint;        //의상 스폰 포인트
    [Tooltip("필수 입력값")]
    [SerializeField] GameObject[] csp_child;      //의상 스폰 포인트 자식 
    [Header("의상 오브젝트")]
    [SerializeField] GameObject[] upcloths;     //상의
    [SerializeField] GameObject[] downcloths;   //하의
    [SerializeField] GameObject[] socks;        //양말
    [SerializeField] GameObject[] shoose;       //신발
    [Header("의상 오브젝트 갯수")]
    [Tooltip("필수 입력값")]
    [SerializeField] int c_count;    //전체 의상 갯수(기준24)
    [SerializeField] int uc_count;   //상의 갯수
    [SerializeField] int dc_count;   //하의 갯수
    [SerializeField] int sk_count;   //양말 갯수
    [SerializeField] int sh_count;   //신발 갯수
    [Header("의상 갯수 평균 오차")]
    [Range(0, 5)]
    [SerializeField] int clth_min_max;  //의상 갯수 오차
    [Header("선별된 의상 오브젝트")]
    [Tooltip("필수 입력값")]
    [SerializeField] GameObject[] select_clth;  //선택된 의상 오브젝트
    
    GameObject SelectCloth; //선택된 옷들이 하위로 저장될 빈 부모 개체

    void Awake()
    {
        //선택된 옷들이 하위로 저장될 빈 부모 개체 생성
        SelectCloth = new GameObject("SelectCloth");
        //스폰 포인트 저장
        for (int i = 0; i < cspoint.transform.childCount; i++)
        {
            csp_child[i] = cspoint.transform.GetChild(i).gameObject;
        }
        //랜덤한 의상 갯수 설정
        RandomClothCount();
        //랜덤한 의상 선택 저장
        SelectRandomCloth();
        //랜덤한 의상 자리 배치
        RandomClothSpawn();
    }


    //랜덤한 의상 갯수 설정
    private void RandomClothCount()
    {
        uc_count = Random.Range(c_count / 4 - clth_min_max, upcloths.Length);
        dc_count = Random.Range(c_count / 4 - clth_min_max, downcloths.Length);

        //상의와 하의의 갯수가 전체 갯수의 3/4이상 일때
        if ((uc_count + dc_count) >= (c_count / 4) * 3)
        {
            sk_count = Random.Range(1, c_count - (uc_count + dc_count) - 1);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
        //상의와 하의의 갯수가 전체의 반 이상 일때
        else if ((uc_count + dc_count) >= (c_count / 2))
        {
            sk_count = Random.Range(c_count / 4 - clth_min_max, socks.Length);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
        //상의와 하의의 갯수가 전체의 반 미만 일때
        else
        {
            sk_count = Random.Range(c_count / 4 + clth_min_max, socks.Length);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
    }

    //랜덤 의상 선택 저장
    private void SelectRandomCloth()
    {
        for(int i=0;i<c_count;i++)
        {
            //상의 저장
            if(i<uc_count)
            {
                select_clth[i] = upcloths[i];
            }
            //하의 저장
            else if(i<uc_count+dc_count)
            {
                select_clth[i] = downcloths[i-uc_count];
            }
            //양말
            else if (i < uc_count + dc_count+sk_count)
            {
                select_clth[i] = socks[i - (uc_count+dc_count)];
            }
            //신발
            else if (i < uc_count + dc_count+sk_count+sh_count)
            {
                select_clth[i] = shoose[i - (uc_count+dc_count+sk_count)];
            }
        }

        int random1;
        int random2;

        GameObject tmp;

        for (int index = 0; index < select_clth.Length; ++index)
        {
            random1 = Random.Range(0, select_clth.Length);
            random2 = Random.Range(0, select_clth.Length);

            tmp = select_clth[random1];
            select_clth[random1] = select_clth[random2];
            select_clth[random2] = tmp;
        }
    }

    //랜덤한 의상 자리 배치
    private void RandomClothSpawn()
    {
        for(int i=0;i<csp_child.Length;i++)
        {
            GameObject.Instantiate(select_clth[i], csp_child[i].transform.position, select_clth[i].transform.rotation).transform.parent
                      = SelectCloth.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
