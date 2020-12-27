using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class TargetFound : ConditionTask<Soldier>
	{
		public float radius = 10;
		public LayerMask layerMask;
		public BBParameter<Destroyable> target;

		protected override string info { get => "Target found < " + radius; }

		protected override bool OnCheck()
		{
			Collider[] colliders = Physics.OverlapSphere(agent.transform.position, radius, layerMask, QueryTriggerInteraction.Collide)
				.Where(c => c.GetComponent<ITeam>().Team != agent.Team)
				.OrderBy(c => Distance.Manhattan2D(c.transform.position, agent.transform.position))
				.ToArray();

			if (colliders.Length > 0)
				target.value = colliders[Random.Range(0, colliders.Length / 4)].GetComponent<Destroyable>();

			return target.value;
		}

		public override void OnDrawGizmosSelected()
		{
			if (agent != null)
				Gizmos.DrawWireSphere(agent.transform.position, radius); 
		}
	}
}