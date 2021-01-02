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

		protected override void OnExecute()
		{
			if (!container.value)
				Debug.LogError("container is null", ownerSystemAgent);
			if (!objectToLoad.value)
				Debug.LogError("objectToLoad is null", ownerSystemAgent);

			bool result = container.value.Load(objectToLoad.value);
			objectToLoad.value.GetComponents<ILoadable>().ForEach(l => l.OnLoad());

			EndAction(result);
		}
	}
}