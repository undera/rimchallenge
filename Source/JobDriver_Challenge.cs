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
                //Pawn actor = giveChallenge.actor;
                //Find.WindowStack.Add(new Dialog_Challenge(actor, this.TargetPawn));

				Letter letter = ChallengeManager.MakeLetter(ChallengeManager.instance.questOwnerChallenge, this.TargetPawn);
				letter.OpenLetter();
                /*
				DiaNode diaNode = new DiaNode(this.text);
                diaNode.options.AddRange(this.Choices);
                WindowStack arg_39_0 = Find.WindowStack;
                DiaNode nodeRoot = diaNode;
                bool flag = this.radioMode;
                arg_39_0.Add(new Dialog_NodeTree(nodeRoot, false, flag, this.title));
                */
            };
            yield return giveChallenge;
        }

		private bool hasChallengeToOffer(Pawn targetPawn)
		{
			return ChallengeManager.instance.currentChallengeDef != null && ChallengeManager.instance.questOwner==targetPawn;
		}
	}
}
