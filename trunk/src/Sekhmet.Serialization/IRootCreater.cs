using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IRootCreater
    {
        XElement CreateRoot(IObjectContext source, IAdviceRequester adviceRequester);
        void ValidateRoot(XElement source, IObjectContext target, IAdviceRequester adviceRequester);
    }
}