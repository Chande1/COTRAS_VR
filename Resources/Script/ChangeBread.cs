using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBread : MonoBehaviour
{
    [SerializeField] Mesh nonbread;
    [SerializeField] Mesh eggbread;
    [SerializeField] Mesh friedbread;
    MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bowl"))
        {
            meshFilter.sharedMesh = eggbread;
        }
        else if(other.CompareTag("goal"))
        {
            meshFilter.sharedMesh = friedbread;
        }
    }
}
