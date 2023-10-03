using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAAndOManager : MonoBehaviour
{
    [SerializeField] GameObject StartWindow;
    [SerializeField] GameObject SelfWindow;
    [SerializeField] GameObject ObjectWindow;

    private void Awake()
    {
        SelfWindow.SetActive(false);
        ObjectWindow.SetActive(false);
    }

    public void OpenWindow(GameObject _window)
    {
        _window.SetActive(true);
    }

    public void CloseWindow(GameObject _window)
    {
        _window.SetActive(false);
    }

}
