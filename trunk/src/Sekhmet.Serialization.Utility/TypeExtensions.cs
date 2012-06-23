using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Sekhmet.Serialization.Utility
{
    /// <summary>
    ///   Extension methods for <see cref="Type" />.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly MethodInfo _miGetDefaultValue =
                typeof(TypeExtensions).GetMethod("GetDefaultValueForReflection", BindingFlags.Static | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        /// <summary>
        ///   Gets the default value of a type programatically. For statically known types, this is equivalent of doing <c>typeof(SomeType)</c>.
        /// </summary>
        public static object GetDefaultValue(this Type type)
        {
            return _miGetDefaultValue.MakeGenericMethod(type).Invoke(null, null);
        }

        /// <summary>
        ///   Gets the reflected property accessor.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TResult"> The type of the result. </typeparam>
        /// <param name="name"> The name. </param>
        /// <param name="bindTo"> The flags. </param>
        /// <returns> </returns>
        public static Func<T, TResult> GetPropertyAccessor<T, TResult>(string name,
                                                                       BindingFlags bindTo = BindingFlags.Instance | BindingFlags.Public)
        {
            return GetPropertyAccessor<T, TResult>(typeof(T), name, bindTo);
        }

        /// <summary>
        ///   Gets the reflected property accessor.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="TResult"> The type of the result. </typeparam>
        /// <param name="type"> The type. </param>
        /// <param name="name"> The name. </param>
        /// <param name="bindTo"> The flags. </param>
        /// <returns> </returns>
        public static Func<T, TResult> GetPropertyAccessor<T, TResult>(this Type type,
                                                                       string name,
                                                                       BindingFlags bindTo = BindingFlags.Instance | BindingFlags.Public)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Cannot be null or empty", "name");

            PropertyInfo prop = type.GetProperty(name, bindTo);
            if (prop == null)
            {
                throw new ArgumentException(
                        String.Format("Type {0} does not contain the property {1} {2}",
                                      type.FullName, name, bindTo));
            }

            Type parameterType = typeof(T);
            ParameterExpression p = Expression.Parameter(typeof(T), "instance");
            Expression cast = null;
            if (type != parameterType)
                cast = Expression.Convert(p, type);

            MemberExpression accessorExpression = Expression.Property(cast ?? p, prop);
            Expression<Func<T, TResult>> lambda = Expression.Lambda<Func<T, TResult>>(accessorExpression, p);
            return lambda.Compile();
        }

        /// <summary>
        ///   Determines whether the specified type is an array or implements either <see cref="IEnumerable" /> or <see
        ///    cref="IEnumerable{T}" />.
        /// </summary>
        public static bool IsCollectionType(this Type type)
        {
            return type != typeof(string)
                   && (type.IsArray
                       || type.IsSubTypeOf<IEnumerable>()
                       || type.IsGenericType && type.GetGenericTypeDefinition().IsSubTypeOf(typeof(IEnumerable<>)));
        }

        /// <summary>
        ///   Determines whether <paramref name="type" /> is a nullable type.
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
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
            return type.IsSubTypeOf(typeof(T));
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

        /// <summary>
        ///   Determines whether <paramref name="type" /> is a super type of <typeparamref name="T" />.
        /// </summary>
        public static bool IsSuperTypeOf<T>(this Type type)
        {
            return type.IsSuperTypeOf(typeof(T));
        }

        /// <summary>
        ///   Determines whether <paramref name="type" /> is a super type of <paramref name="subType" />.
        /// </summary>
        public static bool IsSuperTypeOf(this Type type, Type subType)
        {
            return type.IsAssignableFrom(subType);
        }

        // ReSharper disable UnusedMember.Local
        // Used for reflection
        private static T GetDefaultValueForReflection<T>()
        {
            return default(T);
        }

        // ReSharper restore UnusedMember.Local
    }
}