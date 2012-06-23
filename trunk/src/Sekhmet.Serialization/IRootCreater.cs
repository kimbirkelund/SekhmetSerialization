using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IRootCreater
    {
        XElement CreateRoot(IObjectContext source);
        void ValidateRoot(XElement source, IObjectContext target);
    }
}