using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Player")]
	public class MovePlayer : ActionTask<Movement>
	{
		public BBParameter<bool> enabled = true;

		private bool playerIsRunning;

		protected override void OnUpdate()
		{
			if (enabled.value)
			{
				if (Input.GetButtonDown("Run"))
					playerIsRunning = !playerIsRunning;
				agent.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), playerIsRunning ? Movement.Type.Walk : Movement.Type.Run);
			}
			else
			{
				agent.Move(Vector3.zero);
			}
		}
	}
}