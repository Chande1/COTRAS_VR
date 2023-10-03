using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] GameObject objectprefab;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] int objectcountx;
    [SerializeField] int objectcounty;

    public void GenerateObjects()
    {

        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }


        for (int i = 0; i < objectcountx; i++)
        {
            for(int j=0;j<objectcounty;j++)
            {
                var newobj = Instantiate(objectprefab);
                newobj.transform.SetParent(gameObject.transform);
                newobj.transform.localPosition = new Vector3(j*y, 0f, i * x);
                newobj.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
