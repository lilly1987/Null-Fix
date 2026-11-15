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
        private static MethodBase TargetMethod()
        {
            // 오류 로그에 표시된 제네릭 오버로드:
            // GenTypes.SameOrSubclassOf<T>(Type baseType)
            MethodInfo[] methods = typeof(GenTypes).GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance);

            foreach (MethodInfo method in methods)
            {
                if (method.Name != nameof(GenTypes.SameOrSubclassOf))
                    continue;

                if (!method.IsGenericMethodDefinition)
                    continue;

                if (method.GetGenericArguments().Length != 1)
                    continue;

                ParameterInfo[] parameters = method.GetParameters();

                if (parameters.Length == 1 &&
                    parameters[0].ParameterType == typeof(Type))
                {
                    return method;
                }
            }

            return null;
        }

        private static bool Prefix(Type baseType, ref bool __result)
        {
            // 정상적인 Type이면 원본 메서드를 실행한다.
            if (baseType != null)
                return true;

            // Null Type은 일치하는 하위 타입이 없는 것으로 처리한다.
            __result = false;
            return false;
        }
    }
}
