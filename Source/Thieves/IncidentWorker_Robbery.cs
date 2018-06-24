using System.Linq;
using RimWorld;
using Verse;

namespace Rimchallenge
{
	public class IncidentWorker_Robbery: IncidentWorker_RaidEnemy
    {
		protected override string GetLetterText(IncidentParms parms, System.Collections.Generic.List<Pawn> pawns)
		{
			return "RobberyRaid".Translate(new object[]
                        {
                            parms.faction.def.pawnsPlural,
                            parms.faction.Name
                        });
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
        {
			Log.Message("Target: "+target);           
			int awake = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.Count(x => x.Awake() && !x.Downed && !x.InBed());
			Log.Message("Colonists up: "+awake);           
			return awake  == 0;
        }
  
        protected override void ResolveRaidStrategy(IncidentParms parms)
		{
			parms.raidStrategy = DefDatabase<RaidStrategyDef>.GetNamed("Steal");
			base.ResolveRaidStrategy(parms);
		}
    }
}
