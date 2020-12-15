using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	[Category("Selection")]
	public class IsNotNullAndHasComponent<T> : ConditionTask
	{
		protected override string info => $"{checkedObject} has {typeof(T).Name} component.";

		public BBParameter<Transform> checkedObject;

		protected override bool OnCheck() => checkedObject.value?.GetComponent(typeof(T));
	}
}