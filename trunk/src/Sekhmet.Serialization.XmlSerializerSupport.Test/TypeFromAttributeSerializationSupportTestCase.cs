using System.Collections.Generic;
using System.Xml.Linq;
using Sekhmet.Serialization.XmlSerializerSupport.Test.Dummies;

namespace Sekhmet.Serialization.XmlSerializerSupport.Test
{
    public class TypeFromAttributeSerializationSupportTestCase : XmlSerializerSerializationTestCaseBase
    {
        private const string ExpectedBar1Value1 = "Foo";
        private const int ExpectedBar2Value2 = 24;

        protected override object CreateObject()
        {
            return new FooWithBarBaseWithOutAttributes
            {
                Bar = new Bar2
                {
                    Value2 = ExpectedBar2Value2.ToString()
                },
                Bars = new List<IBar> {
                                                                                     new Bar2 {Value2 = ExpectedBar2Value2.ToString()},
                                                                                     new Bar1 {Value1 = ExpectedBar1Value1}
                                                                                 }
            };
        }

        protected override ISerializationManagerFactory CreateSerializationManager()
        {
            var manager = base.CreateSerializationManager();
            manager.TypeConverter.AddConverter(new ActualTypeFromAttributeTypeConverter());

            return manager;
        }

        protected override XElement CreateXml()
        {
            return new XElement("Foo",
                                Constants.XmlSchemaInstanceNamespaceAttribute,
                                new XElement("Bar",
                                             new XAttribute("type", typeof(Bar2).AssemblyQualifiedName),
                                             new XElement("Value2", ExpectedBar2Value2)),
                                new XElement("Bars",
                                             new XElement("Bar2",
                                                          new XAttribute("type", typeof(Bar2).AssemblyQualifiedName),
                                                          new XElement("Value2", ExpectedBar2Value2)),
                                             new XElement("Bar1",
                                                          new XAttribute("type", typeof(Bar1).AssemblyQualifiedName),
                                                          new XElement("Value1", ExpectedBar1Value1))));
        }
    }
}