using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class Resupply : ActionTask<Soldier>
	{
		public BBParameter<Cargo> cargo;
		public BBParameter<int> count = 1;
		public BBParameter<float> delay = 1;

		private float timer;
		private int realCount;

		protected override void OnExecute()
		{
			timer = 0;

			realCount = Mathf.Clamp(count.value, 0 ,cargo.value.AmmoCount);
			if (realCount > 0)
			{
				cargo.value.AmmoCount -= realCount;
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