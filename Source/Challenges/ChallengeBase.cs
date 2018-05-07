using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Rimchallenge.Challenges
{
    public abstract class ChallengeBase
    {
        public ChallengeBase()
        {
        }

		internal void Complete() {
			IntVec3 dropSpot = DropCellFinder.RandomDropSpot(Find.VisibleMap);           

			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, IncidentCategory.ThreatSmall, Find.VisibleMap);
			incidentParms.faction = Faction.OfPlayer;
			incidentParms.spawnCenter = dropSpot;

			Find.Storyteller.TryFire(new FiringIncident(new ChallengeCompleteIncident(this), null, incidentParms));
			DropPodUtility.DropThingsNear(dropSpot, Find.VisibleMap, GetGratification());
		}

		internal void Interrupt() { }

		abstract public List<Thing> GetGratification();

		public bool IsMapApplicable(Map map) // for challenges on biomes and map conditions
		{
			return true;
		}

    }

	public class ChallengeCompleteIncident : IncidentDef
	{
		public ChallengeCompleteIncident(ChallengeBase challengeBase)
		{
		}
	}

	public class NoneChallenge : ChallengeBase
	{
		public override List<Thing> GetGratification()
		{
			throw new System.NotImplementedException();
		}
	}

}
