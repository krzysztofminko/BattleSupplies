using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class Cargo : MonoBehaviour, IPickable
{
    [SerializeField, ReadOnly]
    private bool _available = true;
    public bool Available { get => _available; private set => _available = value; }

    [SerializeField, PropertyRange(0, "ammoMax")]
    private int _ammoCount = 100;
    public int AmmoCount 
    { 
        get => _ammoCount;
        set
        {
            if(_ammoCount != value)
            {
                _ammoCount = Mathf.Clamp(value, 0 , ammoMax);
                if (ammoBar)
                    ammoBar.fillAmount = 1f * AmmoCount / ammoMax;
                if (_ammoCount == 0)
                    Destroy(gameObject);
            }
        }
    }
    [SerializeField]
    private int ammoMax = 30;

    [SerializeField]
    private Image ammoBar;

    private void OnValidate()
    {
        if (ammoBar)
            ammoBar.fillAmount = 1f * AmmoCount / ammoMax;
    }

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
    }
}
