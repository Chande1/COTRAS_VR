using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerObject : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] AudioSource cleanersound;

    private void Update()
    {
        if(GetComponent<GripState>().GetGripStateValue()==GripStateValue.Gripping)
        {
            if(!cleanersound.isPlaying)
                cleanersound.Play();
        }
        else
        {
            if (cleanersound.isPlaying)
                cleanersound.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Duest")
        {
            timer.GetComponent<GripTimer>().StopTimer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Duest")
        {
            timer.GetComponent<GripTimer>().StartTimer();
        }
    }
}
