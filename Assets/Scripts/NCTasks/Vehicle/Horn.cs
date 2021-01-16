using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;
using VehiclePhysics;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Player")]
	public class Horn : ActionTask<Player>
	{
		public BBParameter<Vehicle> _vehicle;
		private Vehicle vehicle;
		public BBParameter<string> buttonName = "Horn";
		public BBParameter<float> radius = 10;
		public LayerMask layerMask;
		private float buttonPressedTime;
		private bool locked;


		protected override void OnExecute()
		{
			vehicle = _vehicle.value;
		}

		protected override void OnUpdate()
		{
			if (vehicle)
			{				
				if (Input.GetButtonDown(buttonName.value))
				{
					buttonPressedTime = Time.realtimeSinceStartup;
				}
				else if (Input.GetButtonUp(buttonName.value))
				{
					//Call Soldier to enter
					if (Time.realtimeSinceStartup - buttonPressedTime < 1 && !locked)
					{
						Collider collider = Physics.OverlapSphere(agent.transform.position, radius.value, layerMask, QueryTriggerInteraction.Collide).FirstOrDefault(c => c.GetComponent<Soldier>().Team == agent.Team && !c.GetComponent<Soldier>().targetVehicle);
						if (collider)
						{
							Soldier soldier = collider.GetComponent<Soldier>();
							soldier.targetVehicle = vehicle;
						}
					}
					locked = false;
				}
				else if(Input.GetButton(buttonName.value))
				{
					//Unload Soldier
					if (Time.realtimeSinceStartup - buttonPressedTime > 1)
					{
						buttonPressedTime = Time.realtimeSinceStartup;
						locked = true;
						Container container = vehicle.GetComponent<Container>();
						Transform objectInContainer = container.GetObject(o => o.GetComponent<Soldier>());
						if (objectInContainer)
							container.Unload(objectInContainer).GetComponent<Soldier>().targetVehicle = null;
					}
				}
			}
			else
			{
				EndAction(false);
			}
		}
	}
}