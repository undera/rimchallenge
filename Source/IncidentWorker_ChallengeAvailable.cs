using System;
using System.Linq;
using Rimchallenge;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ChallengeAvailable : IncidentWorker
	{

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			if (ChallengeManager.instance.HasChallenge()) {
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.VisibleMap;
				QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDefOf.TravelerGroup, null, incidentParms), Find.TickManager.TicksGame);
				Find.Storyteller.incidentQueue.Add(qi);
			}
			return !ChallengeManager.instance.HasChallenge(); 
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			ChallengeDef offeredChallenge = ChallengeManager.instance.GetOfferedChallenge();
			if (offeredChallenge == null)
			{
				Log.Message("No challenge to offer");
				return false;
			}

			Letter letter = ChallengeManager.MakeLetter(offeredChallenge, Find.WorldPawns.AllPawnsAlive.RandomElement());
			Find.LetterStack.ReceiveLetter(letter);

			return true;
		}


	}
}
