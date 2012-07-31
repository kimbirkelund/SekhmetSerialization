using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Sekhmet.Serialization.Advicing
{
    public class MultipleMatchesAdviceRequestedEventArgs : AdviceRequestedEventArgs
    {
        public IEnumerable<XObject> Matches { get; private set; }
        public XObject SelectedMatch { get; set; }
        public XElement Source { get; private set; }
        public IMemberContext Target { get; private set; }
        public IObjectContext TargetOwner { get; private set; }

        public MultipleMatchesAdviceRequestedEventArgs(XElement source, IObjectContext targetOwner, IMemberContext target, IEnumerable<XObject> matches, XObject selectedMatch)
            : base(CommonAdviceTypes.MultipleMatches)
        {
            Source = source;
            TargetOwner = targetOwner;
            Target = target;
            Matches = (matches ?? Enumerable.Empty<XObject>()).ToList();
            SelectedMatch = selectedMatch;
        }
    }
}