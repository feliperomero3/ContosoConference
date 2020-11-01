using System.Collections.Generic;

namespace ContosoConference.Api.Entities
{
    public class Session : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Timeslot { get; set; }
        public Speaker Speaker { get; set; }
        public ICollection<Topic> Topics { get; private set; }

        private Session()
        {
            Topics = new HashSet<Topic>();
        }

        public Session(string title,
            string description,
            string timeslot,
            Speaker speaker,
            ICollection<Topic> topics = null) : this()
        {
            Title = title;
            Description = description;
            Timeslot = timeslot;
            Speaker = speaker;
            Topics = topics;
        }
    }
}
