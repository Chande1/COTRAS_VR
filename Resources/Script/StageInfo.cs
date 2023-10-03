using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
    public static StageInfo stageinfo;

    public int findAandOArriveCount;
    public int findAandORoundCount;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(stageinfo==null)
        {
            stageinfo = this;
        }
    }
    public void SetFAOArriveInfo(int _acount)
    {
       findAandOArriveCount = _acount;
    }
    public void SetFAORouneInfo(int _rcount)
    {
        findAandORoundCount = _rcount;
    }
}
