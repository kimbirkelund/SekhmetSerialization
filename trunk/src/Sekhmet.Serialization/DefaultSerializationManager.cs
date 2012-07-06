using System;
using System.Linq;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class DefaultSerializationManager : SerializationManagerBase
    {
        private readonly IDeserializerSelector _deserializerSelector;
        private readonly ICompositeObjectContextFactory _objectContextFactory;
        private readonly IRootCreater _rootCreater;
        private readonly ISerializerSelector _serializerSelector;
        private readonly ITypeConverter _typeConverter;

        public DefaultSerializationManager(ICompositeObjectContextFactory objectContextFactory, IDeserializerSelector deserializerSelector, ISerializerSelector serializerSelector,
                                           IRootCreater rootCreater, ITypeConverter typeConverter)
        {
            if (objectContextFactory == null)
                throw new ArgumentNullException("objectContextFactory");
            if (deserializerSelector == null)
                throw new ArgumentNullException("deserializerSelector");
            if (serializerSelector == null)
                throw new ArgumentNullException("serializerSelector");

            _objectContextFactory = objectContextFactory;
            _deserializerSelector = deserializerSelector;
            _serializerSelector = serializerSelector;
            _rootCreater = rootCreater;
            _typeConverter = typeConverter;
        }

        public override object Deserialize(XElement source, Type targetType)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (targetType == null)
                throw new ArgumentNullException("targetType");

            Type containerType = typeof(Container<>).MakeGenericType(targetType);
            IObjectContext containerTypeObjectContext = _objectContextFactory.CreateForDeserialization(null, containerType, AdviceRequester);
            IMemberContext containerTypeObjectContextMemberContext = containerTypeObjectContext.Members.Single();
            IDeserializer deserializer = _deserializerSelector.Select(source, containerTypeObjectContextMemberContext, AdviceRequester);

            Type actualTargetType = _typeConverter.GetActualType(source, containerTypeObjectContextMemberContext, AdviceRequester);

            IObjectContext targetTypeObjectContext = _objectContextFactory.CreateForDeserialization(null, actualTargetType, AdviceRequester);

            _rootCreater.ValidateRoot(source, targetTypeObjectContext, AdviceRequester);

            if (deserializer == null)
                throw new ArgumentException("Unable to find deserializer for specified source.", "source");

            deserializer.Deserialize(source, containerTypeObjectContextMemberContext, AdviceRequester);

            return containerTypeObjectContextMemberContext.GetValue().GetObject();
        }

        public override XElement Serialize(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            Type containerType = typeof(Container<>).MakeGenericType(source.GetType());
            object container = Activator.CreateInstance(containerType, source);

            IObjectContext containerObjectContext = _objectContextFactory.CreateForSerialization(null, container, AdviceRequester);
            IMemberContext sourceMemberContext = containerObjectContext.Members.Single();
            IObjectContext sourceObjectContext = sourceMemberContext.GetValue();

            XElement target = _rootCreater.CreateRoot(sourceObjectContext, AdviceRequester);

            ISerializer serializer = _serializerSelector.Select(sourceMemberContext, target, AdviceRequester);
            if (serializer == null)
                throw new ArgumentException("Unable to find serializer for specified object.", "source");

            serializer.Serialize(sourceMemberContext, target, AdviceRequester);

            return target;
        }

        private class Container<T>
        {
            // Used by reflection
            // ReSharper disable UnusedMember.Local
            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public T Value { get; set; }

            public Container() { }

            public Container(T value)
            {
                Value = value;
            }

            // ReSharper restore UnusedAutoPropertyAccessor.Local
            // ReSharper restore MemberCanBePrivate.Local
            // ReSharper restore UnusedMember.Local
        }
    }
}