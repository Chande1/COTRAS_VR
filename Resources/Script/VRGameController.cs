using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class VRGameController : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("VR enable");
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        XRSettings.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("displays connected: " + Display.displays.Length);

        if (Display.displays.Length > 1)
            Display.displays[1].Activate(1920, 1080, 60);

        if (Display.displays.Length > 2)
            Display.displays[2].Activate(1920, 1080, 60);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
