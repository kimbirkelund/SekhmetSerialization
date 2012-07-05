using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public class ObjectContext : IObjectContext
    {
        private readonly ObjectContextInfo _contextInfo;

        private IEnumerable<IMemberContext> _member;

        public Type Type
        {
            get { return _contextInfo.ActualType; }
        }

        public IEnumerable<object> Attributes
        {
            get { return _contextInfo.Attributes; }
        }

        public IEnumerable<IMemberContext> Members
        {
            get { return _member ?? (_member = _contextInfo.GetMembers(Target)); }
        }

        public object Target { get; private set; }

        public ObjectContext(ObjectContextInfo contextInfo, object target)
        {
            if (contextInfo == null)
                throw new ArgumentNullException("contextInfo");
            if (target == null)
                throw new ArgumentNullException("target");

            _contextInfo = contextInfo;
            Target = target;
        }

        public object GetObject()
        {
            return Target;
        }
    }
}