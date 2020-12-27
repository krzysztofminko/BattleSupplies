/*
 * Based on unity WheelCollider tutorial:
 * https://docs.unity3d.com/Manual/WheelColliderTutorial.html
 */

using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine.Rendering;

namespace VehiclePhysics
{
    public class Vehicle : MonoBehaviour
    {
        [Serializable]
        private class AxleInfo
        {
            public WheelCollider leftWheel;
            public WheelCollider rightWheel;
            public bool steering;
            public bool motor = true;
            public bool brake = true;
        }

        [Header("Settings")]
        [SerializeField]
        private List<AxleInfo> axleInfos;
        [HideLabel, HorizontalGroup("CenterOfMass")]
        public bool customCenterOfMassEnabled;
        [EnableIf("customCenterOfMassEnabled"), HorizontalGroup("CenterOfMass")]
        public Vector3 customCenterOfMass;
        public float maxMotorTorque = 400;
        public float maxBrakeTorque = 400;
        public float handBrakeTorque = 400;
        public float autoBrakeTorque = 400;
        public float maxSteeringAngle = 30;
        [SuffixLabel("maxAngle/sec")]
        public float steeringSpeed = 1.5f;
        [Min(1)]
        public float maxSpeed = 90;
        public AnimationCurve speedToTorque;
        public AnimationCurve speedToSteering;
        public bool handBrake;

        [Header("Controls")]
        [PropertyRange(-1, 1)]
        public float motorInput;
        [PropertyRange(0, 1)]
        public float brakeInput;
        [PropertyRange(-1, 1)]
        public float steeringInput;
        [Min(0), Tooltip("In wheel rotations per minute.")]
        public float movementDetectionTolerance = 1;

        [ShowInInspector, ReadOnly, SuffixLabel("km/h")]
        public int Speed { get; private set; }
        [ShowInInspector, ReadOnly]
        public bool IsStopped { get; private set; }
        [ShowInInspector, ReadOnly]
        public bool IsMovingForward { get; private set; }
        [ShowInInspector, ReadOnly]
        public bool IsMovingBackward { get; private set; }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmo(Vehicle vehicle, GizmoType gizmoType)
        {
            Vector3 position = vehicle.transform.position + vehicle.transform.rotation * (vehicle.customCenterOfMassEnabled ? vehicle.customCenterOfMass : vehicle.GetComponent<Rigidbody>().centerOfMass);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, 0.25f);
        }

        private void Awake()
        {
            if (customCenterOfMassEnabled)
                GetComponent<Rigidbody>().centerOfMass = customCenterOfMass;
        }

        [ReadOnly, ShowInInspector]
        private float motor;
        [ReadOnly, ShowInInspector]
        private float brake;
        [ReadOnly, ShowInInspector]
        private float steering;

        private void Update()
        {
            motor = maxMotorTorque * motorInput * speedToTorque.Evaluate(Speed / maxSpeed);
            brake = maxBrakeTorque * brakeInput;

            if (motorInput == 0 && brakeInput == 0)
                brake = autoBrakeTorque;
            if (Speed / maxSpeed > 1)
            {
                motor = 0;
                brake = maxBrakeTorque;
            }
            if (handBrake)
                brake = handBrakeTorque;

            steering = maxSteeringAngle * steeringInput * speedToSteering.Evaluate(Speed / maxSpeed);
        }

        private void FixedUpdate()
        {
            float rpm = 0;
            foreach (AxleInfo axleInfo in axleInfos)
            {
                rpm += axleInfo.leftWheel.rpm;
                rpm += axleInfo.rightWheel.rpm;

                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                if (axleInfo.brake)
                {
                    axleInfo.leftWheel.brakeTorque = brake;
                    axleInfo.rightWheel.brakeTorque = brake;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }

            rpm /= axleInfos.Count * 2;
            IsMovingForward = rpm > movementDetectionTolerance;
            IsMovingBackward = !IsMovingForward && rpm < -movementDetectionTolerance;
            IsStopped = rpm > -movementDetectionTolerance && rpm < movementDetectionTolerance;

            Speed = (int)(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
        }

        private void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount > 0)
            {
                Transform visualWheel = collider.transform.GetChild(0);

                Vector3 position;
                Quaternion rotation;
                collider.GetWorldPose(out position, out rotation);

                visualWheel.transform.position = position;
                visualWheel.transform.rotation = rotation;
            }
        }
    }
}