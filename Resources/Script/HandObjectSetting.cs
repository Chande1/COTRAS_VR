using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(InteractableHoverEvents))]
[RequireComponent(typeof(HoverGravity))]
[RequireComponent(typeof(VelocityEstimator))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(SteamVR_Skeleton_Poser))]
public class HandObjectSetting : MonoBehaviour
{
    [Header("기본세팅")]
    [TextArea]
    [SerializeField] string memo="1.리지드바디 중력 체크/키네마틱X\n2.인터렉테이블 상호작용시 손 가리기X/하이라이트 숨기기 X";
}
