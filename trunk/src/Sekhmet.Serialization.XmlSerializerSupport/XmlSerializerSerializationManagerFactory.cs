﻿namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerSerializationManagerFactory : ISerializationManagerFactory
    {
        private readonly ISerializationManager _serializationManager;

        public ICompositeDeserializerSelector DeserializerSelector { get; private set; }
        public ICompositeObjectContextFactory ObjectContextFactory { get; private set; }
        public ICompositeSerializerSelector SerializerSelector { get; private set; }
        public ICompositeTypeConverter TypeConverter { get; private set; }

        public XmlSerializerSerializationManagerFactory(IInstantiator instantiator = null, IObjectContextFactory objectContextFactory = null)
        {
            instantiator = instantiator ?? new DefaultInstantiator();

            TypeConverter = new CompositeTypeConverter();
            TypeConverter.AddConverter(new XmlSerializerTypeConverter());
            TypeConverter.AddConverter(new IdentityTypeConverter());

            ObjectContextFactory = new CompositeObjectContextFactory();
            ObjectContextFactory.AddFactory(new BuiltInCollectionsObjectContextFactory(instantiator, ObjectContextFactory));

            objectContextFactory = objectContextFactory ?? new CachingObjectContextFactory(instantiator, new XmlSerializerObjectContextInfoFactory(), ObjectContextFactory);
            ObjectContextFactory.AddFactory(objectContextFactory);

            var collectionMapper = new XmlSerializerCollectionMapper();
            var mapper = new XmlSerializerMapper();

            SerializerSelector = new CompositeSerializerSelector();
            SerializerSelector.AddSelector(new XmlSerializableSerializerSelector());
            SerializerSelector.AddSelector(new CollectionSerializerSelector(collectionMapper, SerializerSelector, new XmlSerializerIsNullableStrategy()));
            SerializerSelector.AddSelector(new ValueTypeSerializerSelector(new XmlSerializerIsNullableStrategy()));
            SerializerSelector.AddSelector(new RecursiveSerializerSelector(mapper, SerializerSelector, new XmlSerializerIsNullableStrategy()));

            DeserializerSelector = new CompositeDeserializerSelector();
            DeserializerSelector.AddSelector(new XmlSerializableDeserializerSelector(ObjectContextFactory, TypeConverter));
            DeserializerSelector.AddSelector(new CollectionDeserializerSelector(collectionMapper, DeserializerSelector, ObjectContextFactory));
            DeserializerSelector.AddSelector(new ValueTypeDeserializerSelector());
            DeserializerSelector.AddSelector(new XmlSerializerValueTypeDeserializerSelector());
            DeserializerSelector.AddSelector(new RecursiveDeserializerSelector(mapper, ObjectContextFactory, DeserializerSelector, TypeConverter));

            _serializationManager = new DefaultSerializationManager(ObjectContextFactory, DeserializerSelector, SerializerSelector, new XmlSerializerRootCreater(), TypeConverter);
        }

        public ISerializationManager Create()
        {
            return _serializationManager;
        }
    }
}