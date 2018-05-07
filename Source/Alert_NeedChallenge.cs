using System;
using RimWorld;
using Verse;

namespace rimchallenge
{
	public class Alert_NeedChallenge:Alert
    {
        public Alert_NeedChallenge()
        {
			this.defaultLabel = "NeedChallenge".Translate();
            this.defaultExplanation = "NeedChallengeDesc".Translate();			
        }

		public override AlertReport GetReport()
		{
			return true;
		}
	}
}
