using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Rimchallenge.Challenges
{
	public abstract class ChallengeBase
	{
		public virtual void Complete()
		{
			ModLoader.instance.ClearChallenge();

			IntVec3 dropSpot = DropCellFinder.RandomDropSpot(Find.VisibleMap);

			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, IncidentCategory.ThreatSmall, Find.VisibleMap);
			incidentParms.faction = Faction.OfPlayer;
			incidentParms.spawnCenter = dropSpot;

			//Find.Storyteller.TryFire(new FiringIncident(new ChallengeCompleteIncident(this), null, incidentParms));
			DropPodUtility.DropThingsNear(dropSpot, Find.VisibleMap, GetGratification());
		}

		public virtual void Initialize()
		{
			Log.Message("Initialize challenge " + this);
		}

		public virtual void Interrupt()
		{
			ModLoader.instance.ClearChallenge();
		}

		abstract public List<Thing> GetGratification();

		public virtual void OnPawnKilled(Pawn pawn)
		{
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
