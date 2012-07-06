using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified source into the target. Returning <c>true</c> if target should be added to structure, or <c>false</c> if it already is added (or just shouldn't be).
        /// </summary>
        bool Serialize(IMemberContext source, XObject target, IAdviceRequester adviceRequester);
    }
}