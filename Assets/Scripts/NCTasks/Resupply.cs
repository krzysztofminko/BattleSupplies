using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class Resupply : ActionTask<Soldier>
	{
		public BBParameter<AmmoSupply> ammoSupply;
		public BBParameter<int> count = 1;
		public BBParameter<float> delay = 1;

		private float timer;
		private int realCount;

		protected override void OnExecute()
		{
			timer = 0;

			realCount = Mathf.Clamp(count.value, 0, ammoSupply.value.Count);
			if (realCount > 0)
			{
				ammoSupply.value.Count -= realCount;
			}
			else
			{
				EndAction(false);
			}
		}

		protected override void OnUpdate()
		{
			timer += Time.deltaTime;
			if (timer > delay.value)
			{
				agent.AmmoCount += realCount;
				EndAction(true);
			}
		}
	}
}