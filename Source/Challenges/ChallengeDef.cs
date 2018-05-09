using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ChallengeDef:Def
	{
		public Type workerClass=typeof(ChallengeWorker);

        [MustTranslate]
		public string messageComplete = "Challenge Complete! Get the prize!";

		public List<ThingCountClass> gratificationThings=new List<ThingCountClass>(0);

		internal int targetValue=0;

		public List<ChallengeDef> prerequisites=new List<ChallengeDef>(0);

		internal float TabViewX=0;
        internal float TabViewY=0;

		private float x = 1f;

        private float y = 1f;

		private ChallengeWorker checker;

		public float TabViewFlexX
        {
            get
            {
                return this.x;
            }
        }

		public float TabViewFlexY
        {
            get
            {
                return this.y;
            }
        }

		public bool IsFinished
        {
            get
            {
				return false; // TODO remember and save
            }
        }

        public bool CanStartNow
        {
            get
            {
				if (IsFinished)
				{
					return false;
				}

				for (int m = 0; m < this.prerequisites.CountAllowNull<ChallengeDef>(); m++)
                {
					ChallengeDef required = this.prerequisites[m];
					if (required != null && !required.IsFinished)
                    {
						return false;
                    }
                }

				if (checker == null) { 
					checker = (ChallengeWorker)Activator.CreateInstance(this.workerClass, this);
				}
				if (!checker.CanPick()) {
					return false;
				}

				return true;
            }
        }

		public static void GenerateNonOverlappingCoordinates()
        {
			Log.Message("GenerateNonOverlappingCoordinates");
            foreach (ChallengeDef current in DefDatabase<ChallengeDef>.AllDefsListForReading)
            {
				current.x = current.TabViewX;
				current.y = current.TabViewY;
            }
            int num = 0;
            while (true)
            {
                bool flag = false;
                foreach (ChallengeDef current2 in DefDatabase<ChallengeDef>.AllDefsListForReading)
                {
                    foreach (ChallengeDef current3 in DefDatabase<ChallengeDef>.AllDefsListForReading)
                    {
                        if (current2 != current3)
                        {
                            bool flag2 = Mathf.Abs(current2.x - current3.x) < 0.5f;
                            bool flag3 = Mathf.Abs(current2.y - current3.y) < 0.25f;
                            if (flag2 && flag3)
                            {
                                flag = true;
                                if (current2.x <= current3.x)
                                {
                                    current2.x -= 0.1f;
                                    current3.x += 0.1f;
                                }
                                else
                                {
                                    current2.x += 0.1f;
                                    current3.x -= 0.1f;
                                }
                                if (current2.y <= current3.y)
                                {
                                    current2.y -= 0.1f;
                                    current3.y += 0.1f;
                                }
                                else
                                {
                                    current2.y += 0.1f;
                                    current3.y -= 0.1f;
                                }
                                current2.x += 0.001f;
                                current2.y += 0.001f;
                                current3.x -= 0.001f;
                                current3.y -= 0.001f;
                                ChallengeDef.ClampInCoordinateLimits(current2);
                                ChallengeDef.ClampInCoordinateLimits(current3);
                            }
                        }
                    }
                }
                if (!flag)
                {
                    break;
                }
                num++;
                if (num > 200)
                {
                    goto Block_4;
                }
            }
            return;
        Block_4:
            Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.");
        }

		private static void ClampInCoordinateLimits(ChallengeDef rp)
        {
            if (rp.x < 0f)
            {
                rp.x = 0f;
            }
            if (rp.y < 0f)
            {
                rp.y = 0f;
            }
            if (rp.y > 6.5f)
            {
                rp.y = 6.5f;
            }
        }
	}
}
