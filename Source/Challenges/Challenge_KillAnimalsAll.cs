using System.Linq;

namespace Verse
{
	public class Challenge_KillAnimalsAll : Challenge_KillAnimals
	{
		private int totalAnimals = 0;
		public Challenge_KillAnimalsAll(ChallengeDef def) : base(def)
		{
			totalAnimals = animalCount;
		}

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
		}

		public override bool CanPick()
		{
			if (Find.AnyPlayerHomeMap.Biome.animalDensity < 1.5f)
			{
				return false;
			}
			return base.CanPick();
		}
	}
}
