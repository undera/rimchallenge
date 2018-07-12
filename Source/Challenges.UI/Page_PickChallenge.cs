using System;
using System.Collections.Generic;
using Challenges;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimchallenge
{
	public class Page_PickChallenge : Page
	{
		private ChallengeDef selectedChallenge;

		private Vector2 challengesScrollPosition = Vector2.zero;

        private float totalChallengeListHeight;


        public Page_PickChallenge()
		{
			foreach (ChallengeDef cDef in DefDatabase<ChallengeDef>.AllDefs)
            {
                Controller.checkFinished(cDef);
            }
		}

		public override string PageTitle
		{
			get
			{
				return "Pick Your Challenge";
			}
		}

		public override void PreOpen()
        {
            base.PreOpen();
			if (selectedChallenge == null) {
				selectedChallenge = DefDatabase<ChallengeDef>.AllDefs.FirstOrFallback(null);
			}
        }

		public override void DoWindowContents(Rect inRect)
		{
			base.DrawPageTitle(inRect);
			Rect mainRect = base.GetMainRect(inRect, 0f, false);
			this.DoChallengeSelectionList(mainRect);
			base.DoBottomButtons(inRect, null, null, null, true);
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.selectedChallenge == null)
			{
				Messages.Message("MustSelectChallenge".Translate(), MessageTypeDefOf.RejectInput);
				return false;
			}

			foreach (ScenPart item in Current.Game.Scenario.AllParts)
			{
				if (item is ScenPart_PickChallenge)
				{
					((ScenPart_PickChallenge)item).setChallenge(this.selectedChallenge);
					return true;
				}
			}
			return false;
		}

		private void DoChallengeSelectionList(Rect rect)
		{
			rect.xMax += 2f;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f - 2f, this.totalChallengeListHeight + 250f);
			Widgets.BeginScrollView(rect, ref this.challengesScrollPosition, rect2, true);
			Rect rect3 = rect2.AtZero();
			rect3.height = 999999f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect2.width;
			listing_Standard.Begin(rect3);
			Text.Font = GameFont.Small;
			this.ListScenariosOnListing(listing_Standard, DefDatabase<ChallengeDef>.AllDefs);
			listing_Standard.End();
			this.totalChallengeListHeight = listing_Standard.CurHeight;
			Widgets.EndScrollView();
		}

		private void ListScenariosOnListing(Listing_Standard listing, IEnumerable<ChallengeDef> challenges)
		{
			bool flag = false;
			foreach (ChallengeDef current in challenges)
			{
				if (flag)
				{
					listing.Gap(12f);
				}
				ChallengeDef scen = current;
				Rect rect = listing.GetRect(62f);
				this.DoScenarioListEntry(rect, scen);
				flag = true;
			}
			if (!flag)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				listing.Label("(" + "NoneLower".Translate() + ")", -1f);
				GUI.color = Color.white;
			}
		}

		private void DoScenarioListEntry(Rect rect, ChallengeDef challenge)
		{
			bool isSelected = this.selectedChallenge == challenge;
			bool canPick = challenge.CanStartNow;

			Widgets.DrawOptionBackground(rect, isSelected);
			Rect rect2 = rect.ContractedBy(4f);

			if (!canPick)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			else
			{
				MouseoverSounds.DoRegion(rect);
			}
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.height = Text.CalcHeight(challenge.LabelCap, rect3.width);
			Widgets.Label(rect3, challenge.LabelCap);
			Text.Font = GameFont.Tiny;
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMax;
			Widgets.Label(rect4, challenge.description);

			GUI.color = Color.white;


			if (canPick)
			{
				if (!isSelected && Widgets.ButtonInvisible(rect, false))
				{
					this.selectedChallenge = challenge;
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
		}
	}
}
