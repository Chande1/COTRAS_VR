using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FastFoodManager : MonoBehaviour
{
    [Header("플레이어 손 위치")]
    [SerializeField] GameObject Player;
    [SerializeField] GameObject RightHand;
    [SerializeField] Transform HandPos;
    [SerializeField] GameObject Card;

    [Header("메뉴 패널")]
    [SerializeField] GameObject InorOut;
    [SerializeField] GameObject MenuButtons;
    [SerializeField] GameObject BurgerSelectPopup;
    [SerializeField] GameObject SidMenuSelectPopup;
    [SerializeField] GameObject ClearOrderButton;
    [SerializeField] GameObject SubmitOrderButton;
    [SerializeField] GameObject Payment;
    [SerializeField] GameObject End;

    [Header("상호작용 오브젝트")]
    [SerializeField] GameObject HamburgerTray;

    [Header("오브젝트 위치")]
    [SerializeField] GameObject StartPos;
    [SerializeField] GameObject CardPos;

    bool havecard = false;



    private void Awake()
    {
        CardPos.SetActive(false);
        Player.transform.position = StartPos.transform.position;  //시작 위치로 플레이어 이동
        Player.transform.eulerAngles = StartPos.transform.eulerAngles;
        HamburgerTray.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        OpenPanelHideButton(BurgerSelectPopup, MenuButtons);

        if (InorOut.activeSelf)
        {
            Player.GetComponent<PlayerController>().enabled = false;
        }
        if(SidMenuSelectPopup.activeSelf)
        {
            if(ClearOrderButton.GetComponent<BoxCollider>().enabled)
            {
                ClearOrderButton.GetComponent<BoxCollider>().enabled = false;
            }
            if (SubmitOrderButton.GetComponent<BoxCollider>().enabled)
            {
                SubmitOrderButton.GetComponent<BoxCollider>().enabled = false;
            }
            OpenPanelHideButton(SidMenuSelectPopup, MenuButtons);

        }
        else
        {
            if (!ClearOrderButton.GetComponent<BoxCollider>().enabled)
            {
                ClearOrderButton.GetComponent<BoxCollider>().enabled = true;
            }
            if (!SubmitOrderButton.GetComponent<BoxCollider>().enabled)
            {
                SubmitOrderButton.GetComponent<BoxCollider>().enabled = true;
            }
        }
        if(Payment.activeSelf)
        {
            if (!havecard)
            {
                CardPos.SetActive(true);

                GameObject mycard = GameObject.Instantiate(Card, HandPos.position, Card.transform.rotation);
                mycard.transform.parent = RightHand.transform;
                havecard = true;
            }
        }
        else if(End.activeSelf)
        {
            if (!HamburgerTray.activeSelf)
            {
                Player.GetComponent<PlayerController>().ResetWBox();
                Player.GetComponent<PlayerController>().enabled = true;

                HamburgerTray.SetActive(true);

            }
               
        }
        
    }

    public void OpenPanelHideButton(GameObject _panel,GameObject _hidebuttons)
    {
        if(_panel.activeSelf)
        {
            if(_hidebuttons.transform.GetChild(0).GetComponent<BoxCollider>().enabled)
            {
                for (int i = 0; i < _hidebuttons.transform.childCount; i++)
                {
                    if (_hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>())
                    {
                        _hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
        }
        else
        {
            if(!_hidebuttons.transform.GetChild(0).GetComponent<BoxCollider>().enabled)
            {
                for (int i = 0; i < _hidebuttons.transform.childCount; i++)
                {
                    if (_hidebuttons.transform.GetChild(i).GetComponent<BoxCollider>())
                    {
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
