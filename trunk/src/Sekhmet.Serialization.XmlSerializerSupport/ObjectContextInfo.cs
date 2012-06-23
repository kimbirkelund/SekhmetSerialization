using System;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class ObjectContextInfo
    {
        public Type ActualType { get; private set; }

        public IEnumerable<object> Attributes { get; private set; }
        public IEnumerable<MemberContextInfo> Members { get; private set; }

        public ObjectContextInfo(Type actualType, IEnumerable<MemberContextInfo> members)
        {
            if (actualType == null)
                throw new ArgumentNullException("actualType");
            if (members == null)
                throw new ArgumentNullException("members");

            Attributes = actualType.GetCustomAttributes(true).ToList();
            ActualType = actualType;
            Members = members
                .OrderBy(m => m.Name)
                .ToList();
        }

        public IObjectContext CreateFor(object target)
        {
            return new XmlSerializerObjectContext(this, target);
        }

        public IEnumerable<IMemberContext> GetMembers(object target)
        {
            return Members
                    .Select(m => m.CreateFor(target))
                    .ToList();
        }
    }
}