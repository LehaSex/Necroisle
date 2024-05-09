using System;
using System.Reflection;

namespace Necroisle
{
    // Определение пользовательского атрибута
    [AttributeUsage(AttributeTargets.Method)]
    public class CountableFunctionAttribute : Attribute { }
    
    // Класс для рефлексии и подсчета функций с атрибутом CountableFunction
    public class FunctionCounter
    {
        // Метод для подсчета общего количества функций с атрибутом CountableFunction
        public static int CountTotalFunctions(Type type)
        {
            int totalFunctions = 0;
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var method in methods)
            {
                if (Attribute.IsDefined(method, typeof(CountableFunctionAttribute)))
                {
                    totalFunctions++;
                }
            }
            return totalFunctions;
        }
    }

}
