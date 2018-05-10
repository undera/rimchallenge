using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using System;
using System.Linq;
using System.Text;

namespace Rimchallenge
{
	public class ChallengesTab : MainTabWindow
	{
		protected ChallengeDef selectedChallenge;
		private float leftScrollViewHeight;

		private Vector2 leftScrollPosition = Vector2.zero;

		private Vector2 rightScrollPosition = default(Vector2);

		private float rightViewWidth = 0;

		private float rightViewHeight = 0;

		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

        public ChallengesTab()
		{
		}

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
			Rect position = leftOutRect;
            GUI.BeginGroup(position);
            if (this.selectedChallenge != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, 500f);
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
				float leftHeight = 0f;

				// header
				Text.Font = GameFont.Medium;
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Rect headerRect = new Rect(0f, leftHeight, viewRect.width, 50f);
				Widgets.LabelCacheHeight(ref headerRect, this.selectedChallenge.LabelCap, true, false);

				// description
				GenUI.ResetLabelAlign();
				Text.Font = GameFont.Small;
				leftHeight += headerRect.height;
				Rect descrRect = new Rect(0f, leftHeight, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref descrRect, this.selectedChallenge.description, true, false);
				leftHeight += descrRect.height + 10f;

				DrawChallengeDetails(ref viewRect, ref leftHeight);

