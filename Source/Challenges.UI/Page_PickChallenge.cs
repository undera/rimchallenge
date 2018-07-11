using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class Page_PickChallenge : Page
	{
        public override string PageTitle
		{
			get
			{
				return "Pick Your Challenge";
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			base.DrawPageTitle(inRect);
			Rect mainRect = base.GetMainRect(inRect, 0f, false);
			Widgets.ButtonText(mainRect, "Test2", true, false, true);
			base.DoBottomButtons(inRect, null, null, null, true);
		}
	}
}
