using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class LoadContainer : ActionTask
	{
		public BBParameter<Container> container;
		public BBParameter<Transform> objectToLoad;
		public bool containerCanBeNull;

		protected override void OnExecute()
		{
			if (!containerCanBeNull && !container.value)
				Debug.LogError("container is null", ownerSystemAgent);
			if (!objectToLoad.value)
				Debug.LogError("objectToLoad is null", ownerSystemAgent);

			if (container.value)
			{
				EndAction(container.value.Load(objectToLoad.value));
			}
			else
			{
				EndAction(false);
			}
		}
	}
}