using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Challenges
{
	public class Challenge_MineOutResources : Challenge_MineOutN
	{
		protected int totalTiles = 0;
		protected int count = -1;
		public Challenge_MineOutResources(ChallengeDef def) : base(def)
		{
		}

        public override void Started()
		{
			count = -1;
			totalTiles = TileCount(true);
		}

		public override void OnDestroyMined(Mineable block, Pawn actor)
		{
			count = -1;
			if (targetTiles <= 0)
			{
				Complete();
			}
		}

		public override float getProgressFloat()
		{
			if (targetTiles > totalTiles)
			{
				totalTiles = targetTiles;
			}
			return (float)(totalTiles - targetTiles) / totalTiles;
		}

		protected virtual int targetTiles
		{
			get
			{
				if (count < 0)
				{
					count = TileCount(true);
				}
				return count;
			}
		}

		public int TileCount(bool onlyResources = false)
		{
			int cnt = 0;
			Map map = Find.AnyPlayerHomeMap;
			foreach (IntVec3 current in map.AllCells)
			{
				List<Thing> thingList = current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Mineable mineable = thingList[i] as Mineable;
					if (mineable != null)
					{
						if (onlyResources && !isResource(mineable.def.building.mineableThing))
						{
							continue;
						}
						cnt++;
						hint = "There is still some "+mineable.def.building.mineableThing.label+" to mine";
					}
				}
			}
			Log.Message("Tile count: " + cnt);
			return cnt;
		}

		private static bool isResource(ThingDef mthing)
		{
			foreach (ThingCategoryDef tcDef in mthing.thingCategories)
			{
				if (ThingCategoryDefOf.Chunks.childCategories.Contains(tcDef))
				{
					return false;
				}
			}
			return true;
		}
	}
}
