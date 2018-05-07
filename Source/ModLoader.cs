
using System;
using System.Collections.Generic;
using System.Linq;
using Rimchallenge.Challenges;
using Verse;

namespace Rimchallenge
{
	public class ModLoader: HugsLib.ModBase
	{
		public static ModLoader instance { get; private set; }
		public ChallengeBase currentChallenge { get; private set; } = new NoneChallenge();
        private readonly List<ChallengeBase> allChallenges = new List<ChallengeBase>();

        public ModLoader()
        {
			instance = this;

			foreach (Type current in typeof(ChallengeBase).AllLeafSubclasses())
            {
				this.allChallenges.Add((ChallengeBase)Activator.CreateInstance(current));
            }

        }

		public override string ModIdentifier
        {
            get { return "RimChallenges"; }
        }

        public override void DefsLoaded()
		{
			base.DefsLoaded();
			EventBridge.Hook(this);
		}

		internal void StartChallenge(ChallengeBase challenge)
		{
			// TODO: check no challenge is chosen yet
			// TODO: check it is known and enabled
			currentChallenge = challenge;
			currentChallenge.Initialize();
			Log.Message("set current challenge to "+currentChallenge);
		}

		public void ClearChallenge()
        {
            currentChallenge = new NoneChallenge();
        }
	}

}
