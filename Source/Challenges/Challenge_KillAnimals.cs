using System.Linq;
using Verse;

namespace Challenges
{
	public class Challenge_KillAnimals : ChallengeWorker
	{
		public Challenge_KillAnimals(ChallengeDef def, Pawn giver) : base(def, giver)
		{
		}

		public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			if (pawn.AnimalOrWildMan() && Challenge_NColonists.AllColonists.Cast<Thing>().Contains(dinfo.Instigator)) {
				progress++;
				if (progress >= def.targetValue) {
					Complete();
				}
			}
		}

		public static int animalCount { get { return Find.AnyPlayerHomeMap.mapPawns.AllPawns.Count((Pawn x) => x.AnimalOrWildMan()); } }
	}
}
