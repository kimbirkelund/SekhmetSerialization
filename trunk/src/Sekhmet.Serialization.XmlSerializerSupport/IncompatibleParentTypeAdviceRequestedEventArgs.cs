using System;

namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class IncompatibleParentTypeAdviceRequestedEventArgs : AdviceRequestedEventArgs
    {
        public Type ExpectedParentType { get; private set; }
        public object Parent { get; set; }

        public IncompatibleParentTypeAdviceRequestedEventArgs(object parent, Type expectedType)
            : base(XmlSerializerAdviceTypes.IncompatibleParentType)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (expectedType == null)
                throw new ArgumentNullException("expectedType");

            Parent = parent;
            ExpectedParentType = expectedType;
        }
    }
}