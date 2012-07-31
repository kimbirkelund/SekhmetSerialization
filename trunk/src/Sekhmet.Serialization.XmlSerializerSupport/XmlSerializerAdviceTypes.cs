namespace Sekhmet.Serialization.XmlSerializerSupport
{
    public class XmlSerializerAdviceTypes
    {
        /// <summary>
        /// Represents the case where the <see cref="XmlParentAttribute"/> has been used on a 
        /// property or field with a different type than the actual parent instance.
        /// </summary>
        public static readonly AdviceType IncompatibleParentType = "IncompatibleParentType";
    }
}