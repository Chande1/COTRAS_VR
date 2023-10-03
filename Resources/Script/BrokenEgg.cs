using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEgg : MonoBehaviour
{
    [SerializeField] GameObject NewEgg;
    [SerializeField] GameObject BrokenEggA;
    [SerializeField] GameObject BrokenEggB;

    private void Awake()
    {
        NewEgg = this.gameObject;
        BrokenEggA.SetActive(false);
        BrokenEggB.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            BrokenEggA.SetActive(true);
            BrokenEggB.SetActive(true);
            NewEgg.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
