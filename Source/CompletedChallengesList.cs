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
			Log.Message("Added to list, len="+challenges.Count );
		}

        public override void ExposeData()
		{
			Log.Message("Expose data called for CompletedChallengesList. Len before: "+this.challenges.Count);
			Scribe_Collections.Look<ChallengeDef>(ref this.challenges, "doneChallenges", LookMode.Def, new object[0]);
			Log.Message("Len after: " + this.challenges.Count);
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
