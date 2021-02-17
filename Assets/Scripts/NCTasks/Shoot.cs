using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class Shoot : ActionTask<Soldier>
	{
		public BBParameter<Destroyable> target;
		public BBParameter<float> range = 10;
		public BBParameter<float> minDelay = 1;
		public BBParameter<float> maxDelay = 2;
		public BBParameter<float> animationDelay;
		public BBParameter<float> animationDuration = 1.5f;
		private float timer;
		private float delay;
		private bool shooted;
		private bool instantiated;

		[GetFromAgent]
		private Animator animator;

		protected override void OnExecute()
		{
			timer = 0;
			delay = Random.Range(minDelay.value, maxDelay.value);
			delay = minDelay.value;
			shooted= false;
			instantiated = false;
		}

		protected override void OnUpdate()
		{
			timer += Time.deltaTime;

			if (target.value && target.value.IsDestroyed)
				target.value = null;

			if (target.value && (target.value.transform.position - agent.transform.position).sqrMagnitude > (range.value + 1) * (range.value + 1))	// +1 (horizontal size of Soldier collider) to fix OverlapingSphere hitting only part of collider
				target.value = null;

			if (target.value)
			{
			//	Debug.DrawLine(agent.transform.position + Vector3.up, target.value.transform.position + Vector3.up, agent.Team == 0 ? Color.blue : Color.red, Time.deltaTime);

				Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.value.transform.position - agent.transform.position, Vector3.up));
				if (Quaternion.Angle(agent.transform.rotation, targetRotation) > 1)
				{
					agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, 600 * Time.deltaTime);
				}
				else if (agent.AmmoCount > 0)
				{
					if (timer > delay)
					{
						if (!shooted)
						{
							animator.SetTrigger("Shoot");
							shooted = true;
						}

						if (timer > delay + animationDelay.value && !instantiated)
						{
							Object.Instantiate(agent.amunition, agent.transform.position + Vector3.up * 2, agent.transform.rotation).target = target.value;
							agent.AmmoCount--;
							instantiated = true;
						}

						if(timer > delay + animationDuration.value)
						{
							EndAction(true);
						}
					}
				}
				else
				{
					//No ammo
					EndAction(false);
				}
			}
			else if(!shooted || timer > delay + animationDuration.value)
			{
				EndAction(false);
			}
		}
	}
}