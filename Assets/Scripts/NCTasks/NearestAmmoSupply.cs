using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	[Category("Soldier")]
	public class NearestAmmoSupply : ConditionTask<Soldier>
	{
		public BBParameter<float> radius = 10;
		public LayerMask layerMask;
		public BBParameter<AmmoSupply> returnedAmmoSupply;

		protected override string info { get => "Ammo supply distance < " + radius; }

		protected override bool OnCheck()
		{
			Collider target = Physics.OverlapSphere(agent.transform.position, radius.value, layerMask, QueryTriggerInteraction.Collide)
				.OrderBy(c => Distance.Manhattan2D(agent.transform.position, c.transform.position))
				.FirstOrDefault();
			returnedAmmoSupply.value = target == null ? null : target.GetComponent<AmmoSupply>();

			return returnedAmmoSupply.value;
		}
	}
}