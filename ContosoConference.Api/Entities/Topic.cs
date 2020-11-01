namespace ContosoConference.Api.Entities
{
    public class Topic : Entity
    {
        public string Name { get; set; }

        private Topic()
        {
        }

        public Topic(string name) : this()
        {
            Name = name;
        }
    }
}
