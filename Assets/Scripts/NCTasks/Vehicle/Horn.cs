using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
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
		private float buttonHoldDuration;
		private bool locked;


		protected override void OnExecute()
		{
			vehicle = _vehicle.value;
		}

		//REFACTOR: Try to exctract pressed/held logic to some class?
		//REFACTOR: Delete unnecessary comments
		protected override void OnUpdate()
		{
			if (vehicle)
			{				
				if (Input.GetButtonDown(buttonName.value))
				{
					buttonPressedTime = Time.realtimeSinceStartup;
					Debug.Log("Down");
				}
				else if (Input.GetButtonUp(buttonName.value))
				{
					Debug.Log("Up");
					if (Time.realtimeSinceStartup - buttonPressedTime < 1 && !locked)
					{
						Debug.Log("Get in!");
						Collider collider = Physics.OverlapSphere(agent.transform.position, radius.value, layerMask, QueryTriggerInteraction.Collide).FirstOrDefault(c => c.GetComponent<Soldier>().Team == agent.Team && !c.GetComponent<Soldier>().targetVehicle);
						if (collider)
						{
							Soldier soldier = collider.GetComponent<Soldier>();
							soldier.targetVehicle = vehicle;
							Debug.Log(soldier, soldier);
						}
					}
					locked = false;
				}
				else if(Input.GetButton(buttonName.value))
				{
					if (Time.realtimeSinceStartup - buttonPressedTime > 1)
					{
						buttonPressedTime = Time.realtimeSinceStartup;
						locked = true;
						Debug.Log("Hold > 1");
						Debug.Log("Get out!");
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