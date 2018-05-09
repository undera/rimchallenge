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

		public override void OnPawnKilled(Pawn pawn)
		{
			colonistCount = -1;
			if (getColonistCount() >= def.targetValue)
			{
				Complete();
			}
		}

		public override void OnPawnFactionSet(Pawn pawn)
		{
			OnPawnKilled(pawn);
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

		public override float getProgress()
		{
			return (float)getColonistCount() / def.targetValue;
		}
        
		public override bool CanPick()
		{
			return getColonistCount() < def.targetValue;
		}

		protected IEnumerable<Pawn> AllColonists
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
}
