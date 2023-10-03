using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeManager : MonoBehaviour
{
    [Header("�÷��̾� �� ��ġ")]
    [SerializeField] GameObject Player;
    [SerializeField] GameObject RightHand;
    [SerializeField] Transform HandPos;
    [SerializeField] GameObject Card;

    [Header("�޴� �г�")]
    [SerializeField] GameObject MenuSelect;
    [SerializeField] GameObject MenuButtons;
    [SerializeField] GameObject OrderPopup;
    [SerializeField] GameObject PaymentPopup;
    [SerializeField] GameObject End;

    [Header("��ȣ�ۿ� ������Ʈ")]
    [SerializeField] GameObject CoffeeTray;

    [Header("������Ʈ ��ġ")]
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject CardPos;

    bool havecard = false;



    private void Awake()
    {
        CardPos.SetActive(false);
        Player.transform.position = StartPos.transform.position;  //���� ��ġ�� �÷��̾� �̵�
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        CoffeeTray.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        OpenPanelHideButton(OrderPopup, MenuButtons);

        if (MenuSelect.activeSelf)
        {
            Player.GetComponent<PlayerController>().enabled = false;
        }
        if (PaymentPopup.activeSelf)
        {
            if (!havecard)
            {
                OpenPanelHideButton(PaymentPopup, MenuButtons);
                CardPos.SetActive(true);

                GameObject mycard = GameObject.Instantiate(Card, HandPos.position, Card.transform.rotation);
                mycard.transform.parent = RightHand.transform;
                havecard = true;
            }
        }
        else if (End.activeSelf)
        {
            if (!CoffeeTray.activeSelf)
            {
                Player.GetComponent<PlayerController>().ResetWBox();
                Player.GetComponent<PlayerController>().enabled = true;
                CoffeeTray.SetActive(true);

            }

        }

    }

    public void OpenPanelHideButton(GameObject _panel, GameObject _hidebuttons)
    {
        if (_panel.activeSelf)
        {
            for (int i = 0; i < _hidebuttons.transform.childCount; i++)
            {
                if (_hidebuttons.transform.GetChild(i).gameObject.activeSelf)
                {
                    if (_hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>())
                    {
                        //Debug.Log(i + ":" + _hidebuttons.transform.GetChild(i).gameObject + "'s box is enable.");
                        _hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _hidebuttons.transform.childCount; i++)
            {
                if (_hidebuttons.transform.GetChild(i).gameObject.activeSelf)
                {
                    if (_hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>())
                    {
                        //Debug.Log(i+":"+_hidebuttons.transform.GetChild(i).gameObject + "'s box is able.");
                        _hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                    }
                }   
            }
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
