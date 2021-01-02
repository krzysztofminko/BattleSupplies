using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class Pick : ActionTask<Carrier>
	{
		public BBParameter<IPickable> _pickable;
		private IPickable pickable;

		protected override void OnExecute()
		{
			if (_pickable.value == null)
			{
				Debug.LogError("Target is null.", agent);
			}
			else
			{
				pickable = _pickable.value;
				agent.cargo = pickable.Pick(agent.anchor);
				EndAction(true);
			}
		}
	}
}