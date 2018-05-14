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
			return !ChallengeManager.instance.HasChallenge(); // TODO: remove when debugged, after lowering chance
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			ChallengeDef offeredChallenge = ChallengeManager.instance.GetOfferedChallenge();
			if (offeredChallenge == null)
			{
				Log.Message("No challenge to offer");
				return false;
			}

			Find.LetterStack.ReceiveLetter(GetLetter(offeredChallenge), null);

			return true;
		}

		private Letter GetLetter(ChallengeDef challenge)
		{
			string text = "Hey survivor!\n\nI'm interested in changing this world to better, and would appreciate your help by sharing some goods. Here's the task:\n";
			text += "\n" + challenge.description;
			text += "\n\n" + challenge.RewardsText();
            text += "\n\nAre you capable of doing this?";

			ChallengeAvailableLetter letter = new ChallengeAvailableLetter();
			letter.label = "Challenge Available";
			letter.title = "Message Over Radio from a Stranger: " + challenge.LabelCap;
			letter.text = text;
			letter.radioMode = true;
			letter.challenge = challenge;
			letter.StartTimeout(60000);
            
			return letter;
		}
	}
}
