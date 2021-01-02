using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Picker")]
	public class Pick : ActionTask<Picker>
	{
		public BBParameter<Parentable> target;
		public BBParameter<Storage> storage;

		protected override void OnExecute()
		{
			if (target.value == null && storage.value == null)
			{
				Debug.LogError("Target and Storage are null. One of them is required.", agent);
			}
			else
			{
				agent.picked = target.value ? target.value : storage.value.Remove();
				agent.picked.Parent = agent.anchor;

				EndAction(true);
			}
		}
	}
}