using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] float rotspeed;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotspeed * Time.deltaTime, 0));
    }
}
