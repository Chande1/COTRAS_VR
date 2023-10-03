using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineManager : MonoBehaviour
{
    [SerializeField]Material outline;

    SkinnedMeshRenderer renderers;
    List<Material> materialList = new List<Material>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RHand" || other.tag == "LHand")
        {
            Debug.Log("touch: " + gameObject);
            renderers = GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderers.sharedMaterials);
            materialList.Add(outline);

            renderers.materials = materialList.ToArray();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RHand" || other.tag == "LHand")
        {
            SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);

            renderer.materials = materialList.ToArray();
        }
    }

    void Start()
    {
        outline = new Material(Shader.Find("Custom/OutLine"));
    }
}
