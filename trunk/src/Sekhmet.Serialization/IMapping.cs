namespace Sekhmet.Serialization
{
    public interface IMapping<out TSource, out TTarget>
    {
        bool AddTargetToParent { get; }
        TSource Source { get; }
        TTarget Target { get; }
    }
}