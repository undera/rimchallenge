﻿using System;
using Verse;
using System.Linq;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Rimchallenge
{
	public class ChallengeManager : GameComponent
	{
		public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone();

		public ChallengeDef currentChallengeDef;
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
			Scribe_References.Look<Pawn>(ref this.questOwner, "questOwner");
			Scribe_Defs.Look<ChallengeDef>(ref this.questOwnerChallenge, "questOwnerChallenge");
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
				Log.Message("Current progress: " + currentChallenge.getProgressFloat());
			}
		}

		public void ClearChallenge()
		{
			currentChallenge = new ChallengeWorkerNone();
			currentChallengeDef = null;
			progress = 0;
		}

		public static Letter MakeLetter(ChallengeDef challenge, Pawn author=null)
        {
            string text = "Hey survivor!\n\nI'm interested in changing this world to better, and would appreciate your help by sharing some goods. Here's the task:\n";
            text += "\n" + challenge.description;
            text += "\n\n" + challenge.RewardsText();
            text += "\n\nAre you capable of doing this?";

            ChallengeAvailableLetter letter = new ChallengeAvailableLetter();
            letter.label = "Challenge Available";
            letter.title = "Message Over Radio from a Stranger: " + challenge.LabelCap;
            letter.text = text;
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
