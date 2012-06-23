using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class MemberContextInfo
    {
        private readonly Func<XmlSerializerMemberContext, IObjectContext> _getter;
        private readonly Action<XmlSerializerMemberContext, IObjectContext> _setter;

        public IEnumerable<object> Attributes { get; private set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public MemberContextInfo(Type type, string name, IEnumerable<object> attributes, Func<XmlSerializerMemberContext, IObjectContext> getter, Action<XmlSerializerMemberContext, IObjectContext> setter)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            if (attributes == null)
                throw new ArgumentNullException("attributes");
            if (getter == null)
                throw new ArgumentNullException("getter");
            if (setter == null)
                throw new ArgumentNullException("setter");

            Type = type;
            Name = name;
            Attributes = attributes;
            _getter = getter;
            _setter = setter;
        }

        public IMemberContext CreateFor(object target)
        {
            return new XmlSerializerMemberContext(this, target);
        }

        public IObjectContext GetValue(XmlSerializerMemberContext context)
        {
            return _getter(context);
        }

        public void SetValue(XmlSerializerMemberContext context, IObjectContext value)
        {
            _setter(context, value);
        }
    }
}