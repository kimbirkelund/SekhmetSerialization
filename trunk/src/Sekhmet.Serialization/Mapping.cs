namespace Sekhmet.Serialization
{
    public class Mapping<TSource, TTarget> : IMapping<TSource, TTarget>
    {
        public bool AddTargetToParent { get; private set; }
        public TSource Source { get; private set; }
        public TTarget Target { get; private set; }

        public Mapping(TSource source, TTarget target, bool addTargetToParent = true)
        {
            AddTargetToParent = addTargetToParent;
            Source = source;
            Target = target;
        }
    }
}