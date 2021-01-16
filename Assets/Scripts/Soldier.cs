using NodeCanvas.StateMachines;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VehiclePhysics;

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

    [SerializeField, Required]
    private ProgressBar ammoBar;

    [SerializeField, Required]
    private Transform ragdollRoot;
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
    [SerializeField, ReadOnly]
    private bool _isDying;
    public bool IsDying { get => _isDying; private set => _isDying = value; }

    [SerializeField, HideInInspector]
    private Vector3 savedRagdollLocalPosition;
    [SerializeField, HideInInspector]
    private bool resetRagdollLocalPosition;

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

    public void Die()
    {
        IsDying = true;
        SetRagdoll(true);
        if (squad)
            squad.RemoveSoldier(this);
        gameObject.layer = LayerMask.NameToLayer("DyingSoldier");
        nmAgent.enabled = fsmOwner.enabled = animator.enabled = false;
    }

    //TODO: Create Ragdoll component
    public void SetRagdoll(bool active, bool groundRoot = false)
    {
        if (!active && resetRagdollLocalPosition)
        {
            ragdollRoot.localPosition = savedRagdollLocalPosition;
            resetRagdollLocalPosition = false;
        }
        for (int i = 0; i < ragdollParts.Count; i++)
        {
            ragdollParts[i].isKinematic = !active;
            ragdollParts[i].GetComponent<Collider>().enabled = active;
        }
        if (active)
        {
            if (groundRoot)
            {
                savedRagdollLocalPosition = ragdollRoot.localPosition;
                resetRagdollLocalPosition = true;
                ragdollRoot.position = transform.position;
            }
            ragdollRoot.GetComponent<Rigidbody>().constraints = groundRoot ? RigidbodyConstraints.FreezePosition : RigidbodyConstraints.None;
        }
    }

    public void OnLoad()
    {
        GetComponent<Soldier>().SetRagdoll(false);
        GetComponent<Collider>().enabled = false;
        nmAgent.enabled = false;
        ragdollRoot.localPosition = Vector3.zero;
    }

    public void OnUnload() => OnPut();

    public void OnPick()
    {
        GetComponent<Soldier>().SetRagdoll(true, true);
        GetComponent<Collider>().enabled = false;
        nmAgent.enabled = false;
    }

    public void OnPut()
    {
        GetComponent<Soldier>().SetRagdoll(true, false);
        GetComponent<Collider>().enabled = true; 
        nmAgent.enabled = !IsDying;
    }
}
