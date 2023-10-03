using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSPoint : MonoBehaviour
{
    [Header("�ǻ� ���� ����Ʈ ������Ʈ")]
    [Tooltip("�ʼ� �Է°�")]
    [SerializeField] GameObject cspoint;        //�ǻ� ���� ����Ʈ
    [Tooltip("�ʼ� �Է°�")]
    [SerializeField] GameObject[] csp_child;      //�ǻ� ���� ����Ʈ �ڽ� 
    [Header("�ǻ� ������Ʈ")]
    [SerializeField] GameObject[] upcloths;     //����
    [SerializeField] GameObject[] downcloths;   //����
    [SerializeField] GameObject[] socks;        //�縻
    [SerializeField] GameObject[] shoose;       //�Ź�
    [Header("�ǻ� ������Ʈ ����")]
    [Tooltip("�ʼ� �Է°�")]
    [SerializeField] int c_count;    //��ü �ǻ� ����(����24)
    [SerializeField] int uc_count;   //���� ����
    [SerializeField] int dc_count;   //���� ����
    [SerializeField] int sk_count;   //�縻 ����
    [SerializeField] int sh_count;   //�Ź� ����
    [Header("�ǻ� ���� ��� ����")]
    [Range(0, 5)]
    [SerializeField] int clth_min_max;  //�ǻ� ���� ����
    [Header("������ �ǻ� ������Ʈ")]
    [Tooltip("�ʼ� �Է°�")]
    [SerializeField] GameObject[] select_clth;  //���õ� �ǻ� ������Ʈ
    
    GameObject SelectCloth; //���õ� �ʵ��� ������ ����� �� �θ� ��ü

    void Awake()
    {
        //���õ� �ʵ��� ������ ����� �� �θ� ��ü ����
        SelectCloth = new GameObject("SelectCloth");
        //���� ����Ʈ ����
        for (int i = 0; i < cspoint.transform.childCount; i++)
        {
            csp_child[i] = cspoint.transform.GetChild(i).gameObject;
        }
        //������ �ǻ� ���� ����
        RandomClothCount();
        //������ �ǻ� ���� ����
        SelectRandomCloth();
        //������ �ǻ� �ڸ� ��ġ
        RandomClothSpawn();
    }


    //������ �ǻ� ���� ����
    private void RandomClothCount()
    {
        uc_count = Random.Range(c_count / 4 - clth_min_max, upcloths.Length);
        dc_count = Random.Range(c_count / 4 - clth_min_max, downcloths.Length);

        //���ǿ� ������ ������ ��ü ������ 3/4�̻� �϶�
        if ((uc_count + dc_count) >= (c_count / 4) * 3)
        {
            sk_count = Random.Range(1, c_count - (uc_count + dc_count) - 1);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
        //���ǿ� ������ ������ ��ü�� �� �̻� �϶�
        else if ((uc_count + dc_count) >= (c_count / 2))
        {
            sk_count = Random.Range(c_count / 4 - clth_min_max, socks.Length);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
        //���ǿ� ������ ������ ��ü�� �� �̸� �϶�
        else
        {
            sk_count = Random.Range(c_count / 4 + clth_min_max, socks.Length);
            sh_count = c_count - (uc_count + dc_count + sk_count);
        }
    }

    //���� �ǻ� ���� ����
    private void SelectRandomCloth()
    {
        for(int i=0;i<c_count;i++)
        {
            //���� ����
            if(i<uc_count)
            {
                select_clth[i] = upcloths[i];
            }
            //���� ����
            else if(i<uc_count+dc_count)
            {
                select_clth[i] = downcloths[i-uc_count];
            }
            //�縻
            else if (i < uc_count + dc_count+sk_count)
            {
                select_clth[i] = socks[i - (uc_count+dc_count)];
            }
            //�Ź�
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

    //������ �ǻ� �ڸ� ��ġ
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
