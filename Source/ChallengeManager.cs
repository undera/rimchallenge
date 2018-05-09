using System;
using System.Collections.Generic;
using Verse;

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
			Log.Message("Expose data called");

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
