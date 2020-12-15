using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class Shoot : ActionTask<Soldier>
	{
		public BBParameter<Destroyable> target;
		public BBParameter<float> minDelay = 1;
		public BBParameter<float> maxDelay = 2;
		private float timer;
		private float delay;

		protected override void OnExecute()
		{
			delay = Random.Range(minDelay.value, maxDelay.value);
		}

		protected override void OnUpdate()
		{
			if (target.value)
			{
			//	Debug.DrawLine(agent.transform.position + Vector3.up, target.value.transform.position + Vector3.up, agent.Team == 0 ? Color.blue : Color.red, Time.deltaTime);

				timer += Time.deltaTime;

				Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.value.transform.position - agent.transform.position, Vector3.up));
				if (Quaternion.Angle(agent.transform.rotation, targetRotation) > 1)
				{
					agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, 600 * Time.deltaTime);
				}
				else if (agent.AmmoCount > 0)
				{
					if (timer > delay)
					{
						timer = 0;
						Object.Instantiate(agent.amunition, agent.transform.position + Vector3.up, agent.transform.rotation).target = target.value;
						agent.AmmoCount--;
						EndAction(true);
					}
				}
				else
				{
					//No ammo
					EndAction(false);
				}
			}
			else
			{
				EndAction(true);
			}
		}
	}
}