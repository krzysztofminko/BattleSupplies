using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class Put : ActionTask<Carrier>
	{
		[Tooltip("Optional")]
		public BBParameter<Storage> _storage;
		private Storage storage;

		protected override void OnExecute()
		{
			storage = _storage.value;
			if (!agent.cargo)
			{
				Debug.LogError($"{agent} is not carrying anything.", agent);
			}
			else 
			{
				agent.cargo.SetParent(null);
				if (storage)
					storage.Put(agent.cargo);
				agent.cargo = null;
				EndAction(true);
			}
		}
	}
}