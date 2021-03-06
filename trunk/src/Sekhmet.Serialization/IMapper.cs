﻿using System.Collections.Generic;
using System.Xml.Linq;

namespace Sekhmet.Serialization
{
    public interface IMapper
    {
        IEnumerable<IMapping<XObject, IMemberContext>> MapForDeserialization(XElement source, IMemberContext target, IAdviceRequester adviceRequester);
        IEnumerable<IMapping<IMemberContext, XObject>> MapForSerialization(IMemberContext source, XElement target, IAdviceRequester adviceRequester);
    }
}