using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUIManager : MonoBehaviour
{
    [Header("��µ� UI�迭")]
    [SerializeField]GameObject[] UIBox;     //��µ� UI�迭
    [Header("��µǰ� �ִ� UI ����")]
    [SerializeField] int ingnum;
    [Header("��µ� ���UI�迭")]
    [SerializeField] GameObject[] RUIBox;   //��µ� ���UI�迭
    [Header("��µǰ� �ִ� ��� UI ����")]
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
            //���, ����
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
            //���, ����
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
            //���, ����
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
            //���, ����
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
            //���, ����
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

        Debug.Log("����� ���� UI:" + UIBox[ingnum].name);
        Debug.Log("�������� ��� UI:" + RUIBox[ingrnum].name);

        ingnum += 1;
    }
}
