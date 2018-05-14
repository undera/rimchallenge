using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Pawn actor = giveChallenge.actor;

				Log.Message("Actor: "+actor);
				Log.Message("Target: " + TargetPawn);
				Log.Message("Quest owner: " + ChallengeManager.instance.questOwner);

                //if (this.TargetPawn.CanTradeNow)
                //{
                //    Find.WindowStack.Add(new Dialog_Trade(actor, this.TargetPawn));
                //}
            };
            yield return giveChallenge;
        }

		private bool hasChallengeToOffer(Pawn targetPawn)
		{
			return ChallengeManager.instance.currentChallengeDef != null && ChallengeManager.instance.questOwner==targetPawn;
		}
	}
}
