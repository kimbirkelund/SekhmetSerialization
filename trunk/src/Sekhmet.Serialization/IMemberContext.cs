using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface IMemberContext
    {
        /// <summary>
        /// Gets any attributes attached to the member represented by this instance.
        /// </summary>
        IEnumerable<object> Attributes { get; }

        /// <summary>
        ///   Gets the type of the value of the member by this instance.
        /// </summary>
        Type ContractType { get; }

        /// <summary>
        ///   Gets the name of the member by this instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// If a new value has been set on this instance it is written back to the member it represents.
        /// </summary>
        void CommitChanges();

        /// <summary>
        ///   Gets the current value of the member represents by this instance.
        /// </summary>
        IObjectContext GetValue();

        /// <summary>
        ///   Sets the value of the member represented by this instance.
        /// </summary>
        void SetValue(IObjectContext value);
    }
}