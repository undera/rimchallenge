using System;
using RimWorld;
using Verse;

namespace Rimchallenge
{
	public class ChallengesTabWorker : MainButtonWorker_ToggleTab
	{
		public override float ButtonBarPercent
		{
			get
			{
				if (!ChallengeManager.instance.HasChallenge())
				{
					return 0f;
				}
				float progress = ChallengeManager.instance.currentChallenge.getProgressFloat();
				return progress > 1f ? 1f : progress;
			}
		}
	}
}
