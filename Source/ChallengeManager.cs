using System;
using Verse;
using System.Linq;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Rimchallenge
{
	public class ChallengeManager : GameComponent
	{
		public ChallengeWorker currentChallenge { get; private set; } = new ChallengeWorkerNone();

		public ChallengeDef currentChallengeDef;
		public int progress = 0;
		public static ChallengeManager instance { get; private set; }

		public Pawn questOwner { get; private set; }
		public ChallengeDef questOwnerChallenge { get; private set; }

		public ChallengeManager(Game game)
		{
			//Log.Message("Constructed ChallengeManager");
			instance = this;
			foreach (ChallengeDef def in DefDatabase<ChallengeDef>.AllDefs)
			{
				Controller.checkFinished(def);
			}
		}

		internal ChallengeDef GetOfferedChallenge()
		{
			IEnumerable<ChallengeDef> possible = DefDatabase<ChallengeDef>.AllDefs.Where(x => x.CanStartNow && x != currentChallengeDef).ToList();
			foreach (ChallengeDef def in possible)
			{
				Log.Message("Possible: " + def);
			}
			return possible.RandomElementWithFallback(null);
		}

		public override void StartedNewGame()
		{
			Log.Message("Started new Game");
			ClearChallenge();
			progress = 0;
		}

		public override void LoadedGame()
		{
			Log.Message("Loaded game: " + currentChallengeDef + " with " + progress);
			StartChallenge(currentChallengeDef);
			currentChallenge.progress = progress;
		}

		public override void ExposeData()
		{
			Log.Message("Expose data called for ChallengeManager");

			Scribe_Defs.Look<ChallengeDef>(ref this.currentChallengeDef, "currentChallenge");
			Scribe_Values.Look<int>(ref this.progress, "progress");
		}

		public bool HasChallenge()
		{
			return currentChallenge != null && !(currentChallenge is ChallengeWorkerNone);
		}

		public void StartChallenge(ChallengeDef challengeDef)
		{
			if (challengeDef != null)
			{
				Log.Message("Picked a challenge: " + challengeDef);
				currentChallengeDef = challengeDef;
				currentChallenge = (ChallengeWorker)Activator.CreateInstance(challengeDef.workerClass, challengeDef);
				Log.Message("Current progress: " + currentChallenge.getProgressFloat());
			}
		}

		public void ClearChallenge()
		{
			currentChallenge = new ChallengeWorkerNone();
			currentChallengeDef = null;
			progress = 0;
		}

		internal void SetQuestOwner(Pawn pawn)
		{
			questOwner = pawn;
			if (pawn == null)
			{
				questOwnerChallenge = null;
			}
			else
			{
				ChallengeDef offer = GetOfferedChallenge();
				questOwnerChallenge = offer;
				if (offer == null)
				{
					questOwner = null;
				}
			}
		}

		private static Material asterisk;

		public static void RenderAsteriskOverlay(Pawn t)
		{
			if (!t.Spawned) return;
			var drawPos = t.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays) + 0.28125f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}

			if (asterisk == null)
			{
				asterisk = MaterialPool.MatFrom("UI/Overlays/asterisk", ShaderDatabase.MetaOverlay);
			}

			RenderPulsing(t, asterisk, drawPos, MeshPool.plane05);
		}

		private static void RenderPulsing(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			var num = (Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f;
			var num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
			num2 = 0.3f + num2 * 0.7f;
			var material = FadedMaterialPool.FadedVersionOf(mat, num2);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}
	}
}
