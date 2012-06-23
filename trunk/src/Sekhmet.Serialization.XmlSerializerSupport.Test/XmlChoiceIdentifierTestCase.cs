using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class XmlChoiceIdentifierTestCase : XmlSerializerSerializationTestCaseBase
    {
        protected override object CreateObject()
        {
            return new Choices
            {
                MyChoice = "Book",
                EnumType = ItemChoiceType.Word,
                ManyChoices = new object[] { "Food", 5, 98.6 },
                ChoiceArray = new[] {
                                                           MoreChoices.Item,
                                                           MoreChoices.Amount,
                                                           MoreChoices.Temp
                                                       }
            };
        }

        protected override XElement CreateXml()
        {
            return
                XElement.Parse(
                    @"
<Choices xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Item>Food</Item>
  <Amount>5</Amount>
  <Temp>98.6</Temp>
  <Word>Book</Word>
</Choices>");
        }
    }

    public class Choices
    {
        // The MyChoice field can be set to any one of 
        // the types below. 

        // Don't serialize this field. The EnumType field
        // contains the enumeration value that corresponds
        // to the MyChoice field value.
        [XmlIgnore]
        public MoreChoices[] ChoiceArray { get; set; }

        [XmlIgnore]
        public ItemChoiceType EnumType { get; set; }

        // The ManyChoices field can contain an array
        // of choices. Each choice must be matched to
        // an array item in the ChoiceArray field.
        [XmlChoiceIdentifier("ChoiceArray")]
        [XmlElement("Item", typeof(string))]
        [XmlElement("Amount", typeof(int))]
        [XmlElement("Temp", typeof(double))]
        public object[] ManyChoices { get; set; }

        [XmlChoiceIdentifier("EnumType")]
        [XmlElement("Word", typeof(string))]
        [XmlElement("Number", typeof(int))]
        [XmlElement("DecimalNumber", typeof(double))]
        public object MyChoice { get; set; }

        // TheChoiceArray field contains the enumeration
        // values, one for each item in the ManyChoices array.

        public override string ToString()
        {
            return new { ChoiceArray, EnumType, ManyChoices, MyChoice }.ToString();
        }
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemChoiceType
    {
        None,
        Word,
        Number,
        DecimalNumber
    }

    public enum MoreChoices
    {
        None,
        Item,
        Amount,
        Temp
    }
}