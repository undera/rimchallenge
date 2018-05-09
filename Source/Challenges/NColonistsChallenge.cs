using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Verse
{
	public class NColonistsChallenge : ChallengeWorker
	{
		public NColonistsChallenge(ChallengeDef def) : base(def)
		{
		}

		public override void OnPawnKilled(Pawn pawn)
		{
			if (getPawnCount() >= def.targetValue)
			{
				Complete();
			}
		}

		public override void OnPawnFactionSet(Pawn pawn)
		{
			Log.Message("Pawn " + pawn + " gets faction " + pawn.Faction);
			OnPawnKilled(pawn);
		}

		private int getPawnCount()
		{
			int result = 0;
			using (IEnumerator<Pawn> enumerator = AllColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
					result++;
			}
			Log.Message("Pawn count " + result);
			return result;
		}

		public override float getProgress()
		{
			return (float)getPawnCount() / def.targetValue;
		}
        
		public override bool CanPick()
		{
			return getPawnCount()<def.targetValue;
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
