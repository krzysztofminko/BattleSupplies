using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;
using VehiclePhysics;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Vehicle")]
	public class Horn : ActionTask<Vehicle>
	{
		public float radius = 20;
		public LayerMask layerMask;

		protected override void OnExecute()
		{
			Collider collider = Physics.OverlapSphere(agent.transform.position, radius, layerMask, QueryTriggerInteraction.Collide).FirstOrDefault();

			if (collider)
				collider.GetComponent<Soldier>().targetVehicle = agent;

			EndAction(collider);
		}
	}
}