using System;
using Verse;
using System.Linq;
using System.Collections.Generic;
using RimWorld;

namespace Rimchallenge
{
	public class ChallengeManager : GameComponent
	{
		public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone();

		public ChallengeDef currentChallengeDef;
		public int progress = 0;
		public static ChallengeManager instance { get; private set; }

		public ChallengeManager(Game game)
		{
			Log.Message("Constructed ChallengeManager");
			instance = this;
			foreach (ChallengeDef def in DefDatabase<ChallengeDef>.AllDefs) {
				Controller.checkFinished(def);
			}
		}

		internal ChallengeDef GetOfferedChallenge()
        {
			IEnumerable<ChallengeDef> possible = DefDatabase<ChallengeDef>.AllDefs.Where(x => x.CanStartNow);
			return possible.RandomElementWithFallback(null);
        }

        public override void StartedNewGame()
		{
			Log.Message("Started new Game");
			ClearChallenge();
			progress = 0;
		}

		public override void LoadedGame()
		{
			Log.Message("Loaded game: " + currentChallengeDef + " with " + progress);
			StartChallenge(currentChallengeDef);
			currentChallenge.progress = progress;
		}

		public override void ExposeData()
		{
			Log.Message("Expose data called for ChallengeManager");

			Scribe_Defs.Look<ChallengeDef>(ref this.currentChallengeDef, "currentChallenge");
			Scribe_Values.Look<int>(ref this.progress, "progress");
		}

		public bool HasChallenge()
		{
			return currentChallenge != null && !(currentChallenge is ChallengeWorkerNone);
		}

		public void StartChallenge(ChallengeDef challengeDef)
		{
			if (challengeDef != null)
			{
				Log.Message("Picked a challenge: " + challengeDef);
				currentChallengeDef = challengeDef;
				currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef);
				Log.Message("Current progress: "+currentChallenge.getProgressFloat());
			}
		}

		public void ClearChallenge()
		{
			currentChallenge = new ChallengeWorkerNone();
			currentChallengeDef = null;
			progress = 0;
		}
	}
}
