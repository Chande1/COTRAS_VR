using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class TPSGameController : MonoBehaviour
{
    [SerializeField]
    bool usemultidisplay;

    private void Awake()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRSettings.enabled = false;
    }

    private void Start()
    {
        if(usemultidisplay)
        {
            Debug.Log("displays connected: " + Display.displays.Length);

            if (Display.displays.Length > 1)
                Display.displays[1].Activate(1920,1080,60);

            if (Display.displays.Length > 2)
                Display.displays[2].Activate(1920, 1080, 60);
        }

    }
}
