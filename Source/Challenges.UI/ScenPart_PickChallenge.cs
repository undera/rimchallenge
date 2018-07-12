using System;
using System.Collections.Generic;
using Challenges;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class ScenPart_PickChallenge:ScenPart
    {
        internal void setChallenge(ChallengeDef selectedChallenge)
        {
			Log.Message("Picking challenge...");
			ChallengeManager.instance.StartChallenge(selectedChallenge);
        }

        public override IEnumerable<Page> GetConfigPages()
		{
			yield return new Page_PickChallenge();
		}
    }
}
