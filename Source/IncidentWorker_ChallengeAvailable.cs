using System;
using System.Linq;
using Rimchallenge;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ChallengeAvailable: IncidentWorker
    {

        protected override bool CanFireNowSub(IIncidentTarget target)
		{
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

			string text = "Hey survivor!\n\nI'm interested in changing this world to better, and would appreciate your help by sharing some goods. Here's the task:\n";
			text+="\n"+offeredChallenge.description;
			text += "\n\n" + offeredChallenge.RewardsText();
			text += "\n\nAre you capable of doing this?";
            DiaNode dialog = new DiaNode(text);
			DiaOption optAccepted = new DiaOption("Challenge Accepted!");
            optAccepted.action = delegate
            {
				ChallengeManager.instance.StartChallenge(offeredChallenge);
            };
            optAccepted.resolveTree = true;
            dialog.options.Add(optAccepted);

			DiaOption optReject = new DiaOption("Reject");
			optReject.resolveTree = true;
			dialog.options.Add(optReject);
			string title = "Message Over Radio from a Stranger: "+offeredChallenge.LabelCap;
            Find.WindowStack.Add(new Dialog_NodeTree(dialog, true, true, title));
            return true;
        }
    }
}
