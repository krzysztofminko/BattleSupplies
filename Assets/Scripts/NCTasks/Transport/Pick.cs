using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class Pick : ActionTask<Carrier>
	{
		public BBParameter<Transform> target;

		protected override void OnExecute()
		{
			if (target.value == null)
			{
				Debug.LogError("Target is null.", agent);
			}
			else
			{
				agent.carriedObject = target.value;
				target.value.transform.parent = agent.anchor;
				target.value.transform.localPosition = Vector3.zero;
				target.value.transform.localRotation = Quaternion.identity;
				target.value.GetComponents<IPickable>().ForEach(p => p.OnPick());

				agent.GetComponent<Animator>().SetBool("Carry", true);

				EndAction(true);
			}
		}
	}
}