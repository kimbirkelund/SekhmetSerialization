using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Sekhmet.Serialization.XmlSerializerSupport.Test.XmlChoiceIdentifier;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test.XmlSerializerConformanceTests
{
    [TestFixture]
    public class XmlChoiceIdentifierTest
    {
        [Test]
        public void DeserializeObject()
        {
            var ser = new XmlSerializer(typeof(Choices));

            var elem = XElement.Parse(@"
<Choices xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Item>Food</Item>
  <Amount>5</Amount>
  <Temp>98.6</Temp>
  <Word>Book</Word>
</Choices>");

            // A FileStream is needed to read the XML document.
            var myChoices = (Choices)ser.Deserialize(elem.CreateReader());

            // Disambiguate the MyChoice value using the enumeration.
            if (myChoices.EnumType == ItemChoiceType.Word)
            {
                Console.WriteLine("Word: " +
                                  myChoices.MyChoice);
            }
            else if (myChoices.EnumType == ItemChoiceType.Number)
            {
                Console.WriteLine("Number: " +
                                  myChoices.MyChoice);
            }
            else if (myChoices.EnumType == ItemChoiceType.DecimalNumber)
            {
                Console.WriteLine("DecimalNumber: " +
                                  myChoices.MyChoice);
            }

            // Disambiguate the ManyChoices values using the enumerations.
            for (int i = 0; i < myChoices.ManyChoices.Length; i++)
            {
                if (myChoices.ChoiceArray[i] == MoreChoices.Item)
                    Console.WriteLine("Item: " + (string)myChoices.ManyChoices[i]);
                else if (myChoices.ChoiceArray[i] == MoreChoices.Amount)
                    Console.WriteLine("Amount: " + myChoices.ManyChoices[i]);
                if (myChoices.ChoiceArray[i] == MoreChoices.Temp)
                    Console.WriteLine("Temp: " + myChoices.ManyChoices[i]);
            }
        }

        [Test]
        public void SerializeObject()
        {
            var mySerializer = new XmlSerializer(typeof(Choices));
            var stream = new MemoryStream();
            var myChoices = new Choices();

            // Set the MyChoice field to a string. Set the
            // EnumType to Word.
            myChoices.MyChoice = "Book";
            myChoices.EnumType = ItemChoiceType.Word;

            // Populate an object array with three items, one
            // of each enumeration type. Set the array to the 
            // ManyChoices field.
            var strChoices = new object[] { "Food", 5, 98.6 };
            myChoices.ManyChoices = strChoices;

            // For each item in the ManyChoices array, add an
            // enumeration value.
            var itmChoices = new[]{
                                          MoreChoices.Item,
                                          MoreChoices.Amount,
                                          MoreChoices.Temp
                                  };
            myChoices.ChoiceArray = itmChoices;

            mySerializer.Serialize(stream, myChoices);

            stream.Position = 0;

            var elem = XElement.Load(stream);

            Console.WriteLine(elem);
        }
    }



}