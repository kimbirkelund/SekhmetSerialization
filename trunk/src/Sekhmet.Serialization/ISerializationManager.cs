using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface ISerializationManager
    {
        /// <summary>
        /// Deserializes the the specified source to an object of the specified target type.
        /// </summary>
        object Deserialize(XElement source, Type targetType);

        /// <summary>
        /// Serialiazes the specified source to an <see cref="XElement"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        XElement Serialize(object source);
    }
}