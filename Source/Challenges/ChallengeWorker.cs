using System.Linq;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;
using System;

namespace Verse
{
	public abstract class ChallengeWorker
	{
		private static readonly IntRange RaidDelay = new IntRange(2000, 5000);

		public ChallengeDef def { get; set; }
		public Pawn giverPawn;
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

		public ChallengeWorker(ChallengeDef def, Pawn giver)
		{
			this.def = def;
			this.giverPawn = giver;
		}

		public virtual float getProgressFloat()
		{
			//Log.Message("Def "+def);
			//Log.Message("Det "+progress + " " + def.targetValue);
			return (float)progress / def.targetValue;
		}

		public void Complete()
		{
			def.IsFinished = true;

			if (giverPawn != null)
			{
				Log.Message("Affect " + giverPawn.Faction + " with " + (def.rewardValue / 500f));
				giverPawn.Faction.AffectGoodwillWith(Faction.OfPlayer, def.rewardValue / 500f);
			}

			ChallengeManager.instance.ClearChallenge();
			Controller.ChallengeComplete(def);

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.AnyPlayerHomeMap); // drop around base
			TargetInfo targetInfo = new TargetInfo(dropSpot, Find.AnyPlayerHomeMap, false);
			Find.LetterStack.ReceiveLetter("ChallengeCompleted".Translate(), def.messageComplete, LetterDefOf.PositiveEvent, targetInfo, null);
			DropPodUtility.DropThingsNear(dropSpot, Find.AnyPlayerHomeMap, def.GetReward());

		}

		public void Interrupt()
		{
			if (giverPawn != null)
			{
				giverPawn.Faction.AffectGoodwillWith(Faction.OfPlayer, -def.rewardValue / 500f);
				string text = "MessageRelationsDegrade".Translate(new object[] {giverPawn.Faction});
				Messages.Message(text.CapitalizeFirst(), MessageTypeDefOf.NegativeEvent);
				Log.Message("Affect " + giverPawn.Faction + " with " + (-def.rewardValue / 500f));

				if (giverPawn.Faction.RelationWith(Faction.OfPlayer).hostile) {
					TriggerRaid(giverPawn.Faction);
				}

			}
			ChallengeManager.instance.ClearChallenge();
		}

		public virtual bool CanPick()
		{
			return true;
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

		public virtual void OnDestroyMined(Mineable block)
		{
		}

		public virtual void OnSkillLearned(SkillRecord skill, Pawn pawn, int oldSkillLevel)
		{
		}

		public virtual void OnThingProduced(Thing result, Pawn worker)
		{
		}

		public void TriggerRaid(Faction enemyFac)
		{
			Map map = Find.AnyPlayerHomeMap;
			IntVec3 spawnSpot;
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return;
			}

			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, IncidentCategory.ThreatBig, map);
			incidentParms.forced = true;
			incidentParms.faction = enemyFac;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.raidArrivalMode = PawnsArriveMode.EdgeWalkIn;
			incidentParms.spawnCenter = spawnSpot;
			incidentParms.points *= 1.35f;
			int when = Find.TickManager.TicksGame + ChallengeWorker.RaidDelay.RandomInRange;
			QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDefOf.RaidEnemy, null, incidentParms), when);
			Find.Storyteller.incidentQueue.Add(qi);
		}
	}

	public class ChallengeWorkerNone : ChallengeWorker
	{
		public ChallengeWorkerNone() : base(null, null)
		{
		}
	}
}
