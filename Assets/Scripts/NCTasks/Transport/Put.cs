using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class Put : ActionTask<Carrier>
	{
		protected override void OnExecute()
		{
			if (!agent.carriedObject)
			{
				Debug.LogError($"{agent} is not carrying anything.", agent);
			}
			else
			{
				agent.carriedObject.transform.parent = null;
				agent.carriedObject.GetComponents<IPickable>().ForEach(p => p.OnPut());
				/*if (Physics.Raycast(agent.carriedObject.position, Vector3.down, out RaycastHit hitInfo, LayerMask.GetMask("Ground")))
				{
					agent.carriedObject.position = hitInfo.point;
				}*/
				agent.carriedObject = null;

				agent.GetComponent<Animator>().SetBool("Carry", false);

				EndAction(true);
			}
		}
	}
}