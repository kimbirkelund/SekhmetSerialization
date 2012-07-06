namespace Sekhmet.Serialization
{
    public interface IAdviceRequester
    {
        void RequestAdvice(AdviceRequestedEventArgs args);
    }
}