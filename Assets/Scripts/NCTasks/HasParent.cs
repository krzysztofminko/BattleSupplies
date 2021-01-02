using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	[Category("Parentable")]
	public class HasParent : ConditionTask
	{
		public BBParameter<Parentable> parentable;

		protected override bool OnCheck() => parentable.value && parentable.value.Parent;
	}
}