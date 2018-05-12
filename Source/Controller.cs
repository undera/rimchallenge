using System;
using Verse;

namespace Rimchallenge
{
	public class Controller: Mod
	{
		private static Controller instance;

		public Controller(ModContentPack content) : base(content)
        {
			instance = this;
			EventBridge.Hook(this);
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


	}   
}
// TODO: save challenge into save file
// TODO: save challenge revealed state globally