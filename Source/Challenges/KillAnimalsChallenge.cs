using System.Linq;
namespace Verse
{
	public class KillAnimalsChallenge : ChallengeWorker
	{
		public KillAnimalsChallenge(ChallengeDef def) : base(def)
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
	}
}
