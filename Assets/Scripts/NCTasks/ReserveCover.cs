using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Diagnostics;

namespace NodeCanvas.Tasks.Actions
{
	[Category("Soldier")]
	public class ReserveCover : ActionTask<Soldier>
	{
		public BBParameter<Cover> cover;
		public BBParameter<bool> reservationState = true;

		protected override string info => (reservationState.value ? "Reserve " : "Free ") + cover.ToString();

		protected override void OnExecute()
		{
			if (reservationState.value)
			{
				if (!cover.value.reserved)
				{
					agent.onDie += FreeCover;
					cover.value.reserved = agent;
				}
				else if(cover.value.reserved != agent)
				{
					cover.value = null;
				}
			}
			else
			{
				FreeCover();
			}

			EndAction(true);
		}

		private void FreeCover()
		{
			agent.onDie -= FreeCover;
			if (cover.value)
				cover.value.reserved = null;
			cover.value = null;
		}
	}
}