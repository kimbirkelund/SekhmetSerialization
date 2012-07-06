using System;

namespace Sekhmet.Serialization
{
    public class AdviceRequestedEventArgs : EventArgs
    {
        public AdviceType Type { get; private set; }

        public AdviceRequestedEventArgs(AdviceType type)
        {
            Type = type;
        }
    }
}