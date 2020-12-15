using System;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public bool invert;

    private void OnValidate() => SetDirection();

    private void LateUpdate() => SetDirection();

    private void SetDirection() => transform.forward = Camera.main.transform.forward * (invert ? -1 : 1);
}
