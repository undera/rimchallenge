using System;
namespace Verse
{
	public class Challenge_MineOutAll : Challenge_MineOutResources
	{
		public Challenge_MineOutAll(ChallengeDef def) : base(def)
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
