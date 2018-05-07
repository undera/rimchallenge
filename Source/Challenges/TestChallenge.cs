using System;
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
			ret.Add(MakeThing(ThingDefOf.MealSurvivalPack, 20));
			ret.Add(MakeThing(ThingDefOf.Medicine, 10));
			ret.Add(MakeThing(ThingDefOf.Component, 5));
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
			if (result == 4) {
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
