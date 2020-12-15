using Sirenix.OdinInspector;
using UnityEngine;
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
                    healthBar.fillAmount = HP / HPMax;
                if (_hp == 0)
                    Destroy(gameObject);
            }
        }
    }
    [SerializeField, Min(0)]
    private float _hpMax = 100;
    public float HPMax { get => _hpMax; set => _hpMax = value; }

    [SerializeField]
    private Image healthBar;

    private void OnValidate()
    {
        if(healthBar)
            healthBar.fillAmount = HP / HPMax;
    }
}
