using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlChoiceIdentifier
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
}