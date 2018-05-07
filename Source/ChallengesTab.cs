using System;
using Rimchallenge.Challenges;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class ChallengesTab: MainTabWindow
    {
		public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);
            this.windowRect.width = (float)UI.screenWidth;

			Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
            Rect leftOutRect = new Rect(0f, 0f, 200f, inRect.height);
            Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
            this.DrawLeftRect(leftOutRect);
            this.DrawRightRect(rightOutRect);
        }

		private void DrawLeftRect(Rect leftOutRect)
        {
			if (Widgets.ButtonText(leftOutRect, "Pick This Challenge".Translate(), true, false, ModLoader.instance.currentChallenge is NoneChallenge))
            {
				ModLoader.instance.StartChallenge(new TestChallenge());
            }
        }

		private void DrawRightRect(Rect rightOutRect)
		{
			
		}

	}
}
