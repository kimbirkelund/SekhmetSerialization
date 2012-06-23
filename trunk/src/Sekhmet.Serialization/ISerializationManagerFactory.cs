namespace Sekhmet.Serialization
{
    public interface ISerializationManagerFactory
    {
        ICompositeDeserializerSelector DeserializerSelector { get; }
        ICompositeObjectContextFactory ObjectContextFactory { get; }
        ICompositeTypeConverter TypeConverter { get; }

        ISerializationManager Create();
    }
}