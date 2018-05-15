using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Rimchallenge
{
	public class ChallengeAvailableLetter : StandardLetter
	{
		internal Pawn giver;
		internal ChallengeDef challenge;
        public ChallengeAvailableLetter()
		{
			def = LetterDefOf.PositiveEvent;
		}

		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return Accept;
				yield return base.Reject;
			}
		}

		protected DiaOption Accept
        {
            get
            {
				return new DiaOption("Challenge Accepted!")
                {
                    action = delegate
                    {
						if (giver == ChallengeManager.instance.questOwner) {
							ChallengeManager.instance.SetQuestOwner(null);
						}
						ChallengeManager.instance.StartChallenge(challenge);
                        Find.LetterStack.RemoveLetter(this);
                    },
                    resolveTree = true
                };
            }
        }
	}
}
