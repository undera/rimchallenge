using System;
using Verse;

namespace Challenges
{
	public class Challenge_MineOutAll : Challenge_MineOutResources
	{
		public Challenge_MineOutAll(ChallengeDef def, Pawn giver) : base(def, giver)
		{
			totalTiles = TileCount(true);
		}

		protected override int targetTiles
        {
            get
            {
                if (count < 0)
                {
					count = TileCount(false);
                }
                return count;
            }
        }
  
	}
}
