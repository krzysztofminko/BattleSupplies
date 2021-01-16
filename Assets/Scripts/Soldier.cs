using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VehiclePhysics;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(FSMOwner)), RequireComponent(typeof(Ragdoll))]
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

    [SerializeField, Required]
    private ProgressBar ammoBar;

    [ReadOnly]
    public Squad squad;
    [ReadOnly]
    public Vector3 targetPosition;
    [ReadOnly]
    public Vehicle targetVehicle;

    [SerializeField, ReadOnly]
    private int _team;
    public int Team { get => _team; set => _team = value; }
    [SerializeField, ReadOnly]
    private bool _isDying;
    public bool IsDying { get => _isDying; private set => _isDying = value; }

    private NavMeshAgent nmAgent;
    private Animator animator;
    private FSMOwner fsmOwner;
    private Ragdoll ragdoll;

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
        ragdoll = GetComponent<Ragdoll>();
        ragdoll.SetRagdoll(false);
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
        IsDying = true;
        ragdoll.SetRagdoll(true);
        if (squad)
            squad.RemoveSoldier(this);
        gameObject.layer = LayerMask.NameToLayer("DyingSoldier");
        nmAgent.enabled = fsmOwner.enabled = animator.enabled = false;
    }

    public void OnLoad()
    {
        ragdoll.SetRagdoll(false, true);
        GetComponent<Collider>().enabled = false;
        nmAgent.enabled = false;
    }

    public void OnUnload() => OnPut();

    public void OnPick()
    {
        ragdoll.SetRagdoll(true, true);
        GetComponent<Collider>().enabled = false;
        nmAgent.enabled = false;
    }

    public void OnPut()
    {
        //TODO: Fix ragdoll positioned far from selecting collider
        ragdoll.SetRagdoll(true, false);
        GetComponent<Collider>().enabled = true; 
        nmAgent.enabled = !IsDying;
    }
}
