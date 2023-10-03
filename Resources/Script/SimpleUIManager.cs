using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUIManager : MonoBehaviour
{
    [Header("출력될 UI배열")]
    [SerializeField]GameObject[] UIBox;     //출력될 UI배열
    [Header("출력되고 있는 UI 순서")]
    [SerializeField] int ingnum;
    [Header("출력될 결과UI배열")]
    [SerializeField] GameObject[] RUIBox;   //출력될 결과UI배열
    [Header("출력되고 있는 결과 UI 순서")]
    [SerializeField] int ingrnum;

    private void Awake()
    {
        StartSetting();
    }

    void StartSetting()
    {
        for(int i=0;i<UIBox.Length;i++)
        {
            UIBox[i].SetActive(false);
        }

        if(RUIBox.Length!=0)
        {
            for(int j=0;j<RUIBox.Length;j++)
            {
                RUIBox[j].SetActive(false);
            }
        }
    }

    public void ShowNowUI()
    {
        if(RUIBox.Length!=0)
        {
            RUIBox[ingrnum].SetActive(false);
        }
        else
        {
            //계란, 라면용
            if (UIBox[0].activeSelf)
                UIBox[0].SetActive(false);
        }
      
        UIBox[ingnum].SetActive(true);
        Debug.Log("shownow:" + ingnum);
    }

    public void ShowNextUI()
    {
        if (RUIBox.Length != 0)
        {
            RUIBox[ingrnum].SetActive(false);
        }
        else
        {
            //계란, 라면용
            if (UIBox[0].activeSelf)
                UIBox[0].SetActive(false);
        }

        if (UIBox[ingnum].activeSelf)
        {
            UIBox[ingnum].SetActive(false);
        }

        Debug.Log("shownext:" + (ingnum + 1));
        UIBox[ingnum+1].SetActive(true);
        ingnum += 1;
        
    }

    public void ShowNumUI(int _num)
    {
        if (RUIBox.Length != 0)
        {
            RUIBox[ingrnum].SetActive(false);
        }
        else
        {
            //계란, 라면용
            if (UIBox[0].activeSelf)
                UIBox[0].SetActive(false);
        }

        if (UIBox[ingnum].activeSelf)
            UIBox[ingnum].SetActive(false);
        UIBox[_num].SetActive(true);
    }

    public void ShowNumUI(int _num,int _next)
    {
        if (RUIBox.Length != 0)
        {
            RUIBox[ingrnum].SetActive(false);
        }
        else
        {
            //계란, 라면용
            if (UIBox[0].activeSelf)
                UIBox[0].SetActive(false);
        }

        if (UIBox[ingnum].activeSelf)
            UIBox[ingnum].SetActive(false);
        ingnum = _num;
        UIBox[_num].SetActive(true);
    }

    public void ShowNumUI(int _num,bool _next)
    {
        if (RUIBox.Length != 0)
        {
            RUIBox[ingrnum].SetActive(false);
        }
        else
        {
            //계란, 라면용
            if (UIBox[0].activeSelf)
                UIBox[0].SetActive(false);
        }

        if (UIBox[ingnum].activeSelf)
            UIBox[ingnum].SetActive(false);
        UIBox[_num].SetActive(true);

        if (_next)
            ingnum += 1;

        Debug.Log("shownum:" + _num+"/ingnum:"+ingnum);
        
    }

    public void HideNowUI()
    {
        if (UIBox[ingnum].activeSelf)
            UIBox[ingnum].SetActive(false);
    }

    public void ShowResultUI(int _ingrnum)
    {
        if (UIBox[ingnum].activeSelf)
            UIBox[ingnum].SetActive(false);

        ingrnum = _ingrnum;
        RUIBox[ingrnum].SetActive(true);

        Debug.Log("종료된 진행 UI:" + UIBox[ingnum].name);
        Debug.Log("진행중인 결과 UI:" + RUIBox[ingrnum].name);

        ingnum += 1;
    }
}
