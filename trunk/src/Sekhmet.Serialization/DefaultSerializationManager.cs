using System;
using System.Linq;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public class DefaultSerializationManager : ISerializationManager
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

        public object Deserialize(XElement source, Type targetType)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (targetType == null)
                throw new ArgumentNullException("targetType");

            var containerType = typeof(Container<>).MakeGenericType(targetType);
            var containerTypeObjectContext = _objectContextFactory.CreateForDeserialization(null, containerType);
            var containerTypeObjectContextMemberContext = containerTypeObjectContext.Members.Single();
            var deserializer = _deserializerSelector.Select(source, containerTypeObjectContextMemberContext);

            var actualTargetType = _typeConverter.GetActualType(source, containerTypeObjectContextMemberContext);

            var targetTypeObjectContext = _objectContextFactory.CreateForDeserialization(null, actualTargetType);

            _rootCreater.ValidateRoot(source, targetTypeObjectContext);

            if (deserializer == null)
                throw new ArgumentException("Unable to find deserializer for specified source.", "source");

            deserializer.Deserialize(source, containerTypeObjectContextMemberContext);

            return containerTypeObjectContextMemberContext.GetValue().GetObject();
        }

        public XElement Serialize(object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var containerType = typeof(Container<>).MakeGenericType(source.GetType());
            var container = Activator.CreateInstance(containerType, source);

            var containerObjectContext = _objectContextFactory.CreateForSerialization(null, container);
            var sourceMemberContext = containerObjectContext.Members.Single();
            var sourceObjectContext = sourceMemberContext.GetValue();

            var target = _rootCreater.CreateRoot(sourceObjectContext);

            var serializer = _serializerSelector.Select(sourceMemberContext, target);
            if (serializer == null)
                throw new ArgumentException("Unable to find serializer for specified object.", "source");

            serializer.Serialize(sourceMemberContext, target);

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