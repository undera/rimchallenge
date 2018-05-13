using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Rimchallenge
{
	public class ChallengeAvailableLetter : StandardLetter
	{
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
						ChallengeManager.instance.StartChallenge(challenge);
                        Find.LetterStack.RemoveLetter(this);
                    },
                    resolveTree = true
                };
            }
        }
	}
}
