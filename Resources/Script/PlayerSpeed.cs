using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    private CharacterController control;
    [SerializeField] private int playerspeed;

    // Start is called before the first frame update
    void Awake()
    {
        control = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativeforward = transform.TransformDirection(Vector3.forward);
        control.Move(relativeforward * playerspeed * Time.deltaTime);
    }
}
