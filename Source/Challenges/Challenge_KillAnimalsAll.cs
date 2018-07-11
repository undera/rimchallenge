using System.Linq;
using RimWorld.Planet;
using Verse;

namespace Challenges
{
	public class Challenge_KillAnimalsAll : Challenge_KillAnimals
	{
		private int totalAnimals = 0;
		public Challenge_KillAnimalsAll(ChallengeDef def, Pawn giver) : base(def, giver)
		{
		}

        public override void Started()
		{
			totalAnimals = animalCount;
		}

		public static int animalCount { get { return Find.AnyPlayerHomeMap.mapPawns.AllPawns.Count((Pawn x) => x.AnimalOrWildMan()); } }

		public override float getProgressFloat()
		{
			if (animalCount>totalAnimals)
			{
				totalAnimals=animalCount;
			}
			return (float)(totalAnimals - animalCount) / totalAnimals;
		}

		public override void OnPawnKilled(Pawn pawn, DamageInfo dinfo)
		{
			Log.Message("Animals: " + animalCount);
			if (animalCount <= 0)
			{
				Complete();
			}
			else {
				totalAnimals = animalCount;
				hint = "Look, there is " + Find.AnyPlayerHomeMap.mapPawns.AllPawns.Where((Pawn x) => x.AnimalOrWildMan()).RandomElement().Label+" still alive";
			}
		}

		public override bool CanPick()
		{
			Tile tile = Find.World.grid[Find.GameInitData.startingTile];
			if (tile.biome.animalDensity < 1.5f)
			{
				return false;
			}
			return base.CanPick();
		}

	}
}
