using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	[Category("Soldier")]
	public class NearestCargo : ConditionTask<Soldier>
	{
		public BBParameter<float> radius = 10;
		public LayerMask layerMask;
		public BBParameter<Cargo> returnedCargo;

		protected override string info { get => "Nearest cargo < " + radius; }

		protected override bool OnCheck()
		{
			Collider target = Physics.OverlapSphere(agent.transform.position, radius.value, layerMask, QueryTriggerInteraction.Collide)
				.Where(c => c.GetComponent<Cargo>().Available)
				.OrderBy(c => Distance.Manhattan2D(agent.transform.position, c.transform.position))
				.FirstOrDefault();
			returnedCargo.value = target == null ? null : target.GetComponent<Cargo>();

			return returnedCargo.value;
		}
	}
}