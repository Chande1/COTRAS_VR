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
    [Header("�⺻����")]
    [TextArea]
    [SerializeField] string memo="1.������ٵ� �߷� üũ/Ű�׸�ƽX\n2.���ͷ����̺� ��ȣ�ۿ�� �� ������X/���̶���Ʈ ����� X";
}
