
using Rimchallenge.Challenges;

namespace Rimchallenge
{
	public class ModLoader: HugsLib.ModBase
	{
		public static ModLoader instance { get; private set; }
		public ChallengeBase currentChallenge { get; private set; }
        
        public ModLoader()
        {
			instance = this;
			currentChallenge = new NoneChallenge();
        }

		public override string ModIdentifier
        {
            get { return "RimChallenges"; }
        }

        public override void DefsLoaded()
		{
			base.DefsLoaded();
			Logger.Message("Logger Test");
			// TODO: inject harmony patches here
		}

		public ChallengeBase GetCurrentChallenge()
		{
			return currentChallenge;
		}
	}

}
