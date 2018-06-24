using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Rimchallenge.Thieves
{
	public class LordJob_StealUnconditional : LordJob
	{
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
            LordToil_StealCover lordToil_StealCover = new LordToil_StealCoverCustom();
			lordToil_StealCover.avoidGridMode = AvoidGridMode.Ignore;
			lordToil_StealCover.cover = false;
            stateGraph.AddToil(lordToil_StealCover);
            return stateGraph;
		}
	}

	internal class LordToil_StealCoverCustom : LordToil_StealCover
	{
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing != null)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			return LordToil_StealCoverCustom.TryFindBestItemToSteal(pawn.Position, pawn.Map, 9999999f, out target, pawn, alreadyTakenTargets);
		}

		public static bool TryFindBestItemToSteal(IntVec3 root, Map map, float maxDist, out Thing item, Pawn thief, List<Thing> disallowed = null)
		{
			if (thief != null && !thief.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				item = null;
				return false;
			}
			Predicate<Thing> validator = (Thing t) => (thief == null || thief.CanReserve(t, 1, -1, null, false)) && (disallowed == null || !disallowed.Contains(t)) && (t.def!=null && t.def.stealable) && !t.IsBurning();
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable);
			Func<Thing, float> priorityGetter = new Func<Thing, float>(StealAIUtility.GetValue);
			item = GenClosest.ClosestThing_Global(root, map.listerThings.ThingsMatching(thingReq), maxDist, validator, priorityGetter);
			Log.Message("Item to steal: " + item);
			return item != null;
		}
	}
}
