using System;
using System.Collections.Generic;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerMemberContext : IMemberContext
    {
        private readonly MemberContextInfo _contextInfo;
        private IObjectContext _value;

        public IEnumerable<object> Attributes
        {
            get { return _contextInfo.Attributes; }
        }

        public Type ContractType
        {
            get { return _contextInfo.Type; }
        }

        public string Name
        {
            get { return _contextInfo.Name; }
        }

        public object Target { get; private set; }

        public XmlSerializerMemberContext(MemberContextInfo contextInfo, object target)
        {
            if (contextInfo == null)
                throw new ArgumentNullException("contextInfo");
            if (target == null)
                throw new ArgumentNullException("target");

            _contextInfo = contextInfo;
            Target = target;
        }

        public void CommitChanges()
        {
            if (_value != null)
                _contextInfo.SetValue(this, _value);
        }

        public IObjectContext GetValue()
        {
            return _value ?? (_value = _contextInfo.GetValue(this));
        }

        public void SetValue(IObjectContext value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return ContractType + " " + Name;
        }
    }
}