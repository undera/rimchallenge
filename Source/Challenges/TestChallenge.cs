using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Verse
{
	public class TestChallenge : ChallengeWorker
	{
        public TestChallenge(ChallengeDef def) : base(def)
		{
		}

		public override void OnPawnKilled(Pawn pawn)
		{
			int result = 0;
			using (IEnumerator<Pawn> enumerator = AllColonists.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    result++;
            }
			Log.Message("Size "+result);
			if (result == 2) {
				Complete();
			}
		}

		public override void OnPawnFactionSet(Pawn pawn)
        {
			Log.Message("Pawn "+pawn+" gets faction "+pawn.Faction);
			OnPawnKilled(pawn);
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
