using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public class MemberContextInfo
    {
        private readonly Func<MemberContext, IObjectContext> _getter;
        private readonly Action<MemberContext, IObjectContext> _setter;

        public IEnumerable<object> Attributes { get; private set; }
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public MemberContextInfo(Type type, string name, IEnumerable<object> attributes, Func<MemberContext, IObjectContext> getter, Action<MemberContext, IObjectContext> setter)
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
            return new MemberContext(this, target);
        }

        public IObjectContext GetValue(MemberContext context)
        {
            return _getter(context);
        }

        public void SetValue(MemberContext context, IObjectContext value)
        {
            _setter(context, value);
        }
    }
}