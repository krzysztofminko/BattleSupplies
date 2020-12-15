using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class MoveSoldier : ActionTask<Soldier>
	{
		public BBParameter<Vector3> targetPosition;
		public BBParameter<float> targetDistance = 1;

		protected override void OnUpdate()
		{
			Soldier.MoveStatus status = agent.Move(targetPosition.value, targetDistance.value);

			if (status != Soldier.MoveStatus.Running)
				EndAction(status == Soldier.MoveStatus.Success);
		}

		protected override void OnStop(bool interrupted)
		{
			if (interrupted)
				agent.Stop(true);
		}
	}
}