using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class FindCover : ActionTask<Soldier>
	{
		public BBParameter<Transform> fromTarget;
		public BBParameter<float> squareRange = 20;
		[Range(0.0f, 1.0f)]
		public float rangeForwardOffset = 0.5f;
		public BBParameter<LayerMask> layerMask;
		public BBParameter<Cover> result;

		protected override void OnExecute()
		{
			result.value = Physics.OverlapBox(agent.transform.position + (agent.targetPosition - agent.transform.position).normalized * squareRange.value * rangeForwardOffset, Vector3.one * squareRange.value * 0.5f, Quaternion.LookRotation(agent.targetPosition - agent.transform.position), layerMask.value, QueryTriggerInteraction.Collide)
				.OrderBy(c => Distance.Manhattan2D(agent.transform.position, c.transform.position))
				.FirstOrDefault(c => !c.GetComponent<Cover>().reserved 
									&& Distance.Manhattan2D(fromTarget.value.position, c.transform.position) > Distance.Manhattan2D(agent.transform.position, c.transform.position)
									&& Vector3.Angle(fromTarget.value.position - agent.transform.position, c.transform.forward) < 30 
								)
				?.GetComponent<Cover>();
			
			EndAction(result.value);
		}
	}
}