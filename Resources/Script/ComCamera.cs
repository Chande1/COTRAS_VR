using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComCamera : MonoBehaviour
{
    private float xRotate, yRotate, xRotateMove, yRotateMove;
    public float rotateSpeed = 50.0f;

    private void Update()
    {
        if (Input.GetMouseButton(1)) // 클릭한 경우
        {
            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed; ;
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed; ;

            yRotate = yRotate + yRotateMove;
            //xRotate = transform.eulerAngles.x + xRotateMove; 
            xRotate = xRotate + xRotateMove;

            xRotate = Mathf.Clamp(xRotate, -90, 90); // 위, 아래 고정

            //transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

            Quaternion quat = Quaternion.Euler(new Vector3(xRotate, yRotate, 0));
            transform.rotation
                = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime /* x speed */);
        }
    }
}
