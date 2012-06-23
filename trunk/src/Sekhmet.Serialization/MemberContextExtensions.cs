using System;

namespace Sekhmet.Serialization
{
    public static class MemberContextExtensions
    {
        public static Type GetActualType(this IMemberContext context)
        {
            var objectContext = context.GetValue();
            if (objectContext == null)
                return null;

            object objectContextObject = objectContext.GetObject();
            if (objectContextObject == null)
                return null;

            return objectContextObject.GetType();
        }

        public static object GetActualValue(this IMemberContext context)
        {
            var objectContext = context.GetValue();
            if (objectContext == null)
                return null;

            return objectContext.GetObject();
        }
    }
}