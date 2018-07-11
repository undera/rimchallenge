using System;
using System.Linq;
using Verse;

namespace Challenges
{
	public class Challenge_KillAnimals : ChallengeWorker
	{
		public Challenge_KillAnimals(ChallengeDef def) : base(def)
		{
		}

		public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			if (isRightType(pawn) && Challenge_NColonists.AllColonists.Cast<Thing>().Contains(dinfo.Instigator)) {
				progress++;
				if (progress >= def.targetValue) {
					Complete();
				}
			}
		}

		private bool isRightType(Pawn pawn)
		{
			switch (def.param1)
			{
				case 1: 
					return pawn.RaceProps.Humanlike;
				default: 
					return pawn.RaceProps.Animal;
			}
		}
	}
}
