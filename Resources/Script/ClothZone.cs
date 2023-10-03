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

    GameObject CW;  //������

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
            //����
            if (other.tag == zonename)
            {
                CW.GetComponent<CountWindow>().CorrectAnswer(); //���� ó��
                //Destroy(other.gameObject);//�Ƿ� ������Ʈ ����
                //���� ����Ʈ ����
                GameObject c_effect=GameObject.Instantiate(O_effect, transform.position, transform.rotation);
                c_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                Destroy(c_effect, 2f);                          //2�� �Ŀ� ����
                keepclothcount += 1;    //������ �������� �� �߰�
                //�ݶ��̴� ����
                Destroy(other.gameObject.GetComponent<BoxCollider>());
            }
            //����
            else
            {
                CW.GetComponent<CountWindow>().InCorrectAnswer(); //���� ó��
                Destroy(other.gameObject);//�Ƿ� ������Ʈ ����
                //���� ����Ʈ ����
                GameObject c_effect = GameObject.Instantiate(X_effect, transform.position, transform.rotation);
                c_effect.GetComponent<ParticleSystem>().Play(); //��ƼŬ ���
                Destroy(c_effect, 2f);                          //2�� �Ŀ� ����
            }
        }
        
    }

    //���� �������� ���� ��ǥ 
    public Transform GetKeepClothPos()
    {
        return clothspos[keepclothcount];
    }
}
