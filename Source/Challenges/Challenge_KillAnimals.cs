using System.Linq;
namespace Verse
{
	public class Challenge_KillAnimals : ChallengeWorker
	{
		public Challenge_KillAnimals(ChallengeDef def) : base(def)
		{
		}

		public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			if (pawn.AnimalOrWildMan() && ChallengeWorker.AllColonists.Cast<Thing>().Contains(dinfo.Instigator)) {
				progress++;
				if (progress >= def.targetValue) {
					Complete();
				}
			}
		}

		public static int animalCount { get { return Find.AnyPlayerHomeMap.mapPawns.AllPawns.Count((Pawn x) => x.AnimalOrWildMan()); } }
	}
}
