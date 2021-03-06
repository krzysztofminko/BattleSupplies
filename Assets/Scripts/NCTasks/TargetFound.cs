using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class TargetFound : ConditionTask<Soldier>
	{
		public BBParameter<float> radius = 10;
		public LayerMask layerMask;
		public BBParameter<Destroyable> target;

		protected override string info { get => "Target found < " + radius; }

		protected override bool OnCheck()
		{
			//TODO: Squad object should call OverlapSphere once, instead of calling by every Soldier
			IEnumerable<Collider> colliders = Physics.OverlapSphere(agent.transform.position, radius.value, layerMask, QueryTriggerInteraction.Collide)
				.Where(c => c.GetComponent<ITeam>().Team != agent.Team && !c.GetComponent<Destroyable>().IsDestroyed)
				.OrderBy(c => (c.transform.position - agent.transform.position).sqrMagnitude);
				//.OrderBy(c => Distance.Manhattan2D(c.transform.position, agent.transform.position));

			if (colliders.Count() > 0)
				target.value = colliders.ElementAt(0).GetComponent<Destroyable>();
			//target.value = colliders.ElementAt(Random.Range(0, colliders.Count() / 4)).GetComponent<Destroyable>();

			return target.value;
		}

		public override void OnDrawGizmosSelected()
		{
			if (agent != null)
				Gizmos.DrawWireSphere(agent.transform.position, radius.value); 
		}
	}
}