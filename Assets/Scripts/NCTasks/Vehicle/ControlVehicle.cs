using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Player")]
	public class ControlVehicle : ActionTask<Player>
	{
		public BBParameter<ExtendedVehicle> _vehicle;
		private ExtendedVehicle vehicle;

		private ExtendedVehicle.Seat seat;

		[GetFromAgent]
		private CharacterController characterController;

		protected override void OnExecute()
		{
			if (!_vehicle.value)
			{
				Debug.LogError("Vehicle is null.", agent);
			}
			else
			{
				vehicle = _vehicle.value;
				seat = vehicle.seats.FirstOrDefault(s => !s.user);
				if (seat != null)
				{
					agent.transform.parent = seat.transform;
					agent.transform.localPosition = Vector3.zero;
					agent.transform.localRotation = Quaternion.identity;
					characterController.enabled = false; 
					vehicle.handBrake = false;
					vehicle.Team = agent.Team;
				}
				else
				{
					EndAction(false);
				}
			}
		}

		protected override void OnUpdate()
		{
			vehicle.motorInput = Input.GetAxis("Vertical");
			vehicle.handBrake = Input.GetButton("Brakes");
			vehicle.brakeInput = 0;

			if (Input.GetAxis("Horizontal") > vehicle.steeringInput)
				vehicle.steeringInput = Mathf.Min(Input.GetAxis("Horizontal"), vehicle.steeringInput + Time.deltaTime * vehicle.steeringSpeed);
			else if (Input.GetAxis("Horizontal") < vehicle.steeringInput)
				vehicle.steeringInput = Mathf.Max(Input.GetAxis("Horizontal"), vehicle.steeringInput - Time.deltaTime * vehicle.steeringSpeed);

			if (vehicle.IsMovingForward)
			{
				vehicle.motorInput = Mathf.Max(0, Input.GetAxis("Vertical"));
				if (Input.GetAxis("Vertical") < 0)
					vehicle.brakeInput = -Input.GetAxis("Vertical");
			}
			else if (vehicle.IsMovingBackward)
			{
				vehicle.motorInput = Mathf.Min(0, Input.GetAxis("Vertical"));
				if (Input.GetAxis("Vertical") > 0)
					vehicle.brakeInput = Input.GetAxis("Vertical");
			}
		}

		protected override void OnStop()
		{
			agent.transform.parent = null;
			if (seat != null)
			{
				agent.transform.position = seat.exit.position;
				agent.transform.rotation = seat.exit.rotation;
			}
			vehicle.Team = -1;
			vehicle.handBrake = true;
			characterController.enabled = true;
		}

	}
}