using System.Collections.Generic;

namespace ContosoConference.Api.Entities
{
    public class Session
    {
        public Session()
        {
            Topics = new HashSet<Topic>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Timeslot { get; set; }
        public Speaker Speaker { get; set; }
        public ICollection<Topic> Topics { get; private set; }
    }
}
