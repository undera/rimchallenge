using System;
using RimWorld;
using Verse;

namespace Challenges
{
	public class Challenge_Masterpiece : ChallengeWorker
	{
		public Challenge_Masterpiece(ChallengeDef def) : base(def)
		{
		}

		public override void OnThingProduced(Thing result, Pawn worker)
		{
			if (result.def.thingCategories.Contains(getItemCategory()))
			{
				QualityCategory qualityCategory;
                result.TryGetQuality(out qualityCategory);

				Log.Message(worker + " has finished " + qualityCategory.GetLabel() + " " + result.Label);

				string text = "MessageThingProduced".Translate(new object[]
				{
					worker.LabelShort, result.Label
				});

				MessageTypeDef flag;

				switch (qualityCategory)
                {
                    case QualityCategory.Awful:
						flag = MessageTypeDefOf.TaskCompletion; break;
                    case QualityCategory.Poor:
						flag = MessageTypeDefOf.TaskCompletion; break;
                    case QualityCategory.Normal:
						flag = MessageTypeDefOf.NeutralEvent; break;
                    case QualityCategory.Good:
						flag = MessageTypeDefOf.NeutralEvent; break;
                    case QualityCategory.Excellent:
						flag = MessageTypeDefOf.PositiveEvent; break;
                    case QualityCategory.Masterwork:
						flag = MessageTypeDefOf.PositiveEvent; break;
                    case QualityCategory.Legendary:
						flag = MessageTypeDefOf.SituationResolved; break;
                    default:
                        throw new ArgumentException();
                }

				Messages.Message(text.CapitalizeFirst(), new TargetInfo(result.Position, worker.Map, false), flag);

				if (qualityCategory == QualityCategory.Legendary) {
					Complete();
				}
			}
		}

		private ThingCategoryDef getItemCategory()
		{
			switch (def.param1)
			{
				case 1:
					return ThingCategoryDefOf.BuildingsArt;
				case 2:
					return ThingCategoryDefOf.Apparel;
				default:
					return null;
			}
		}
	}
}
