using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Verse
{
	public class ChallengeDef:Def
	{
		public Type workerClass=typeof(ChallengeWorker);

        [MustTranslate]
		public string messageComplete = "Challenge Complete! Get the prize!";

		public List<ThingCountClass> gratificationThings=new List<ThingCountClass>(0);
        
        public ChallengeDef()
		{
		}
       
	}
}
