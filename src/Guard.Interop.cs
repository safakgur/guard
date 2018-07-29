namespace Dawn
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <content>Provides utilities to support legacy frameworks.</content>
    public static partial class Guard
    {
        /// <summary>Determines whether the specified type is a value type.</summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///     <c>true</c>, if <paramref name="type" /> represents
        ///     a value type; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValueType(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        /// <summary>Determines whether the specified type is a generic type.</summary>
        /// <param name="type">The type to check.</param>
        /// <param name="definition">The type definition.</param>
        /// <returns>
        ///     <c>true</c>, if <paramref name="type" /> represents a generic
        ///     type with the specified definition; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsGenericType(this Type type, Type definition)
        {
#if NETSTANDARD1_0
            var info = type.GetTypeInfo();
            return type.GetTypeInfo().IsGenericType
                && info.GetGenericTypeDefinition() == definition;
#else
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == definition;
#endif
        }

        /// <summary>Determines whether the specified type is an enum.</summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///     <c>true</c>, if <paramref name="type" /> represents
        ///     an enumeration; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEnum(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        /// <summary>
        ///     Determines whether a type is derived from the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseType">The base type.</param>
        /// <returns>
        ///     <c>true</c>, if <paramref name="type" /> is derived from
        ///     <paramref name="baseType" /> otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSubclassOf(this Type type, Type baseType)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsSubclassOf(baseType);
#else
            return type.IsSubclassOf(baseType);
#endif
        }

        /// <summary>Returns the type from wich the specified type directly inherits.</summary>
        /// <param name="type">The type whose base type that will be returned.</param>
        /// <returns>
        ///     The type from wich the <paramref name="type" /> directly inherits,
        ///     if there is one; otherwise, <c>null</c>.
        /// </returns>
        private static Type GetBaseType(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        /// <summary>Returns the constructor with the specified parameters.</summary>
        /// <param name="type">The type that the constructor belongs to.</param>
        /// <param name="arguments">The types of the constructor parameters.</param>
        /// <returns>
        ///     The constructor with the specified parameters if it can be
        ///     found in <paramref name="type" />; otherwise, <c>null</c>.
        /// </returns>
        private static ConstructorInfo GetConstructor(this Type type, params Type[] arguments)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo()
                .DeclaredConstructors.FirstOrDefault(c => c
                    .GetParameters()
                    .Select(p => p.ParameterType)
                    .SequenceEqual(arguments));
#else
            return type.GetConstructor(arguments);
#endif
        }

        /// <summary>Returns the getter of the property with the specified name.</summary>
        /// <param name="type">The type that the property belongs to.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>
        ///     The getter of the property with the specified name, if it can be
        ///     found in <paramref name="type" />; otherwise, <c>null</c>.
        /// </returns>
        private static MethodInfo GetPropertyGetter(this Type type, string name)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().GetDeclaredProperty(name)?.GetMethod;
#else
            return type.GetProperty(name)?.GetGetMethod();
#endif
        }

        /// <summary>Returns the method with the specified signature.</summary>
        /// <param name="type">The type that the method belongs to.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="arguments">The types of the method arguments.</param>
        /// <returns>
        ///     The method with the specified signature if it can be found
        ///     in <paramref name="type" />; otherwise, <c>null</c>.
        /// </returns>
        private static MethodInfo GetMethod(this Type type, string name, Type[] arguments)
        {
#if NETSTANDARD1_0
            return type.GetRuntimeMethod(name, arguments);
#else
            return type.GetMethod(name, arguments);
#endif
        }

        /// <summary>
        ///     Determines whether the specified string is empty
        ///     or consists only of white-space characters.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>
        ///     <c>true</c>, if <paramref name="s" /> is empty
        ///     or consists only of white-space characters.
        /// </returns>
        private static bool IsWhiteSpace(string s)
        {
#if NET35
            for (var i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s, i))
                    return false;
            }

            return true;
#else
            return string.IsNullOrWhiteSpace(s);
#endif
        }
    }
}

#if NETSTANDARD1_0
namespace System.Runtime.InteropServices
{
    /// <summary>Required to use "in" parameters on .NET Standard 1.0.</summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class InAttribute : Attribute
    {
    }
}
#endif
