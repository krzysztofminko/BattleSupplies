using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField, Required]
    private Transform ragdollRoot;
    [SerializeField]
    private List<Rigidbody> ragdollParts;


    [SerializeField, HideInInspector]
    private Vector3 savedRagdollLocalPosition;
    [SerializeField, HideInInspector]
    private bool resetRagdollLocalPosition;


    public void SetRagdoll(bool active, bool groundRoot = false)
    {
        //Reset root position
        if (!active && resetRagdollLocalPosition)
        {
            ragdollRoot.localPosition = savedRagdollLocalPosition;
            resetRagdollLocalPosition = false;
        }

        //Enable/disable ragdoll effect
        for (int i = 0; i < ragdollParts.Count; i++)
        {
            ragdollParts[i].isKinematic = !active;
            ragdollParts[i].GetComponent<Collider>().enabled = active;
        }

        //Set root position same as this.transform
        if (groundRoot)
        {
            savedRagdollLocalPosition = ragdollRoot.localPosition;
            resetRagdollLocalPosition = true;
            ragdollRoot.position = transform.position;
        }

        //Freeze/unfreeze root rigidbody
        ragdollRoot.GetComponent<Rigidbody>().constraints = groundRoot ? RigidbodyConstraints.FreezePosition : RigidbodyConstraints.None;
    }
}
