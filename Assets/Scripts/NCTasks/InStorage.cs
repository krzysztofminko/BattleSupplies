using NodeCanvas.Framework;
using ParadoxNotion.Design;
using VehiclePhysics;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class InStorage : ActionTask<Soldier>
	{
		public BBParameter<Storage> _storage;
		private Storage storage;

		protected override void OnExecute()
		{
			storage = _storage.value;
			storage.Add(agent.GetComponent<Parentable>());
		}

		protected override void OnStop()
		{
			if (storage)
				storage.Remove(agent.GetComponent<Parentable>());
		}
	}
}