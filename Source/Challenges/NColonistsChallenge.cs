using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Verse
{
	public class NColonistsChallenge : ChallengeWorker
	{
		private int colonistCount = -1;
		public NColonistsChallenge(ChallengeDef def) : base(def)
		{
		}

		public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			Check();
		}

		public override void OnPawnFactionSet(Pawn pawn)
		{
			Check();
		}

		private void Check() { 
			colonistCount = -1;
            this.progress = getColonistCount();
            if (this.progress >= def.targetValue)
            {
                Complete();
            }
		}
               
		private int getColonistCount()
		{
			if (colonistCount >= 0) {
				return colonistCount;
			}

			colonistCount = 0;
			using (IEnumerator<Pawn> enumerator = AllColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
					colonistCount++;
			}
			Log.Message("Colonist count " + colonistCount);
			return colonistCount;
		}

		public override float getProgressFloat()
		{
			return (float)getColonistCount() / def.targetValue;
		}
        
		public override bool CanPick()
		{
			return getColonistCount() < def.targetValue;
		}

	}
}
