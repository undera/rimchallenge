using System;
using Verse;

namespace Challenges
{
	public class Challenge_MineOutAll : Challenge_MineOutResources
	{
		public Challenge_MineOutAll(ChallengeDef def, Pawn giver) : base(def, giver)
		{
		}

        public override void Started()
		{
			count = -1;
			totalTiles = TileCount(false);
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
