﻿using System.Linq;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;
using System;
using UnityEngine;

namespace Challenges
{
	public abstract class ChallengeWorker
	{
		private static readonly IntRange RaidDelay = new IntRange(2000, 5000);

		public ChallengeDef def { get; set; } = new ChallengeDef();
		public string hint { get; protected set; } = null;

		private int _progress = 0;

		public int progress
		{
			get { return _progress; }
			set
			{
				_progress = value;
				ChallengeManager.instance.progress = _progress;
			}
		}

		public ChallengeWorker(ChallengeDef def)
		{
			this.def = def;
		}

		public virtual float getProgressFloat()
		{
			//Log.Message("Def "+def);
			//Log.Message("Det "+progress + " " + def.targetValue);
			if (def.targetValue != 0)
			{
				return (float)progress / def.targetValue;
			}
			else { return 0f; }
		}

		public void Complete()
		{
			def.IsFinished = true;

			ChallengeManager.instance.ClearChallenge();
			Controller.ChallengeComplete(def);
			IEnumerable<Thing> reward = def.GetReward();

			string rewardText = "";
			foreach (Thing thing in reward)
			{
				rewardText += "  - " + GenLabel.ThingLabel(thing.def, thing.Stuff, thing.stackCount).CapitalizeFirst() + "\n";
			}

			EndGameDialogMessage("ChallengeCompletedEndDialog".Translate(new[] { def.label, def.description, rewardText }));

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.AnyPlayerHomeMap); // drop around base
			DropPodUtility.DropThingsNear(dropSpot, Find.AnyPlayerHomeMap, reward);
		}

		private void EndGameDialogMessage(string msg)
		{
			bool allowKeepPlaying = true;
			Color screenFillColor = Color.clear;
			DiaNode diaNode = new DiaNode(msg);
			if (allowKeepPlaying)
			{
				DiaOption diaOption = new DiaOption("GameOverKeepPlaying".Translate());
				diaOption.resolveTree = true;
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("GameOverMainMenu".Translate());
			diaOption2.action = delegate
			{
				GenScene.GoToMainMenu();
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			Dialog_NodeTree dialog_NodeTree = new Dialog_GameEnd(diaNode, true, false, "Challenge Complete!");
			dialog_NodeTree.screenFillColor = screenFillColor;
			dialog_NodeTree.silenceAmbientSound = !allowKeepPlaying;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		public void Interrupt()
		{
			ChallengeManager.instance.ClearChallenge();
		}

		public virtual bool CanPick()
		{
			return true;
		}

		public virtual void Started()
		{
		}

		public virtual void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
		}

		public virtual void OnPawnDestroyed(Pawn pawn)
		{
		}

		public virtual void OnPawnFactionSet(Pawn pawn)
		{
		}

		public virtual void OnDestroyMined(Mineable block, Pawn actor)
		{
		}

		public virtual void OnSkillLearned(SkillRecord skill, Pawn pawn, int oldSkillLevel)
		{
		}

		public virtual void OnThingProduced(Thing result, Pawn worker)
		{
		}
	}

	public class ChallengeWorkerNone : ChallengeWorker
	{
		public ChallengeWorkerNone(ChallengeDef def) : base(def)
		{
		}
	}
}
