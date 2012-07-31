using System.Linq;
using System.Xml.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerRecursiveDeserializer : RecursiveDeserializer
    {
        public XmlSerializerRecursiveDeserializer(IMapper mapper, IDeserializerSelector recursiveSelector, IObjectContextFactory objectContextFactory, ITypeConverter typeConverter)
            : base(mapper, recursiveSelector, objectContextFactory, typeConverter) { }

        public override void Deserialize(XObject source, IMemberContext target, IAdviceRequester adviceRequester)
        {
            base.Deserialize(source, target, adviceRequester);

            IObjectContext targetValue = target.GetValue();
            if (targetValue == null)
                return;

            foreach (var member in targetValue.Members)
            {
                IObjectContext memberValue = member.GetValue();
                if (memberValue == null)
                    continue;

                foreach (var childMember in memberValue.Members)
                {
                    if (!childMember.Attributes.OfType<XmlParentAttribute>().Any())
                        continue;

                    childMember.SetValue(targetValue);
                    childMember.CommitChanges();
                }
            }
        }
    }
}