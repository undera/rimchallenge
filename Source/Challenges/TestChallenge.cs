using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Rimchallenge.Challenges
{
	public class TestChallenge : ChallengeBase
	{

		public override List<Thing> GetGratification()
		{
			List<Thing> ret = new List<Thing>();
			Thing silver = ThingMaker.MakeThing(ThingDefOf.Silver);
			silver.stackCount = 100;
			ret.Add(silver);
			return ret;
		}

		public override void Initialize()
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
