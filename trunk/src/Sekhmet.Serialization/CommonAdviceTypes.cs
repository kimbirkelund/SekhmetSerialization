namespace Sekhmet.Serialization
{
    public static class CommonAdviceTypes
    {
        /// <summary>
        /// Meta-type that represents all types.
        /// </summary>
        public static readonly AdviceType All = "All";

        /// <summary>
        /// Represents the case where multiple matches for a given member was found during deserialization, 
        /// and no automatic disambiguation could be found.
        /// If no advice is given the first match is used.
        /// </summary>
        public static readonly AdviceType MultipleMatches = "MultipleMatches";
    }
}