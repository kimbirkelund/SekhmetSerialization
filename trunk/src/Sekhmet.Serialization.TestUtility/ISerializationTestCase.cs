using System.Xml.Linq;

namespace Sekhmet.Serialization.TestUtility
{
    public interface ISerializationTestCase
    {
        object Object { get; }
        ISerializationManagerFactory SerializationManager { get; }
        XElement Xml { get; }

        void AssertCorrectObject(object actual);
        void AssertCorrectXml(XElement actual);
    }
}