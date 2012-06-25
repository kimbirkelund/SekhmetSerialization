using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sekhmet.Serialization.Utility
{
    /// <summary>
    ///   Extension methods for <see cref="Type" />.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        ///   Determines whether the specified type is an array or implements either <see cref="IEnumerable" /> or <see
        ///    cref="IEnumerable{T}" />.
        /// </summary>
        public static bool IsCollectionType(this Type type)
        {
            return type != typeof (string)
                   && (type.IsArray
                       || type.IsSubTypeOf<IEnumerable>()
                       || type.IsGenericType && type.GetGenericTypeDefinition().IsSubTypeOf(typeof (IEnumerable<>)));
        }

        /// <summary>
        ///   Determines whether <paramref name="type" /> is a nullable type.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        /// <summary>
        ///   Determines whether <c>type</c> is a sub-type of <c>T</c>.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="type"> The type. </param>
        /// <returns> <c>true</c> if <c>type</c> is a sub-type of <c>T</c> ; <c>false</c> otherwise. </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool IsSubTypeOf<T>(this Type type)
        {
            return type.IsSubTypeOf(typeof (T));
        }

        /// <summary>
        ///   Determines whether the <c>type</c> is a sub-type of <c>superType</c>.
        /// </summary>
        /// <param name="type"> The type. </param>
        /// <param name="superType"> The postulated super-type. </param>
        /// <returns> <c>true</c> if <c>type</c> is a sub-type of <c>superType</c> ; <c>false</c> otherwise. </returns>
        public static bool IsSubTypeOf(this Type type, Type superType)
        {
            return superType.IsAssignableFrom(type);
        }
    }
}