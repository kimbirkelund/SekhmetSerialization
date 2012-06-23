using Sekhmet.Serialization.TestUtility;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public abstract class XmlSerializerSerializationTestCaseBase : SerializationTestCaseBase
    {
        protected override ISerializationManagerFactory CreateSerializationManager()
        {
            return new XmlSerializerSerializationManagerFactory();
        }
    }
}