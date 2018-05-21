using System;
using System.Linq;
using RimWorld;
using Verse;

namespace Challenges
{
	public class Challenge_SurviveRaids : ChallengeWorker
	{
		public Challenge_SurviveRaids(ChallengeDef def, Pawn giver) : base(def, giver)
		{
		}

		public override bool CanPick()
		{
			return Find.FactionManager.AllFactions.Count(x => x.HostileTo(Faction.OfPlayer)) > 0;
		}

		public override void Started()
		{
			switch (def.param1)
			{
				case 1:
					StartRaid(new IntRange(100, 500), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttack"));
					StartRaid(new IntRange(100, 500), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttackSmart"));
					StartRaid(new IntRange(100, 500), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttackSappers"));
										break;
				case 2:
					StartSiege(new IntRange(100, 500));
					StartSiege(new IntRange(100, 500));
					StartRaid(new IntRange(10000, 25000), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttack"));
                    StartRaid(new IntRange(10000, 25000), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttackSmart"));
                    StartRaid(new IntRange(10000, 25000), DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttackSappers"));
					break;
				default:
					return;
			}
		}

		private void StartSiege(IntRange delay)
		{
			QueueEvent(DefDatabase<RaidStrategyDef>.GetNamed("Siege"), delay);
		}

		private void StartRaid(IntRange delay, RaidStrategyDef strat)
		{
			QueueEvent(strat, delay);
		}

		private void QueueEvent(RaidStrategyDef strat, IntRange delay)
		{
			Map map = Find.AnyPlayerHomeMap;
			IntVec3 spawnSpot;
			if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnSpot))
			{
				return;
			}

			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, IncidentCategory.ThreatBig, map);
			incidentParms.forced = true;
			incidentParms.faction = Find.FactionManager.RandomEnemyFaction();
			incidentParms.points = Challenge_NColonists.AllColonists.Count()*100f;
			incidentParms.raidStrategy = strat;
			incidentParms.raidNeverFleeIndividual = true;
			int when = Find.TickManager.TicksGame + delay.RandomInRange;
			QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDefOf.RaidEnemy, null, incidentParms), when);
			Find.Storyteller.incidentQueue.Add(qi);
		}
	}
}
