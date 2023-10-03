using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField]string nextSceneName;

    public void TPSSceneMove()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void VRSceneMove()
    {
        Valve.VR.SteamVR_LoadLevel.Begin(nextSceneName);
    }
}
