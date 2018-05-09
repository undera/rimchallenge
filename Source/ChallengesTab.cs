using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Rimchallenge
{
	public class ChallengesTab : MainTabWindow
	{
		protected ChallengeDef selectedProject;

		private Vector2 leftScrollPosition = Vector2.zero;

		private float leftScrollViewHeight = 0;

		private Vector2 rightScrollPosition = default(Vector2);

		private float rightViewWidth = 0;

		private float rightViewHeight = 0;

        public ChallengesTab()
		{
			ChallengeDef.GenerateNonOverlappingCoordinates();
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
			if (!ModLoader.instance.HasChallenge())
			{
				if (Widgets.ButtonText(leftOutRect, "Pick This Challenge".Translate(), true, false, true))
				{
					ModLoader.instance.StartChallenge(DefDatabase<ChallengeDef>.GetRandom());
				}
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
			drawDependencies(allDefsListForReading);
			drawBoxes(allDefsListForReading);

			GUI.EndGroup();
			Widgets.EndScrollView();
		}


		private void drawDependencies(List<ChallengeDef> allDefsListForReading)
		{
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);

			for (int i = 0; i < 2; i++)
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
							if (this.selectedProject == challengeDef || this.selectedProject == prerequisite)
							{
								if (i == 1)
								{
									Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
								}
							}
							else if (i == 0)
							{
								Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
							}
						}
					}
				}
			}
		}


		private void drawBoxes(List<ChallengeDef> allDefsListForReading)
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
				bool canPickIt = !aChallenge.IsFinished && !aChallenge.CanStartNow;
				if (aChallenge == ModLoader.instance.currentChallenge.def)
				{
					color = TexUI.ActiveResearchColor;
				}
				else if (aChallenge.IsFinished)
				{
					color = TexUI.FinishedResearchColor;
				}
				else if (canPickIt)
				{
					color = TexUI.LockedResearchColor;
				}
				else if (aChallenge.CanStartNow)
				{
					color = TexUI.AvailResearchColor;
				}
				if (this.selectedProject == aChallenge)
				{
					color += TexUI.HighlightBgResearchColor;
					borderColor = TexUI.HighlightBorderResearchColor;
				}
				else
				{
					borderColor = TexUI.DefaultBorderResearchColor;
				}

				if (canPickIt)
				{
					textColor = Color.gray;
				}

				for (int m = 0; m < aChallenge.prerequisites.CountAllowNull<ChallengeDef>(); m++)
				{
					ChallengeDef researchProjectDef4 = aChallenge.prerequisites[m];
					if (researchProjectDef4 != null && this.selectedProject == researchProjectDef4)
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
						if (this.selectedProject == researchProjectDef5)
						{
							borderColor = TexUI.HighlightLineResearchColor;
						}
					}
				}*/

				if (Widgets.CustomButtonText(ref btnRect, label, color, textColor, borderColor, true, 1, true, true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.selectedProject = aChallenge;
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
