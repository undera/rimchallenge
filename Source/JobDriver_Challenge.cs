using System;
using System.Collections.Generic;
using Rimchallenge;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Challenge : JobDriver
    {
		private Pawn TargetPawn
        {
            get
            {
                return (Pawn)base.TargetThingA;
            }
        }

        public override bool TryMakePreToilReservations()
        {
            return this.pawn.Reserve(this.TargetPawn, this.job, 1, -1, null);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !hasChallengeToOffer(this.TargetPawn));
			Toil giveChallenge = new Toil();
            giveChallenge.initAction = delegate
            {
				Letter letter = ChallengeManager.MakeLetter(ChallengeManager.instance.questOwnerChallenge, this.TargetPawn);
				letter.OpenLetter();
            };
            yield return giveChallenge;
        }

		private bool hasChallengeToOffer(Pawn targetPawn)
		{
			return ChallengeManager.instance.currentChallengeDef != null && ChallengeManager.instance.questOwner==targetPawn;
		}
	}
}
