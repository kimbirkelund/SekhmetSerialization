using System;

namespace Sekhmet.Serialization
{
    public interface IObjectContextFactory
    {
        /// <summary>
        /// Creates an object context factory representing a new instance of the specified type for use when deserializing 
        /// or <c>null</c> if this factory cannot handle the specified type.
        /// </summary>
        IObjectContext CreateForDeserialization(IMemberContext targetMember, Type targetType, IAdviceRequester adviceRequester);

        /// <summary>
        /// Creates an object context factory representing a new instance of the specified type for use when serializing
        /// or <c>null</c> if this factory cannot handle the specified type.
        /// </summary>
        IObjectContext CreateForSerialization(IMemberContext sourceMember, object source, IAdviceRequester adviceRequester);
    }
}