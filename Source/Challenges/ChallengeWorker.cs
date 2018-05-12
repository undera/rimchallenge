using System.Linq;
using System.Collections.Generic;
using Rimchallenge;
using RimWorld;
using Verse;
using System;

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

		private IEnumerable<Thing> GetReward()
		{
			List<Thing> res = new List<Thing>();
			for (int j = 0; j < def.reward.Count; j++)
			{
				Thing thing = ThingMaker.MakeThing(def.reward[j].thingDef);
				thing.stackCount = def.reward[j].count;
				res.Add(thing);
			}

			return res;
		}

		public virtual float getProgressFloat()
		{
			//Log.Message("Def "+def);
			//Log.Message("Det "+progress + " " + def.targetValue);
			return (float)progress / def.targetValue;
		}

		public void Complete()
		{
			def.IsFinished = true;
			ChallengeManager.instance.ClearChallenge();
			Controller.ChallengeComplete(def);

			IntVec3 dropSpot = DropCellFinder.TradeDropSpot(Find.AnyPlayerHomeMap); // drop around base
			TargetInfo targetInfo = new TargetInfo(dropSpot, Find.AnyPlayerHomeMap, false);
			Find.LetterStack.ReceiveLetter("Challenge Complete", def.messageComplete, LetterDefOf.PositiveEvent, targetInfo, null);
			DropPodUtility.DropThingsNear(dropSpot, Find.AnyPlayerHomeMap, GetReward());
		}


		public virtual void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
		}

		public virtual void OnPawnDestroyed(Pawn pawn)
		{
		}

		public virtual void OnPawnFactionSet(Pawn pawn)
		{
		}

		public virtual void OnDestroyMined(Mineable block)
        {
        }

		public virtual bool CanPick()
		{
			return true;
		}
	}

	public class ChallengeWorkerNone : ChallengeWorker
	{
		public ChallengeWorkerNone() : base(null)
		{
		}
	}
}
