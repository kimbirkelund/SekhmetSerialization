using System;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public abstract class SerializationManagerBase : ISerializationManager
    {
        private readonly DefaultAdviceRequester _adviceRequester;

        public void AddAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types)
        {
            _adviceRequester.AddAdvisor(advisor, types);
        }

        public abstract object Deserialize(XElement source, Type targetType);

        public void RemoveAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types)
        {
            _adviceRequester.RemoveAdvisor(advisor, types);
        }

        public abstract XElement Serialize(object source);

        protected SerializationManagerBase()
        {
            _adviceRequester = new DefaultAdviceRequester(this);
        }

        protected IAdviceRequester AdviceRequester
        {
            get { return _adviceRequester; }
        }
    }
}