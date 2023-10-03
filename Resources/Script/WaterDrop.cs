using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    ParticleSystem myparticle;

    void Start()
    {
        myparticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Angle(Vector3.down,transform.forward)<=90f|| Vector3.Angle(Vector3.down, transform.right) <= 90f)
        {
            myparticle.Play();
        }
        else
        {
            Debug.Log("angle:" + Vector3.Angle(Vector3.down, transform.forward));
            myparticle.Stop();
        }
    }
}
