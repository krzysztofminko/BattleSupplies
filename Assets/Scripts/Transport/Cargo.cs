using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Cargo : MonoBehaviour, IPickable, ILoadable
{
    [SerializeField, ReadOnly]
    private bool _available = true;
    public bool Available { get => _available; private set => _available = value; }

    public void OnLoad() => OnPick();
    public void OnUnload() => OnPut();

    public void OnPick()
    {
        GetComponent<Collider>().enabled = GetComponent<NavMeshObstacle>().enabled = Available = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void OnPut()
    {
        GetComponent<Collider>().enabled = GetComponent<NavMeshObstacle>().enabled = Available = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
