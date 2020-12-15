/*
 * https://forum.unity.com/threads/how-to-make-a-physically-real-stable-car-with-wheelcolliders.50643/
 * Add two anti-roll scripts to your vehicle, one per axle (front - rear). 
 * Set the left-right wheels on each one and adjust the AntiRoll value. 
 * Remember to reset center of mass to the real position, or don't touch it at all!
 */

using UnityEngine;

namespace VehiclePhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class AntiRollBar : MonoBehaviour
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public float antiRoll = 5000;

        private void FixedUpdate()
        {
            WheelHit hit;
            float travelL = 1.0f;
            float travelR = 1.0f;

            bool groundedL = leftWheel.GetGroundHit(out hit);
            bool groundedR = rightWheel.GetGroundHit(out hit);

            if (groundedL)
                travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
            if (groundedR)
                travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

            float antiRollForce = (travelL - travelR) * antiRoll;

            if (groundedL)
                GetComponent<Rigidbody>().AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
            if (groundedR)
                GetComponent<Rigidbody>().AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
        }

    }
}