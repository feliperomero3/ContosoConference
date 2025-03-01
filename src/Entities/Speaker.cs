namespace ContosoConference.Api.Entities;

public class Speaker : Entity
{
    public string Name { get; set; }
    public string Bio { get; set; }

    private Speaker()
    {
    }

    public Speaker(string name, string bio) : this()
    {
        Name = name;
        Bio = bio;
    }
}