using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Challenges
{
	public class Challenge_MineOutN : ChallengeWorker
	{
		public Challenge_MineOutN(ChallengeDef def) : base(def)
		{
		}

		public override void OnDestroyMined(Mineable block, Pawn actor)
		{
			if (actor!=null && actor.Faction.IsPlayer)
			{
				progress++;
			}

			if (progress >= def.targetValue)
            {
                Complete();
            }
		}

        public override bool CanPick()
        {
			Tile tile = Find.World.grid[Find.GameInitData.startingTile];
            return tile.hilliness >= Hilliness.LargeHills;
        }
	}
}
