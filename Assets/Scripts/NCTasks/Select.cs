using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System.Linq;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Selection")]
	public class Select : ActionTask<Transform>
	{

		[Tooltip("Related to agent's rotation. 'Z' should be same as radius.")]
		public BBParameter<Vector3> offset = new Vector3(0, 1, 1);
		public BBParameter<float> radius = 1;
		public BBParameter<LayerMask> layerMask;
		public QueryTriggerInteraction triggersInteraction;
		public BBParameter<Transform> returnedGameObject;

		protected override void OnUpdate()
		{
			returnedGameObject.value = Physics.OverlapSphere(agent.position + (agent.rotation * offset.value), radius.value, layerMask.value, triggersInteraction)
				.OrderBy(c => Distance.Manhattan2D(agent.position, c.transform.position))
				.FirstOrDefault()?.transform;
		}
	}
}