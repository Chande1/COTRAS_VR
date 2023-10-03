using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeManager : MonoBehaviour
{
    [Header("플레이어 손 위치")]
    [SerializeField] GameObject Player;
    [SerializeField] GameObject RightHand;
    [SerializeField] Transform HandPos;
    [SerializeField] GameObject Card;

    [Header("메뉴 패널")]
    [SerializeField] GameObject MenuSelect;
    [SerializeField] GameObject MenuButtons;
    [SerializeField] GameObject OrderPopup;
    [SerializeField] GameObject PaymentPopup;
    [SerializeField] GameObject End;

    [Header("상호작용 오브젝트")]
    [SerializeField] GameObject CoffeeTray;

    [Header("오브젝트 위치")]
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject CardPos;

    bool havecard = false;



    private void Awake()
    {
        CardPos.SetActive(false);
        Player.transform.position = StartPos.transform.position;  //시작 위치로 플레이어 이동
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
