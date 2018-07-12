using System.Linq;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;
using System;

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
			return (float)progress / def.targetValue;
		}

		public void Complete()
		{
			def.IsFinished = true;

			ChallengeManager.instance.ClearChallenge();
			Controller.ChallengeComplete(def);
			IEnumerable<Thing> reward = def.GetReward();

			string rewardText = "";
			foreach (Thing thing in reward ) {
				rewardText += "  - " + GenLabel.ThingLabel(thing.def, thing.Stuff, thing.stackCount).CapitalizeFirst() + "\n";
			}

			GenGameEnd.EndGameDialogMessage("ChallengeCompletedEndDialog".Translate(new[] { def.label, def.description, rewardText }));

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.AnyPlayerHomeMap); // drop around base
			DropPodUtility.DropThingsNear(dropSpot, Find.AnyPlayerHomeMap, reward);
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
			incidentParms.raidStrategy = DefDatabase<RaidStrategyDef>.AllDefs.RandomElement();
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
		public ChallengeWorkerNone(ChallengeDef def) : base(def)
		{
		}
	}
}
