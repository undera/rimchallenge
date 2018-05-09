using System;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;

namespace Verse
{
    public abstract class ChallengeWorker
    {		
		public ChallengeDef def { get; set; }
		private int _progress = 0;

        public int progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
				ChallengeManager.instance.progress = _progress;
            }
        }

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

		public virtual float getProgressFloat()
        {
            return (float)progress / def.targetValue;
        }
        

        protected virtual void Complete()
        {
			def.IsFinished = true;
			ChallengeManager.instance.ClearChallenge();

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.VisibleMap); // drop around base
            TargetInfo targetInfo = new TargetInfo(dropSpot, Find.VisibleMap, false);
            Find.LetterStack.ReceiveLetter("Challenge Complete".Translate(), def.messageComplete, LetterDefOf.PositiveEvent, targetInfo, null);
            DropPodUtility.DropThingsNear(dropSpot, Find.VisibleMap, GetGratification());
        }

        
		public virtual void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
        {
        }

        public virtual void OnPawnFactionSet(Pawn pawn)
        {
        }

		public virtual bool CanPick()
		{
			return true;
		}

		public static IEnumerable<Pawn> AllColonists
        {
            get
            {
                List<Map> maps = Find.Maps;
                for (int i = 0; i < maps.Count; i++)
                {
                    if (maps[i].IsPlayerHome)
                    {
                        foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
                        {
                            yield return p;
                        }
                    }
                }
            }
        }

	}

	public class ChallengeWorkerNone : ChallengeWorker
	{
		public ChallengeWorkerNone() : base(null)
		{
		}
	}
}
