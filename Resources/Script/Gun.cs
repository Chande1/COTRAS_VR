
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPivot;
    [SerializeField] float shootingspeed;
    [SerializeField] GameObject effect;

    [SerializeField]Hand hand;
    SteamVR_Skeleton_Poser skeletonPoser;

    [SerializeField] bool grab;

    private void Awake()
    {
        grab = false;
        skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();
    }

    private void Update()
    {
        if(grab&&hand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.RightHand)|| hand.grabPinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Fire();
        }
    }


    public void message()
    {
        Debug.Log("now");

    }

    public void HandFixation()
    {
        gameObject.transform.parent = hand.transform;
        if (skeletonPoser != null && hand.skeleton != null)
        {
            hand.skeleton.BlendToPoser(skeletonPoser, 0.1f);
        }
        grab = true;
    }

    void Fire()
    {
        //Debug.Log("Fire");
        //Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
        GameObject newbullet = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation);
        Rigidbody bulletrb = newbullet.GetComponent<Rigidbody>();
        bulletrb.velocity = barrelPivot.forward * shootingspeed;
        Destroy(newbullet, 2f);
        

        effect.SetActive(true);
        effect.GetComponent<ParticleSystem>().Play();
    }

}