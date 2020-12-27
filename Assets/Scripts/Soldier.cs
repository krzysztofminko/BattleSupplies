using NodeCanvas.StateMachines;
using NodeCanvas.Tasks.Actions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
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
                    ammoBar.fillAmount = 1f * AmmoCount / AmmoMax;
            }
        }
    }

    [SerializeField, Min(0)]
    private int _ammoMax = 20;
    public int AmmoMax => _ammoMax;

    [SerializeField, Min(0)]
    public int ammoResupplyTrigger = 10;

    [SerializeField]
    private Image ammoBar;

    [ReadOnly]
    public Squad squad;
    [ReadOnly]
    public Vector3 targetPosition;

    [SerializeField, ReadOnly]
    private int _team;
    public int Team { get => _team; set => _team = value; }

    public bool IsDestroyed { get; private set; }
    

    private NavMeshAgent nmAgent;
    private Animator animator;

    private void OnValidate()
    {
        if (ammoBar)
            ammoBar.fillAmount = 1f * AmmoCount / AmmoMax;
    }

    private void Awake()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public MoveStatus Move(Transform target, float distance = 1) => Move(target.position, distance);

    public MoveStatus Move(Vector3 targetPosition, float distance = 1)
    {
        if (nmAgent.enabled && !IsDestroyed)
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
        if (nmAgent.enabled && !IsDestroyed)
            nmAgent.SetDestination(immediately ? transform.position : (transform.position + transform.forward * nmAgent.stoppingDistance));
    }

    private void OnDestroy()
    {
        IsDestroyed = true;
    }

    private void OnDisable()
    {
        squad?.RemoveSoldier(this);
    }
}
