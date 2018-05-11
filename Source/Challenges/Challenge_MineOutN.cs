using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public class Challenge_MineOutN : ChallengeWorker
	{
		public Challenge_MineOutN(ChallengeDef def) : base(def)
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
