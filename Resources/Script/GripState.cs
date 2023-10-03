using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GripStateValue
{
    None=0,
    Gripping,
    GripStop
}

public class GripState : MonoBehaviour
{
    //��ü�� ��� �ִ� ����
    [SerializeField] GripStateValue gripstate;

    public void GripNone()
    {
        gripstate = GripStateValue.None;
    }

    public void Gripping()
    {
        gripstate = GripStateValue.Gripping;
    }

    public void GripStop()
    {
        gripstate = GripStateValue.GripStop;
    }

    public GripStateValue GetGripStateValue()
    {
        return gripstate;
    }
}
