﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	public class Challenge_MineOutResources : Challenge_MineOutN
	{
		private int totalTiles = 0;
		private int count = -1;
		public Challenge_MineOutResources(ChallengeDef def) : base(def)
		{
			totalTiles = targetTiles;
		}

		public override void OnDestroyMined(Mineable block)
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

		public int targetTiles
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

		public static int TileCount(bool onlyResources = false)
		{
			int count = 0;
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
						count++;
					}
				}
			}
			Log.Message("Resource count: " + count);
			return count;
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
			//Log.Message("Mineable " + mthing);
			return true;
		}
	}
}