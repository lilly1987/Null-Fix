using System;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace Lilly.NullFix
{
    [StaticConstructorOnStartup]
    internal static class NullFixStartup
    {
        static NullFixStartup()
        {
            new Harmony("Lilly.NullFix")
                .PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Gravship), "AddThing", new[] { typeof(Thing), typeof(IntVec3) })]
    internal static class Gravship_AddThing_NullThing_Patch
    {
        [ThreadStatic]
        private static int addThingDepth;

        internal static bool IsInsideAddThing
        {
            get { return addThingDepth > 0; }
        }

        private static bool Prefix(Thing __0, out int __state)
        {
            __state = addThingDepth;
            addThingDepth++;
            return __0 != null && __0.def != null &&
                (!(__0 is Pawn pawn) || pawn.carryTracker != null);
        }

        private static Exception Finalizer(Exception __exception, int __state)
        {
            addThingDepth = __state;
            return __exception;
        }
    }

    [HarmonyPatch]
    internal static class Pawn_CarryTracker_TryDropCarriedThing_GravshipFix_Patch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(
                typeof(Pawn_CarryTracker),
                "TryDropCarriedThing",
                new[]
                {
                    typeof(IntVec3),
                    typeof(ThingPlaceMode),
                    typeof(Thing).MakeByRefType(),
                    typeof(Action<Thing, int>)
                });
        }

        private static void Postfix(ref bool __result, ref Thing __2)
        {
            if (Gravship_AddThing_NullThing_Patch.IsInsideAddThing &&
                __result && __2 == null)
                __result = false;
        }
    }
}
