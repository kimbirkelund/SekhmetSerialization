using System;
using System.Collections.Generic;
using System.Linq;
using Sekhmet.Serialization.Utility;

namespace Sekhmet.Serialization
{
    public class DefaultAdviceRequester : IAdviceRequester
    {
        private readonly object _adviceRequestSender;

        private readonly IList<AdvisorInfo> _advisors = new List<AdvisorInfo>();
        private readonly ReadWriteLock _lock = new ReadWriteLock();

        public DefaultAdviceRequester(object adviceRequestSender)
        {
            _adviceRequestSender = adviceRequestSender;
        }

        public void AddAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types)
        {
            if (advisor == null)
                throw new ArgumentNullException("advisor");
            if (types.Length == 0)
                throw new ArgumentOutOfRangeException("types", "Must specify one or more types to listen to.");

            using (_lock.EnterWriteScope())
            {
                AdvisorInfo existingAdvisor = _advisors
                    .Where(ai => ai.Advisor == advisor)
                    .FirstOrDefault();

                if (existingAdvisor == null)
                {
                    existingAdvisor = new AdvisorInfo(advisor);
                    _advisors.Add(existingAdvisor);
                }

                if (existingAdvisor.Types.Contains(CommonAdviceTypes.All))
                    return;
                if (types.Contains(CommonAdviceTypes.All))
                {
                    existingAdvisor.Types.Clear();
                    existingAdvisor.Types.Add(CommonAdviceTypes.All);
                    return;
                }

                List<AdviceType> combinedTypes = existingAdvisor.Types.Concat(types)
                    .Distinct()
                    .ToList();

                existingAdvisor.Types.Clear();
                existingAdvisor.Types.AddRange(combinedTypes);
            }
        }

        public IEnumerable<AdvisorInfo> Advisors
        {
            get
            {
                using (_lock.EnterReadScope())
                    return _advisors.ToList();
            }
        }

        public void RemoveAdvisor(EventHandler<AdviceRequestedEventArgs> advisor, params AdviceType[] types)
        {
            if (advisor == null)
                throw new ArgumentNullException("advisor");
            if (types.Length == 0)
                throw new ArgumentOutOfRangeException("types", "Must specify one or more types to listen to.");

            using (_lock.EnterWriteScope())
            {
                AdvisorInfo existingAdvisor = _advisors
                    .Where(ai => ai.Advisor == advisor)
                    .FirstOrDefault();

                if (existingAdvisor == null)
                    return;

                if (types.Contains(CommonAdviceTypes.All))
                {
                    _advisors.Remove(existingAdvisor);
                    return;
                }
                if (existingAdvisor.Types.Contains(CommonAdviceTypes.All))
                    return;

                List<AdviceType> combinedTypes = existingAdvisor.Types.Except(types)
                    .Distinct()
                    .ToList();

                existingAdvisor.Types.Clear();
                existingAdvisor.Types.AddRange(combinedTypes);

                if (!existingAdvisor.Types.Any())
                    _advisors.Remove(existingAdvisor);
            }
        }

        public void RequestAdvice(AdviceRequestedEventArgs args)
        {
            using (_lock.EnterReadScope())
            {
                foreach (AdvisorInfo advisor in _advisors)
                {
                    if (!advisor.Types.Contains(CommonAdviceTypes.All) && !advisor.Types.Contains(args.Type))
                        continue;

                    advisor.Advisor(_adviceRequestSender, args);
                }
            }
        }

        public sealed class AdvisorInfo
        {
            public AdvisorInfo(EventHandler<AdviceRequestedEventArgs> advisor)
            {
                Advisor = advisor;
                Types = new List<AdviceType>();
            }

            public EventHandler<AdviceRequestedEventArgs> Advisor { get; private set; }
            public List<AdviceType> Types { get; private set; }
        }
    }
}