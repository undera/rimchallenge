using System;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;

namespace Verse
{
    public abstract class ChallengeWorker
    {
		private ChallengeDef def;

		public ChallengeWorker(ChallengeDef def)
        {
			this.def = def;
        }

		private IEnumerable<Thing> GetGratification()
        {
            List<Thing> res = new List<Thing>();
            for (int j = 0; j < def.gratificationThings.Count; j++)
            {
                Thing thing = ThingMaker.MakeThing(def.gratificationThings[j].thingDef);
                thing.stackCount = def.gratificationThings[j].count;
                res.Add(thing);
            }

            return res;
        }

		protected virtual void Complete()
        {
            ModLoader.instance.ClearChallenge();

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.VisibleMap); // drop around base
            TargetInfo targetInfo = new TargetInfo(dropSpot, Find.VisibleMap, false);
            Find.LetterStack.ReceiveLetter("Challenge Complete".Translate(), def.messageComplete, LetterDefOf.PositiveEvent, targetInfo, null);
            DropPodUtility.DropThingsNear(dropSpot, Find.VisibleMap, GetGratification());
        }

        
        public virtual void OnPawnKilled(Pawn pawn)
        {
        }

        public virtual void OnPawnFactionSet(Pawn pawn)
        {
        }
    }
}
