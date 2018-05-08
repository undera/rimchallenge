using System;
using Verse;

namespace Rimchallenge
{
	public class ModLoader: Mod
	{
		public static ModLoader instance { get; private set; }
		public ChallengeWorker currentChallenge { get; private set; } = null;

		public ModLoader(ModContentPack content) : base(content)
        {
			Log.Message("Starting Challenge Mod");
			instance = this;

			EventBridge.Hook(this);
        }

		internal bool HasChallenge()
		{
			return currentChallenge != null;
		}

		internal void StartChallenge(ChallengeDef challengeDef)
		{
			// TODO: check no challenge is chosen yet
			// TODO: check it is known and enabled
			Log.Message("set current challenge to " + challengeDef);
			currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef);
		}

		public void ClearChallenge()
        {
			currentChallenge = null;
        }
	}

}
// TODO: save challenge into save file
// TODO: save challenge revealed state globally