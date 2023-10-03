using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComController : MonoBehaviour
{
    [SerializeField]
    float movespeed;
    [SerializeField]
    GameObject HeadDirect;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            ChangeRotation();
        }

        // w ->앞
        if (Input.GetKey(KeyCode.W))
        {
            transform.transform.Translate(Vector3.forward*movespeed*Time.deltaTime);
        }
        // s->뒤
        if (Input.GetKey(KeyCode.S))
        {
            transform.transform.Translate(Vector3.back* movespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.transform.Translate(Vector3.left * movespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.transform.Translate(Vector3.right * movespeed * Time.deltaTime);
        }
    }

    private void ChangeRotation()
    {
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, HeadDirect.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
