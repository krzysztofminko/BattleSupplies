﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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
            if (_hp != Mathf.Clamp(value, 0, 100))
            {
                _hp = Mathf.Clamp(value, 0, 100);
                if (HealthBar)
                    HealthBar.Value = HP / HPMax;
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
    private ProgressBar _healthBar;
    public ProgressBar HealthBar => _healthBar;

    [SerializeField]
    private bool overrideDestroyMethod;

    [Tooltip("Override Destroy method"), ShowIf("overrideDestroyMethod")]
    public UnityEvent onDestroy;

    private void OnValidate()
    {
        if(_healthBar)
            _healthBar.Value = HP / HPMax;
    }
}
