using System;
using Challenges;
using RimWorld;
using UnityEngine;
using Verse;

namespace Rimchallenge
{
	public class ScenPart_PickChallenge:ScenPart_ConfigPage
    {
        internal void setChallenge(ChallengeDef selectedChallenge)
        {
			Log.Message("Picking challenge...");
			ChallengeManager.instance.StartChallenge(selectedChallenge);
        }
    }
}
