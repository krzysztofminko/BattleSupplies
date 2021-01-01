using Sirenix.OdinInspector;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Cargo : MonoBehaviour, IPickable
{
    [SerializeField, ReadOnly]
    private bool _available = true;
    public bool Available { get => _available; private set => _available = value; }

    public Action<bool> onSetParent;

    public Cargo Pick(Transform parentTo)
    {
        SetParent(parentTo);
        return this;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        if (parent)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, LayerMask.GetMask("Ground")))
        {
            transform.position = hitInfo.point;
        }
        GetComponent<Collider>().enabled = Available = !parent;
        onSetParent?.Invoke(parent);
    }
}
