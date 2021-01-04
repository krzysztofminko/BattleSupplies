﻿using NodeCanvas.StateMachines;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FSMOwner))]
public class Soldier : MonoBehaviour, ITeam, IPickable, ILoadable
{
    public enum MoveStatus { Running, Success, Failure}

    public Bullet amunition;
    
    [SerializeField, PropertyRange(0, "_ammoMax")]
    private int _ammoCount = 20;
    public int AmmoCount
    {
        get => _ammoCount;
        set
        {
            if(_ammoCount != value)
            {
                _ammoCount = Mathf.Clamp(value, 0, AmmoMax);
                if (ammoBar)
                    ammoBar.Value = 1f * AmmoCount / AmmoMax;
            }
        }
    }

    [SerializeField, Min(0)]
    private int _ammoMax = 20;
    public int AmmoMax => _ammoMax;

    [SerializeField, Min(0)]
    public int ammoResupplyTrigger = 10;

    [SerializeField]
    private ProgressBar ammoBar;

    [SerializeField]
    private Rigidbody ragdollCenter;
    [SerializeField]
    private List<Rigidbody> ragdollParts;

    [ReadOnly]
    public Squad squad;
    [ReadOnly]
    public Vector3 targetPosition;

    [SerializeField, ReadOnly]
    private int _team;
    public int Team { get => _team; set => _team = value; }
    
    private NavMeshAgent nmAgent;
    private Animator animator;
    private FSMOwner fsmOwner;

    private void OnValidate()
    {
        if (ammoBar)
            ammoBar.Value = 1f * AmmoCount / AmmoMax;
    }

    private void Awake()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fsmOwner = GetComponent<FSMOwner>();
        SetRagdoll(false);
    }

    public MoveStatus Move(Transform target, float distance = 1) => Move(target.position, distance);

    public MoveStatus Move(Vector3 targetPosition, float distance = 1)
    {
        if (nmAgent.enabled)
        {
            nmAgent.SetDestination(targetPosition);
            if (nmAgent.pathPending)
            {
                return MoveStatus.Running;
            }
            else if (nmAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                if (nmAgent.remainingDistance <= distance)
                {
                    Stop();
                    return MoveStatus.Success;
                }
                else
                {
                    animator.SetFloat("MoveSpeed", nmAgent.velocity.magnitude);
                    return MoveStatus.Running;
                }
            }
            else
            {
                return MoveStatus.Failure;
            }
        }
        return MoveStatus.Failure;
    }

    public void Stop(bool immediately = false)
    {
        animator.SetFloat("MoveSpeed", 0);
        if (nmAgent.enabled)
            nmAgent.SetDestination(immediately ? transform.position : (transform.position + transform.forward * nmAgent.stoppingDistance));
    }

    //TODO: Figure out how to separate DyingSoldier's ragdoll object from Soldier object (copying bone's positions and rotations from original, to newly spawned ragdoll?)
    public void Die()
    {
        SetRagdoll(true);
        if (squad)
            squad.RemoveSoldier(this);
        gameObject.layer = LayerMask.NameToLayer("DyingSoldier");
        nmAgent.enabled = fsmOwner.enabled = animator.enabled = false;
    }

    public void SetRagdoll(bool active)
    {
        for (int i = 0; i < ragdollParts.Count; i++)
        {
            ragdollParts[i].isKinematic = !active;
            ragdollParts[i].GetComponent<Collider>().enabled = active;
        }
    }

    public void FreezeRagdollCenter(bool freeze)
    {
        if (ragdollCenter)
        {
            if (freeze)
                ragdollCenter.transform.localPosition = Vector3.zero;
            ragdollCenter.constraints = freeze ? RigidbodyConstraints.FreezePosition : RigidbodyConstraints.None;
        }
    }

    public void OnLoad() => OnPick();
    public void OnUnload() => OnPut();

    public void OnPick()
    {
        //GetComponent<Soldier>().SetRagdoll(false);
        GetComponent<Soldier>().FreezeRagdollCenter(true);
        GetComponent<Collider>().enabled = false;
    }

    public void OnPut()
    {
        //GetComponent<Soldier>().SetRagdoll(true);
        GetComponent<Soldier>().FreezeRagdollCenter(false);
        GetComponent<Collider>().enabled = true;
    }
}
