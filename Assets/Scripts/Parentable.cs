using Sirenix.OdinInspector;
using UnityEngine;

public class Parentable : MonoBehaviour
{
    public delegate void OnSetParent(Parentable parentable);
    public event OnSetParent onSetParent;

    [SerializeField, ReadOnly]
    private Transform _parent;
    public Transform Parent
    {
        get => _parent;
        set
        {
            _parent = value;

            transform.parent = value;
            if (value)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, LayerMask.GetMask("Ground")))
            {
                transform.position = hitInfo.point;
            }
            onSetParent?.Invoke(this);
        }
    }

}
