using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface IObjectContext
    {
        /// <summary>
        ///   Gets the type the object represented by this instance.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets any attributes attached to the object represented by this instance.
        /// </summary>
        IEnumerable<object> Attributes { get; }

        /// <summary>
        ///   Gets the members represented by this instance.
        /// </summary>
        IEnumerable<IMemberContext> Members { get; }

        /// <summary>
        ///   Gets the actual object represented by this context.
        /// </summary>
        /// <remarks>
        ///   No guarantees are given that this method will not throw an exception. This can happen if the 
        ///   deserialization is incomplete - i.e. not enough of the members has been given a value.
        /// </remarks>
        object GetObject();
    }
}