using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Threading;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("Input")]
    public class CheckButtonHold : ConditionTask
    {

        [RequiredField]
        public BBParameter<string> buttonName = "Fire1";
        public BBParameter<float> duration = 2;

        private float timer;
        private float lastUpdate;
        private bool pressed;

        protected override string info => $"Hold {buttonName.ToString()} for {duration}s";

        protected override bool OnCheck()
        {

            if (Input.GetButtonDown(buttonName.value))
            {
                timer = 0;
                pressed = true;
                lastUpdate = Time.time;
            }

            if (pressed)
            {
                timer += Time.time - lastUpdate;
                lastUpdate = Time.time;

                if (!Input.GetButton(buttonName.value))
                {
                    timer = 0;
                    pressed = false;
                }
                else if (timer > duration.value)
                {
                    timer = 0;
                    pressed = false;
                    return true;
                }
            }

            return false;
        }

        protected override void OnEnable()
        {
            timer = 0; 
            pressed = false;
        }
    }
}