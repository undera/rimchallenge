using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Challenges
{
	public class Challenge_MineOutN : ChallengeWorker
	{
		public Challenge_MineOutN(ChallengeDef def, Pawn giver) : base(def, giver)
		{
		}

		public override void OnDestroyMined(Mineable block)
		{
			progress++;
            if (progress >= def.targetValue)
            {
                Complete();
            }
		}

        public override bool CanPick()
        {
            return Find.AnyPlayerHomeMap.TileInfo.hilliness >= Hilliness.LargeHills;
        }
	}
}
