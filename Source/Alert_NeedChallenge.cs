using System;
using Rimchallenge.Challenges;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	// TODO: how to make it to open choice dialog on click?
	public class Alert_NeedChallenge: Alert
    {
        public Alert_NeedChallenge()
        {
			this.defaultLabel = "NeedChallenge";
            this.defaultExplanation = "NeedChallengeDesc";			
        }

		public override Rect DrawAt(float topY, bool minimized)
		{
			Rect rect = base.DrawAt(topY, minimized);

			if (Widgets.ButtonInvisible(rect, false) && !this.GetReport().culprit.IsValid)
            {
				Log.Message("Event0");
                CameraJumper.TryJumpAndSelect(this.GetReport().culprit);
            }


			if (Widgets.ButtonInvisible(rect, false))
			{
				Log.Message("Event");
			}
			else { 
				Log.Message("Rect "+rect);
			}

			return rect;
		}

		public override AlertReport GetReport()
		{
			bool val = ModLoader.instance.GetCurrentChallenge() is NoneChallenge;
			return val;
		}
	}
}
