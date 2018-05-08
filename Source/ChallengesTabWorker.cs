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
				if (!ModLoader.instance.HasChallenge())
				{
					return 0f;
				}
				float progress = ModLoader.instance.currentChallenge.getProgress();
				return progress > 1f ? 1f : progress;
			}
		}
	}
}
