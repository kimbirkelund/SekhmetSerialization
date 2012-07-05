using System;
using System.Collections.Generic;
using System.Linq;

namespace Sekhmet.Serialization
{
    public class ObjectContextInfo
    {
        public Type ActualType { get; private set; }

        public IEnumerable<object> Attributes { get; private set; }
        public IEnumerable<MemberContextInfo> Members { get; private set; }

        public ObjectContextInfo(Type actualType, IEnumerable<object> attributes, IEnumerable<MemberContextInfo> members)
        {
            if (actualType == null)
                throw new ArgumentNullException("actualType");
            if (members == null)
                throw new ArgumentNullException("members");

            ActualType = actualType;
            Attributes = (attributes ?? Enumerable.Empty<object>()).ToList();
            Members = members
                .OrderBy(m => m.Name)
                .ToList();
        }

        public IObjectContext CreateFor(object target)
        {
            return new ObjectContext(this, target);
        }

        public IEnumerable<IMemberContext> GetMembers(object target)
        {
            return Members
                .Select(m => m.CreateFor(target))
                .ToList();
        }
    }
}