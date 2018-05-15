using System;
using System.Collections.Generic;
using Verse;

namespace Rimchallenge
{
	public class CompletedChallengesList: ModSettings
    {
		private List<ChallengeDef> challenges = new List<ChallengeDef>();

		public void AddCompleted(ChallengeDef def) {
			challenges.Add(def);
		}

        public override void ExposeData()
		{
			Scribe_Collections.Look<ChallengeDef>(ref this.challenges, "doneChallenges", LookMode.Def, new object[0]);
		}

        internal bool isFinished(ChallengeDef def)
        {
			return challenges.Contains(def);
        }

		internal void Reset()
		{
			challenges.Clear();
		}
	}
}
