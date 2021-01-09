using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.Utilities;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Transport")]
	public class UnloadContainer : ActionTask
	{
		public BBParameter<Container> container;
		[Tooltip("Optional. If null, last object will be returned.")]
		public BBParameter<Transform> objectToUnload;
		public BBParameter<Transform> returnedObject;

		protected override void OnExecute()
		{
			if (!container.value)
				Debug.LogError("container is null", ownerSystemAgent);

			returnedObject.value = container.value.Unload(objectToUnload.value);
			if (returnedObject.value)
				returnedObject.value.GetComponents<ILoadable>().ForEach(l => l.OnUnload());

			EndAction(returnedObject.value);
		}
	}
}