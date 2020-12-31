using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private static List<ProgressBar> list = new List<ProgressBar>();

    [SerializeField, Required]
    private Image foreground;

    [SerializeField]
    private Image background;

    [SerializeField, ProgressBar(0.0f, 1.0f)]
    private float _value = 0.75f;
    public float Value
    {
        get => _value;
        set => foreground.fillAmount =_value = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    private void OnEnable()
    {
        foreground.enabled = true;
        if (background)
            background.enabled = true;
    }

    private void OnDisable()
    {
        foreground.enabled = false;
        if (background)
            background.enabled = false;
    }

    private void OnValidate() => Value = _value;

    private void Awake() => list.Add(this);
    private void OnDestroy() => list.Remove(this);

    public static void EnableAll() => list.ForEach(pb => pb.enabled = true);
    public static void DisableAll() => list.ForEach(pb => pb.enabled = false);
}