				leftHeight += 3f;
				this.leftScrollViewHeight = leftHeight;
				Widgets.EndScrollView();
				DrawButtonsAndProgress(position, outRect);
			}
			GUI.EndGroup();
		}

		private void DrawChallengeDetails(ref Rect viewRect, ref float leftHeight)
		{
			string text = this.RewardsText(this.selectedChallenge);

            /*
			float num2 = this.selectedChallenge.targetValue;
			if (num2 != 1.0f)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
						text2,
						"\n\n",
						"ResearchCostMultiplier".Translate().CapitalizeFirst(),
						": ",
						num2.ToStringPercent(),
						"\n",
						"ResearchCostComparison".Translate(new object[]
						{
							this.selectedChallenge.targetValue.ToString("F0"),
							this.selectedChallenge.targetValue.ToString("F0")
						})
				});
			}
			*/
			Rect detailsRect = new Rect(0f, leftHeight, viewRect.width, 0f);
			Widgets.LabelCacheHeight(ref detailsRect, text, true, false);
			leftHeight = detailsRect.yMax + 10f;

            /*
			Rect prereqsRect = new Rect(0f, leftHeight, viewRect.width, 500f);
			float num3 = 0;this.DrawResearchPrereqs(this.selectedChallenge, prereqsRect);
            if (num3 > 0f)
            {
                leftHeight += num3 + 15f;
            }
            Rect rect5 = new Rect(0f, leftHeight, viewRect.width, 500f);
            leftHeight += this.DrawResearchBenchRequirements(this.selectedChallenge, rect5);
            */
            
		}

		private string RewardsText(ChallengeDef challenge)
        {
			string stringBuilder = "Rewards: \n";
			foreach (ThingCountClass current in challenge.reward)
            {
				string stringLabel = GenLabel.ThingLabel(current.thingDef, null, current.count).CapitalizeFirst();
				stringBuilder+=("   -" + stringLabel+"\n");
            }
            return stringBuilder;
        }


		private void DrawButtonsAndProgress(Rect position, Rect outRect)
		{			
			bool showDebugBtns = Prefs.DevMode && this.selectedChallenge != ChallengeManager.instance.currentChallengeDef && !this.selectedChallenge.IsFinished;
            Rect rect6 = new Rect(0f, 0f, 90f, 50f);
            if (showDebugBtns)
            {
                rect6.x = (outRect.width - (rect6.width * 2f + 20f)) / 2f;
            }
            else
            {
                rect6.x = (outRect.width - rect6.width) / 2f;
            }


			rect6.y = outRect.y + outRect.height + 20f;
			if (this.selectedChallenge.IsFinished)
			{
				Widgets.DrawMenuSection(rect6);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect6, "Finished".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else if (this.selectedChallenge == ChallengeManager.instance.currentChallengeDef)
			{
				Widgets.DrawMenuSection(rect6);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect6, "InProgress".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else if (!this.selectedChallenge.CanStartNow)
			{
				Widgets.DrawMenuSection(rect6);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect6, "Locked".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else if (!ChallengeManager.instance.HasChallenge() && Widgets.ButtonText(rect6, "Accept This Challenge", true, false, true))
			{
				SoundDef.Named("ResearchStart").PlayOneShotOnCamera(null);
				ChallengeManager.instance.StartChallenge(selectedChallenge);
			}
			if (showDebugBtns)
			{
				Rect rect7 = rect6;
				rect7.x += rect7.width + 20f;
				if (Widgets.ButtonText(rect7, "Debug Insta-finish", true, false, true))
				{
					ChallengeManager.instance.StartChallenge(selectedChallenge);
					ChallengeManager.instance.currentChallenge.Complete();
				}
			}

			if (selectedChallenge.targetValue!=0)
			{
				ChallengeWorker curChallenge = ChallengeManager.instance.currentChallenge;
				float progress = selectedChallenge.EstimateProgressFloat();
				Rect rect8 = new Rect(15f, rect6.y + rect6.height + 20f, position.width - 30f, 35f);
				Widgets.FillableBar(rect8, progress, ChallengesTab.ResearchBarFillTex, ChallengesTab.ResearchBarBGTex, true);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect8, ((int)(progress * selectedChallenge.targetValue)).ToString("F0") + " / " + selectedChallenge.targetValue.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Rect outRect = rightOutRect.ContractedBy(10f);
			this.rightViewWidth = outRect.width;
			this.rightViewHeight = outRect.height;
			Rect rect = new Rect(0f, 0f, this.rightViewWidth, this.rightViewHeight);
			Rect position = rect.ContractedBy(10f);
			rect.width = this.rightViewWidth;
			position = rect.ContractedBy(10f);

			Widgets.ScrollHorizontal(outRect, ref this.rightScrollPosition, rect, 20f);
			Widgets.BeginScrollView(outRect, ref this.rightScrollPosition, rect, true);
			GUI.BeginGroup(position);

			List<ChallengeDef> allDefsListForReading = DefDatabase<ChallengeDef>.AllDefsListForReading;
			DrawDependencies(allDefsListForReading);
			DrawBoxes(allDefsListForReading);

			GUI.EndGroup();
			Widgets.EndScrollView();
		}


		private void DrawDependencies(List<ChallengeDef> allDefsListForReading)
		{
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);

			for (int passN = 0; passN < 2; passN++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					ChallengeDef challengeDef = allDefsListForReading[j];
					start.x = this.PosX(challengeDef);
					start.y = this.PosY(challengeDef) + 25f;
					for (int k = 0; k < challengeDef.prerequisites.CountAllowNull<ChallengeDef>(); k++)
					{
						ChallengeDef prerequisite = challengeDef.prerequisites[k];
						if (prerequisite != null)
						{
							end.x = this.PosX(prerequisite) + 140f;
							end.y = this.PosY(prerequisite) + 25f;
							if (this.selectedChallenge == challengeDef || this.selectedChallenge == prerequisite)
							{
								if (passN == 1)
								{
									Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
								}
							}
							else if (passN == 0)
							{
								Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
							}
						}
					}
				}
			}
		}


		private void DrawBoxes(List<ChallengeDef> allDefsListForReading)
		{
			for (int l = 0; l < allDefsListForReading.Count; l++)
			{
				ChallengeDef aChallenge = allDefsListForReading[l];
				Rect source = new Rect(this.PosX(aChallenge), this.PosY(aChallenge), 140f, 50f);
				string label = aChallenge.label;
				Rect btnRect = new Rect(source);
				Color textColor = Widgets.NormalOptionColor;
				Color color = default(Color);
				Color borderColor = default(Color);
				bool canPickIt = aChallenge.CanStartNow;

				if (aChallenge == ChallengeManager.instance.currentChallenge.def)
				{
					color = TexUI.ActiveResearchColor;
				}
				else if (aChallenge.IsFinished)
				{
					color = TexUI.FinishedResearchColor;
				}
				else if (!canPickIt)
				{
					color = TexUI.LockedResearchColor;
				}
				else if (aChallenge.CanStartNow)
				{
					color = TexUI.AvailResearchColor;
				}

				if (this.selectedChallenge == aChallenge)
				{
					color += TexUI.HighlightBgResearchColor;
					borderColor = TexUI.HighlightBorderResearchColor;
				}
				else
				{
					borderColor = TexUI.DefaultBorderResearchColor;
				}

				if (!canPickIt)
				{
					textColor = Color.gray;
				}

				for (int m = 0; m < aChallenge.prerequisites.CountAllowNull<ChallengeDef>(); m++)
				{
					ChallengeDef researchProjectDef4 = aChallenge.prerequisites[m];
					if (researchProjectDef4 != null && this.selectedChallenge == researchProjectDef4)
					{
						borderColor = TexUI.HighlightLineResearchColor;
					}
				}

				/*
				if (this.requiredByThisFound) 
				{
					for (int n = 0; n < aChallenge.requiredByThis.CountAllowNull<ChallengeDef>(); n++)
					{
						ChallengeDef researchProjectDef5 = aChallenge.requiredByThis[n];
						if (this.selectedChallenge == researchProjectDef5)
						{
							borderColor = TexUI.HighlightLineResearchColor;
						}
					}
				}*/

				if (Widgets.CustomButtonText(ref btnRect, label, color, textColor, borderColor, true, 1, true, true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.selectedChallenge = aChallenge;
				}
			}
		}
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}


		private float PosX(ChallengeDef d)
		{
			return this.CoordToPixelsX(d.TabViewFlexX);
		}

		private float PosY(ChallengeDef d)
		{
			return this.CoordToPixelsY(d.TabViewFlexY);
		}


	}
}
