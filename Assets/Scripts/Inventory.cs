using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Inventory : MonoBehaviour
{
    [SerializeField, Required]
    private Transform weaponAnchor;

    [SerializeField, OnValueChanged("UpdateWeapon")]
    private Weapon _weapon;
    public Weapon Weapon
    {
        get => _weapon;
        set 
        {
            if (_weapon != value)
            {
                _weapon = value;
                UpdateWeapon();
            }
        }
    }

    [SerializeField, ReadOnly]
    private Transform instantiatedWeapon;

    private Animator animator;

    private bool playMode;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playMode = true;
        UpdateWeapon();
    }

    private void OnValidate()
    {
        animator = GetComponent<Animator>();
    }

    private void UpdateWeapon()
    {
        if (instantiatedWeapon)
        {
            if (playMode)
                Destroy(instantiatedWeapon.gameObject);
            else
                DestroyImmediate(instantiatedWeapon.gameObject);
        }
        if (Weapon)
        {
            instantiatedWeapon = Instantiate(Weapon.Model, weaponAnchor);
            instantiatedWeapon.localPosition = Vector3.zero;
            instantiatedWeapon.localRotation = Quaternion.identity;
            animator.SetFloat("WeaponID", Weapon.ID);
        }
        else
        {
            animator.SetFloat("WeaponID", 0);
        }

    }
}
