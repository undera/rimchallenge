using System;
using Challenges;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class Alert_ChallengeStatus : Alert
	{
		public override string GetLabel()
		{
			if (ChallengeManager.instance.HasChallenge())
			{
				ChallengeWorker cur = ChallengeManager.instance.currentChallenge;
				return cur.def.LabelCap+" - "+((int)(100 * cur.getProgressFloat())) + "%";
			}
			else
			{
				return "";
			}
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

        /*
        public override Rect DrawAt(float topY, bool minimized)
		{
			Rect rect= base.DrawAt(topY, minimized);
			Rect rect2 = new Rect(rect.x, rect.y, rect.width*ChallengeManager.instance.currentChallenge.getProgressFloat(), rect.height);
			GUI.DrawTexture(rect2, TexUI.HighlightSelectedTex);
			return rect;
		}
		*/
	}
}
