using System;
using Verse;
using System.Linq;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Challenges;

namespace Rimchallenge
{
	public class ChallengeManager : GameComponent
	{
		public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone();

		public ChallengeDef currentChallengeDef;
		public Pawn currentChallengeGiver;
		public int progress = 0;
		public static ChallengeManager instance { get; private set; }

		public Pawn questOwner;
		public ChallengeDef questOwnerChallenge;

		public ChallengeManager(Game game)
		{
			//Log.Message("Constructed ChallengeManager");
			instance = this;
			foreach (ChallengeDef def in DefDatabase<ChallengeDef>.AllDefs)
			{
				Controller.checkFinished(def);
			}
		}

		internal ChallengeDef GetOfferedChallenge()
		{
			IEnumerable<ChallengeDef> possible = DefDatabase<ChallengeDef>.AllDefs.Where(x => x.CanStartNow && x != currentChallengeDef).ToList();
			foreach (ChallengeDef def in possible)
			{
				Log.Message("Possible: " + def);
			}
			return possible.RandomElementWithFallback(null);
		}

		public override void StartedNewGame()
		{
			ClearChallenge();
			progress = 0;
		}

		public override void LoadedGame()
		{
			Log.Message("Loaded game: " + currentChallengeDef + " with " + progress);
			StartChallenge(currentChallengeDef, currentChallengeGiver);
			currentChallenge.progress = progress;
		}

		public override void ExposeData()
		{
			//Log.Message("Expose data called for ChallengeManager");
			Scribe_Defs.Look<ChallengeDef>(ref this.currentChallengeDef, "currentChallenge");
			Scribe_References.Look<Pawn>(ref this.currentChallengeGiver, "currentChallengeGiver");
			Scribe_Values.Look<int>(ref this.progress, "progress");
			Scribe_References.Look<Pawn>(ref this.questOwner, "questOwner");
			Scribe_Defs.Look<ChallengeDef>(ref this.questOwnerChallenge, "questOwnerChallenge");
		}

		public bool HasChallenge()
		{
			return currentChallenge != null && !(currentChallenge is ChallengeWorkerNone);
		}

		public void StartChallenge(ChallengeDef challengeDef, Pawn giver)
		{
			if (HasChallenge())
			{
				currentChallenge.Interrupt();
			}

			if (challengeDef != null)
			{
				Log.Message("Picked a challenge: " + challengeDef);
				currentChallengeDef = challengeDef;
				currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef, giver);
				Log.Message("Current progress: " + currentChallenge.getProgressFloat());
			}
		}

		public void ClearChallenge()
		{
			currentChallenge = new ChallengeWorkerNone();
			currentChallengeDef = null;
			currentChallengeGiver = null;
			progress = 0;
		}

		public static Letter MakeLetter(ChallengeDef challenge, Pawn author)
		{
			ChallengeAvailableLetter letter = new ChallengeAvailableLetter();
			letter.label = "ChallengeAvailable".Translate();
			letter.title = "ChallengeAvailable".Translate() + ": " + challenge.LabelCap;
			letter.text = "ChallengeDescr".Translate(new[] { author.LabelShort, author.Faction.Name, challenge.label, challenge.description, challenge.RewardsText() });
			letter.radioMode = true;
			letter.challenge = challenge;
			letter.giver = author;
			letter.StartTimeout(60000);

			return letter;
		}

		internal void SetQuestOwner(Pawn pawn)
		{
			questOwner = pawn;
			if (pawn == null)
			{
				questOwnerChallenge = null;
			}
			else
			{
				ChallengeDef offer = GetOfferedChallenge();
				questOwnerChallenge = offer;
				if (offer == null)
				{
					questOwner = null;
				}
			}
		}
	}
}
