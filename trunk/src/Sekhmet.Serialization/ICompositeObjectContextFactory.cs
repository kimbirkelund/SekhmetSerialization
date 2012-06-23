using System.Collections.Generic;

namespace Sekhmet.Serialization
{
    public interface ICompositeObjectContextFactory : IObjectContextFactory
    {
        IEnumerable<IObjectContextFactory> Factories { get; }

        void AddFactory(IObjectContextFactory selector, int? index = null);
        void RemoveFactory(IObjectContextFactory selector);
    }
}