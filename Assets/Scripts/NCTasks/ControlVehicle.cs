using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	//TODO: Create separate action EnterVehicle
	[Category("Player")]
	public class ControlVehicle : ActionTask<Transform>
	{
		public BBParameter<ExtendedVehicle> _vehicle;
		private ExtendedVehicle vehicle;

		public BBParameter<float> steeringSpeed = 1;

		private ExtendedVehicle.Seat seat;

		//TODO: Create Driver class and move characterController there
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
					agent.parent = seat.transform;
					agent.localPosition = Vector3.zero;
					agent.localRotation = Quaternion.identity;
					characterController.enabled = false;
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
			vehicle.brakeInput = Input.GetButton("Brakes") ? 1 : 0;

			if (Input.GetAxis("Horizontal") > vehicle.steeringInput)
				vehicle.steeringInput = Mathf.Min(Input.GetAxis("Horizontal"), vehicle.steeringInput + Time.deltaTime * steeringSpeed.value);
			else if (Input.GetAxis("Horizontal") < vehicle.steeringInput)
				vehicle.steeringInput = Mathf.Max(Input.GetAxis("Horizontal"), vehicle.steeringInput - Time.deltaTime * steeringSpeed.value);

			if (vehicle.IsMovingForward)
			{
				vehicle.motorInput = Mathf.Max(0, Input.GetAxis("Vertical"));
				if (Input.GetAxis("Vertical") < 0)
					vehicle.brakeInput = -Input.GetAxis("Vertical") * 0.5f;
			}
			else if (vehicle.IsMovingBackward)
			{
				vehicle.motorInput = Mathf.Min(0, Input.GetAxis("Vertical"));
				if (Input.GetAxis("Vertical") > 0)
					vehicle.brakeInput = Input.GetAxis("Vertical") * 0.5f;
			}
		}

		protected override void OnStop()
		{
			agent.parent = null;
			if (seat != null)
			{
				agent.position = seat.exit.position;
				agent.rotation = seat.exit.rotation;
			}
			characterController.enabled = true;
		}

	}
}