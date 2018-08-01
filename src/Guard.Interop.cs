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
            return info.IsGenericType && info.GetGenericTypeDefinition() == definition;
#else
            return type.IsGenericType && type.GetGenericTypeDefinition() == definition;
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

        /// <summary>Returns the type from wich the specified type directly inherits.</summary>
        /// <param name="type">The type whose base type to return.</param>
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

#if NETSTANDARD1_0
        private static Type[] GetGenericArguments(this Type type)
            => type.GetTypeInfo().GenericTypeArguments;

        private static bool IsSubclassOf(this Type type, Type baseType)
            => type.GetTypeInfo().IsSubclassOf(baseType);

        private static FieldInfo GetField(this Type type, string name)
            => type.GetTypeInfo().GetDeclaredField(name);

        private static ConstructorInfo GetConstructor(this Type type, Type[] arguments)
        {
            return type.GetTypeInfo().DeclaredConstructors
                .FirstOrDefault(c => c.IsPublic && c
                    .GetParameters()
                    .Select(p => p.ParameterType)
                    .SequenceEqual(arguments));
        }

        private static MethodInfo GetMethod(this Type type, string name, Type[] arguments)
            => type.GetRuntimeMethod(name, arguments);
#endif

        /// <summary>Provides a cached, empty array.</summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        private static class Array<T>
        {
            /// <summary>Gets an empty array.</summary>
#if NETSTANDARD1_0
            public static T[] Empty { get; } = new T[0];
#else
            public static T[] Empty => Array.Empty<T>();
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
