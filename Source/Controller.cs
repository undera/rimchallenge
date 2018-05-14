using System;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class Controller: Mod
	{
		private static Controller instance;

		public Controller(ModContentPack content) : base(content)
        {
			instance = this;
			EventBridge.Hook();
        }

		internal static void ChallengeComplete(ChallengeDef def)
		{
			instance.GetSettings<CompletedChallengesList>().AddCompleted(def);
			instance.WriteSettings();
		}

		internal static void checkFinished(ChallengeDef def)
		{
			CompletedChallengesList completed = instance.GetSettings<CompletedChallengesList>();
			def.IsFinished = completed.isFinished(def);
		}

        public override string SettingsCategory()
		{
			return "Challenges";
		}

        public override void DoSettingsWindowContents(Rect inRect)
		{
			Rect btnRect = new Rect(inRect.x+inRect.width / 3, inRect.y+inRect.height / 3, inRect.width / 3, inRect.height / 3);
			if (Widgets.ButtonText(btnRect, "Reset Complete Challenges", true, false, true))
			{
				GetSettings<CompletedChallengesList>().Reset();
				WriteSettings();
			}
		}
	}   
}
// TODO: save challenge into save file
// TODO: save challenge revealed state globally