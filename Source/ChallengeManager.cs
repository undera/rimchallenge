using System;
using Verse;
using System.Linq;
using System.Collections.Generic;
using Challenges;
using UnityEngine;

namespace Rimchallenge
{
	public class ChallengeManager : GameComponent
	{
        public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone(null);

		public ChallengeDef currentChallengeDef;
		public int progress = 0;
		public static ChallengeManager instance { get; private set; }

		public ChallengeManager(Game game)
		{
			//Log.Message("Constructed ChallengeManager");
			instance = this;
			foreach (ChallengeDef def in DefDatabase<ChallengeDef>.AllDefs)
			{
				Controller.checkFinished(def);
			}

			//DefDatabase<MainButtonDef>.GetNamed("ChallengesTab").buttonVisible=Prefs.DevMode;
		}

		internal ChallengeDef GetOfferedChallenge()
		{
			IEnumerable<ChallengeDef> possible = DefDatabase<ChallengeDef>.AllDefs.Where(x => x.CanStartNow && x != currentChallengeDef).ToList();
			return possible.RandomElementWithFallback(null);
		}
        
		public override void StartedNewGame()
		{
			Log.Message("Started new game, challenge is: " + currentChallengeDef + " with progress " + progress);
			currentChallenge.Started();
		}

		public override void LoadedGame()
		{
			Log.Message("Loaded game, challenge is: " + currentChallengeDef + " with progress " + progress);
			StartChallenge(currentChallengeDef);
			currentChallenge.Started();
			if (currentChallenge != null)
			{
				currentChallenge.progress = progress;
			}
		}

		public override void ExposeData()
		{
			//Log.Message("Expose data called for ChallengeManager");
			Scribe_Defs.Look<ChallengeDef>(ref this.currentChallengeDef, "currentChallenge");
			Scribe_Values.Look<int>(ref this.progress, "progress");
		}

		public bool HasChallenge()
		{
			return currentChallenge != null && !(currentChallenge is ChallengeWorkerNone);
		}

		public void StartChallenge(ChallengeDef challengeDef)
		{
			if (HasChallenge())
			{
				currentChallenge.Interrupt();
			}

			if (challengeDef != null)
			{
				Log.Message("Picked a challenge: " + challengeDef);
				currentChallengeDef = challengeDef;
				currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef);
			}
		}

		public void ClearChallenge()
		{
			Log.Message("Clearing challenge");
			currentChallenge = new ChallengeWorkerNone(null);
			currentChallengeDef = null;
			progress = 0;
		}
	}
}
