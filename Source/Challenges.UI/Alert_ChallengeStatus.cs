using System;
using Challenges;
using RimWorld;

namespace Rimchallenge
{
	public class Alert_ChallengeStatus : Alert
	{
		public override string GetLabel()
		{
			return ChallengeManager.instance.currentChallenge.def.LabelCap;
		}

		public override string GetExplanation()
		{
			ChallengeWorker cur = ChallengeManager.instance.currentChallenge;
			string text = cur.def.description;
			text += "\n\nProgress: ";
			if (cur.def.targetValue > 0)
			{
				text += cur.progress + "/" + cur.def.targetValue;
			}
			else
			{
				text += ((int)(100 * cur.getProgressFloat())) + "%";
			}

			if (cur.hint != null) {
				text += "\n\n" + cur.hint;
			}
			return text;
		}

		public override AlertReport GetReport()
		{
			return ChallengeManager.instance.HasChallenge();
		}
	}
}
