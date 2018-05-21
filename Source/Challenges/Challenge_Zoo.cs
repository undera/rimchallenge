using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Challenges
{
	public class Challenge_Zoo : ChallengeWorker
	{
		public static HashSet<PawnKindDef> allZooableAnimals = new HashSet<PawnKindDef>();

		public Challenge_Zoo(ChallengeDef def, Pawn giver) : base(def, giver)
		{
			if (allZooableAnimals.Count() == 0)
			{
				int cnt = 0;
				foreach (PawnKindDef pkdef in DefDatabase<PawnKindDef>.AllDefs.Where(x => x.RaceProps.Animal))
				{
					allZooableAnimals.Add(pkdef);
					Log.Message(pkdef + " " + pkdef.RaceProps.wildness);
					cnt++;
				}
				Log.Message("Total animal kind count: " + allZooableAnimals.Count());
			}
		}

		private void Check()
        {
			HashSet<PawnKindDef> found = new HashSet<PawnKindDef>();
			foreach (Pawn pawn in Find.AnyPlayerHomeMap.mapPawns.AllPawns.Where((Pawn x) => x.RaceProps.Animal && x.Faction == Faction.OfPlayer)) {
				if (allZooableAnimals.Contains(pawn.kindDef)) {
					found.Add(pawn.kindDef);
				}
			}
			progress = found.Count();
			if (progress >= def.targetValue)
			{
				Complete();
			}
			else { 
				hint="You heard that there is also "+allZooableAnimals.Where(x=>!found.Contains(x)).RandomElement().label+" on this planet...";
			}
        }

        public override void Started()
		{
			def.targetValue = allZooableAnimals.Count() / def.param1;
		}

        public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			if (allZooableAnimals.Contains(pawn.kindDef)) {
				Check();
			}
		}

		public override void OnPawnFactionSet(Pawn pawn)
		{
			if (allZooableAnimals.Contains(pawn.kindDef)) {
                Check();
            }
		}
	}
}
