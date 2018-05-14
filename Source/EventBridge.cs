﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Rimchallenge
{
	public static class EventBridge
    {
		private static HarmonyInstance harmony = HarmonyInstance.Create("rimworld.undera4.challenge");

		private static void patch(Type type, string srcName, string preName, string postName) {
			MethodBase srcMethod = AccessTools.Method(type, srcName);
			HarmonyMethod preMethod = preName != null ?new HarmonyMethod(typeof(EventBridge).GetMethod(preName)):null;
			HarmonyMethod postMethod = postName != null ? new HarmonyMethod(typeof(EventBridge).GetMethod(postName)) : null;           
			harmony.Patch(srcMethod, preMethod, postMethod);
		}

        public static void Hook(Controller modL)
        {
			patch(typeof(Map), nameof(Map.FinalizeInit), null, nameof(OnMapLoaded));

			patch(typeof(Pawn), nameof(Pawn.Kill), null, nameof(OnPawnKilled));
			patch(typeof(Pawn), nameof(Pawn.Destroy), null, nameof(OnPawnDestroyed));
			patch(typeof(Pawn), nameof(Pawn.SetFaction), null, nameof(OnPawnFactionSet)); 
			// TODO when escape pod colonist joins, nothing happens

			patch(typeof(Mineable), nameof(Mineable.DestroyMined), null, nameof(OnDestroyMined));
			patch(typeof(Mineable), nameof(Mineable.Destroy), null, nameof(OnDestroyMined));

			//patch(typeof(SkillRecord), nameof(SkillRecord.Learn), null, nameof(OnSkillLearned));

			patch(typeof(IncidentWorker_NeutralGroup), "SpawnPawns", null, nameof(OnGroupSpawned));

			MethodInfo dst = AccessTools.Method(typeof(PawnRenderer), nameof(PawnRenderer.RenderPawnAt), new[] { typeof(Vector3), typeof(RotDrawMode), typeof(bool) });
			HarmonyMethod met = new HarmonyMethod(typeof(EventBridge), nameof(RenderPawnAt));
			harmony.Patch(dst, null, met);
		}

		public static void OnMapLoaded()
        {
			ChallengeDef.GenerateNonOverlappingCoordinates();
            // TODO load saved status
        }

		public static void OnPawnKilled(Pawn __instance, DamageInfo dinfo)
		{
			ChallengeManager.instance.currentChallenge.OnPawnKilled(__instance, dinfo);
		}

		public static void OnPawnFactionSet(Pawn __instance)
        {
			ChallengeManager.instance.currentChallenge.OnPawnFactionSet(__instance);
        }

		public static void OnPawnDestroyed(Pawn __instance)
        {
			if (ChallengeManager.instance.questOwner == __instance) {
				ChallengeManager.instance.SetQuestOwner(null);
			}

            ChallengeManager.instance.currentChallenge.OnPawnDestroyed(__instance);
        }

		public static void OnDestroyMined(Mineable __instance)
        {
			ChallengeManager.instance.currentChallenge.OnDestroyMined(__instance);
        }

		public static void SkillLearned(SkillRecord __instance)
        {
            Log.Message("Skill learned " + __instance);
            ChallengeManager.instance.currentChallenge.OnSkillLearned(__instance);
        }

		public static void OnGroupSpawned(IncidentWorker_NeutralGroup __instance, List<Pawn> __result) {
			Log.Message("Spawned group by "+__instance);
			foreach (Pawn p in __result)
            {
                Log.Message(p + " can trade " + p.CanTradeNow);
            }
			ChallengeManager.instance.SetQuestOwner(__result.RandomElement());
		}

		public static void RenderPawnAt(PawnRenderer __instance, Vector3 drawLoc, RotDrawMode bodyDrawType, bool headStump)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
			if (ChallengeManager.instance.questOwner==pawn)
			{
				ChallengeManager.RenderAsteriskOverlay(pawn);
			}
        }
    }
}