using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HideMonoScript]
public class Destroyable : MonoBehaviour
{
    [SerializeField, ProgressBar(0, "_hpMax")]
    private float _hp = 100;
    public float HP
    {
        get => _hp;
        set
        {
            if (_hp != value)
            {
                _hp = Mathf.Clamp(value, 0, 100);
                if (healthBar)
                    healthBar.Value = HP / HPMax;
                if (_hp == 0)
                {
                    IsDestroyed = true;
                    if (overrideDestroyMethod)
                        onDestroy?.Invoke();
                    else
                        Destroy(gameObject);
                }
            }
        }
    }
    [SerializeField, Min(0)]
    private float _hpMax = 100;
    public float HPMax { get => _hpMax; set => _hpMax = value; }

    [SerializeField, ReadOnly]
    private bool _isDestroyed;
    public bool IsDestroyed { get => _isDestroyed; private set => _isDestroyed = value; }

    [SerializeField]
    private ProgressBar healthBar;

    [SerializeField]
    private bool overrideDestroyMethod;

    [Tooltip("Override Destroy method"), ShowIf("overrideDestroyMethod")]
    public UnityEvent onDestroy;

    private void OnValidate()
    {
        if(healthBar)
            healthBar.Value = HP / HPMax;
    }
}
