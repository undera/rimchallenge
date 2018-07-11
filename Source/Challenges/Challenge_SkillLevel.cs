using System;
using RimWorld;
using Verse;

namespace Challenges
{
	public class Challenge_SkillLevel : ChallengeWorker
	{		
		public Challenge_SkillLevel(ChallengeDef def) : base(def)
		{
		}

		public override void OnSkillLearned(SkillRecord skill, Pawn pawn, int oldSkillLevel)
		{
			if (oldSkillLevel != skill.levelInt)
			{
				string text = "MessageSkillChanged".Translate(new object[]
					{
					pawn.LabelShort, ("Skill"+skill.levelInt).Translate().ToLower(), skill.levelInt, skill.def.label
				});

				MessageTypeDef flag = oldSkillLevel < skill.levelInt ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent;
				Messages.Message(text.CapitalizeFirst(), new TargetInfo(pawn.Position, pawn.Map, false), flag);
			}

			progress = Math.Max(progress, skill.levelInt);
			if (skill.levelInt >= this.def.targetValue)
			{
				Complete();
			}
		}
	}
}
