using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlChoiceIdentifier
{
    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        None,
        Word,
        Number,
        DecimalNumber
    }
}