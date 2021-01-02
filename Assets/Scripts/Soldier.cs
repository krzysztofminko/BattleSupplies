using NodeCanvas.StateMachines;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VehiclePhysics;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FSMOwner))]
public class Soldier : MonoBehaviour, ITeam
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
    private List<Rigidbody> ragdollParts;

    [ReadOnly]
    public Squad squad;
    [ReadOnly]
    public Vector3 targetPosition;
    [ReadOnly]
    public Vehicle targetVehicle;

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
        GetComponent<Parentable>().onSetParent += Soldier_onSetParent;
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

    public void Die()
    {
        SetRagdoll(true);
        nmAgent.enabled = fsmOwner.enabled = animator.enabled = false;
        if (squad)
            squad.RemoveSoldier(this);
        gameObject.layer = LayerMask.NameToLayer("DyingSoldier");
        GetComponent<Parentable>().onSetParent += DyingSoldier_onSetParent;
        GetComponent<Parentable>().onSetParent -= Soldier_onSetParent;
    }

    public void Revive()    //Unused, untested
    {
        SetRagdoll(false);
        nmAgent.enabled = fsmOwner.enabled = animator.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Soldier");
        GetComponent<Parentable>().onSetParent -= DyingSoldier_onSetParent;
        GetComponent<Parentable>().onSetParent += Soldier_onSetParent;
    }

    private void DyingSoldier_onSetParent(Parentable parentable)
    {
        GetComponent<Collider>().enabled = !parentable.Parent;
        SetRagdoll(!parentable.Parent);
    }

    private void Soldier_onSetParent(Parentable parentable)
    {
        GetComponent<Collider>().enabled = nmAgent.enabled = !parentable.Parent;
        if (!parentable.Parent)
            targetVehicle = null;

    }

    private void SetRagdoll(bool active)
    {
        for (int i = 0; i < ragdollParts.Count; i++)
        {
            ragdollParts[i].isKinematic = !active;
            ragdollParts[i].GetComponent<Collider>().enabled = active;
        }
    }
}
