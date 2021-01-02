using Sirenix.OdinInspector;
using UnityEngine;

public class AmmoSupply : MonoBehaviour
{
    [SerializeField, PropertyRange(0, "max")]
    private int _count = 30;
    public int Count
    {
        get => _count;
        set
        {
            if (_count != value)
            {
                _count = Mathf.Clamp(value, 0, max);
                if (uiBar)
                    uiBar.Value = 1f * Count / max;
                if (_count == 0)
                    Destroy(gameObject);
            }
        }
    }
    [SerializeField]
    private int max = 30;

    [SerializeField]
    private ProgressBar uiBar;

    private void OnValidate()
    {
        if (uiBar)
            uiBar.Value = 1f * Count / max;
    }

    private void OnEnable() => GetComponent<Parentable>().onSetParent += AmmoSupply_onSetParent;
    private void OnDisable() => GetComponent<Parentable>().onSetParent -= AmmoSupply_onSetParent;

    private void AmmoSupply_onSetParent(Parentable parentable) => GetComponent<Collider>().enabled = !parentable.Parent;
}
