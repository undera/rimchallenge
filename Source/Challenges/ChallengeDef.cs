using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Challenges
{
	public class ChallengeDef : Def
	{
		public Type workerClass = typeof(ChallengeWorker);

		[MustTranslate]
		public string messageComplete = "You have completed the challenge! Get your well-deserved reward!";

		private List<ThingCountClass> rewardDef = new List<ThingCountClass>(0);
		private List<Thing> reward = new List<Thing>(0);

		internal int param1 = 0;
		internal int targetValue = 0;

		public List<ChallengeDef> prerequisites = new List<ChallengeDef>(0);

		private float x = 1f;

		private float y = 1f;

		private ChallengeWorker _checker;
		internal string applicability = "";


		public ChallengeWorker checkerInstance
		{
			get
			{
				if (_checker == null)
				{
					_checker = (ChallengeWorker)Activator.CreateInstance(this.workerClass, this);
				}
				return _checker;
			}
		}

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
			get; set; // TODO remember and save
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

				return checkerInstance.CanPick();
			}
		}

		internal string RewardsText()
		{
			string stringBuilder = "";
			foreach (Thing current in GetReward())
			{
				string stringLabel = GenLabel.ThingLabel(current.def, null, current.stackCount).CapitalizeFirst();
				stringBuilder += ("   -" + stringLabel + "\n");
			}
			return stringBuilder;
		}

		public IEnumerable<Thing> GetReward()
		{
			if (reward.NullOrEmpty())
			{
				if (rewardDef.NullOrEmpty())
				{
					ThingSetMaker gen = ThingSetMakerDefOf.Reward_ItemStashQuestContents.root;
					ThingSetMakerParams conf = default(ThingSetMakerParams);
					conf.validator = ((ThingDef td) => td != ThingDefOf.Silver);
					reward = gen.Generate(conf);
				}
				else
				{
					foreach (ThingCountClass t in rewardDef)
					{
						Thing thing = ThingMaker.MakeThing(t.thing.def, t.thing.Stuff);
						thing.stackCount = t.Count;
						reward.Add(thing);
					}
				}
			}
			return reward;
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
