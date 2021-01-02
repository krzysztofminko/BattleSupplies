using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Picker")]
	public class Put : ActionTask<Picker>
	{
		[Tooltip("Optional")]
		public BBParameter<Storage> storage;

		protected override void OnExecute()
		{
			if (!agent.picked)
			{
				Debug.LogError($"{agent} is not carrying anything.", agent);
			}
			else 
			{
				agent.picked.Parent = null;
				if (storage.value)
					storage.value.Add(agent.picked);
				agent.picked = null;

				EndAction(true);
			}
		}
	}
}