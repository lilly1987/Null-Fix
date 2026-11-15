
using System;
using HarmonyLib;
using Verse;

namespace Lilly.NullFix
{
    /// <summary>
    /// SameOrSubclassOf 호출 시 Null Type으로 인한
    /// NullReferenceException을 방지한다.
    /// </summary>
    [HarmonyPatch(
        typeof(GenTypes),
        nameof(GenTypes.SameOrSubclassOf),
        new[] { typeof(Type), typeof(Type) }
    )]
    internal static class ReadingPolicyNullPatch
    {
        private static bool Prefix(
            Type baseType,
            Type parentType,
            ref bool __result)
        {
            // 비교 대상 중 하나라도 Null이면
            // 하위 타입 관계가 성립하지 않는 것으로 처리한다.
            if (baseType == null || parentType == null)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
