using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceCube
{
    Entrance=0, //현관
    LivingRoom, //거실
    Kitchen,    //부엌
    WashRoom,   //화장실
    MainRoom    //안방
}

public class PlaceInfo : MonoBehaviour
{
    //장소선택
    public PlaceCube pc;
    [SerializeField] GameObject psign;
    [SerializeField] GameObject placegate;
    [SerializeField] GameObject[] othergate;

    private void Start()
    {
        placegate.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="RHand"||other.tag=="LHand")
        {
            GameObject.Find("VirusGameManager").GetComponent<VirusGameManager>().PlaceCheck(pc);

            if(psign!=null)
            {
                psign.SetActive(true);
                Destroy(psign, 5f);
            }

            if(placegate!=null)
            {
                placegate.SetActive(true);
                for(int i=0;i<othergate.Length;i++)
                {
                    othergate[i].SetActive(false);
                }
            }
        }
    }


}
