
using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace Lilly.NullFix
{
    /// <summary>
    /// Null Type이 전달된 경우 false를 반환하여
    /// ReadingPolicy 초기화 중 NullReferenceException을 방지한다.
    /// </summary>
    [HarmonyPatch]
    internal static class ReadingPolicyNullPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(
                typeof(GenTypes),
                nameof(GenTypes.SameOrSubclassOf),
                new Type[]
                {
                    typeof(Type),
                    typeof(Type)
                }
            );
        }

        static bool Prefix(
            Type baseType,
            Type parentType,
            ref bool __result)
        {
            // 정상적인 타입이면 원래 메서드 실행
            if (baseType != null && parentType != null)
                return true;

            // Null 타입이면 상속 관계가 성립하지 않는 것으로 처리
            __result = false;
            return false;
        }
    }
}
