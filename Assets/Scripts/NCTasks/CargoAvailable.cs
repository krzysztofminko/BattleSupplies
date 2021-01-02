using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	[Category("Cargo")]
	public class CargoAvailable : ConditionTask
	{
		public BBParameter<Cargo> cargo;

		protected override bool OnCheck() => cargo.value && cargo.value.Available;
	}
}