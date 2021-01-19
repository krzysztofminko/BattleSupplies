using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class FindCover : ActionTask<Soldier>
	{
		public BBParameter<Transform> fromTarget;
		public BBParameter<float> minDistanceToTarget = 10;
		public BBParameter<float> squareRange = 20;
		public BBParameter<LayerMask> layerMask;
		public BBParameter<Cover> result;

		protected override void OnExecute()
		{
			result.value = Physics.OverlapBox(agent.transform.position + agent.transform.forward * squareRange.value * 0.5f, Vector3.one * squareRange.value * 0.5f, agent.transform.rotation, layerMask.value, QueryTriggerInteraction.Collide)
				.OrderBy(c => Distance.Manhattan2D(agent.transform.position, c.transform.position))
				.FirstOrDefault(c => Distance.Manhattan2D(fromTarget.value.position, c.transform.position) > minDistanceToTarget.value 
										&& Vector3.Angle(fromTarget.value.position - agent.transform.position, c.transform.forward) < 45 
										&& !c.GetComponent<Cover>().reserved
								)
				?.GetComponent<Cover>();
			
			EndAction(result.value);
		}
	}
}