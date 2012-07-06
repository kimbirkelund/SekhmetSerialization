using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface ISerializationManager
    {
        /// <summary>
        ///   Adds the specified advisor to the list of advisors for the specified types.
        /// </summary>
        void AddAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types);

        /// <summary>
        ///   Deserializes the the specified source to an object of the specified target type.
        /// </summary>
        object Deserialize(XElement source, Type targetType);

        /// <summary>
        /// Removes the specified advisor from getting asked about the specified types.
        /// </summary>
        void RemoveAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types);

        /// <summary>
        ///   Serialiazes the specified source to an <see cref="XElement" />.
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        XElement Serialize(object source);
    }
}