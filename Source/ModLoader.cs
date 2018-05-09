using System;
using Verse;

namespace Rimchallenge
{
	public class ModLoader: Mod
	{
		public static ModLoader instance { get; private set; }
		public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone();

		public ModLoader(ModContentPack content) : base(content)
        {
			Log.Message("Starting Challenge Mod");
			instance = this;

			EventBridge.Hook(this);
        }

		public bool HasChallenge()
		{
			return currentChallenge != null && !(currentChallenge is ChallengeWorkerNone);
		}

		public void StartChallenge(ChallengeDef challengeDef)
		{
			Log.Message("Picked a challenge: " + challengeDef);
			currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef);
		}

		public void ClearChallenge()
        {
			currentChallenge = new ChallengeWorkerNone();
        }
	}

}
// TODO: save challenge into save file
// TODO: save challenge revealed state globally