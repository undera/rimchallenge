using System;
using Rimchallenge.Thieves;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Rimchallenge
{
	public class RaidStrategyWorker_Steal:RaidStrategyWorker
    {
		public override LordJob MakeLordJob(IncidentParms parms, Map map)
        {
            return new LordJob_StealUnconditional();
        }
	}

}
